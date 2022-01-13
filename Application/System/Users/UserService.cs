using Data.EF;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Common;
using ViewModel.System.Users;

namespace Application.System.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly RecruimentWebsiteDbContext _context;
        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager, IConfiguration config, RecruimentWebsiteDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
            _context = context;
        }
        public async Task<string> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null) return "User doesn't exits";
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded)
            {
                return "Wrong username or password, please re-enter";
            }
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role,string.Join(";",roles)),
                new Claim(ClaimTypes.Name,request.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ApiResult<bool>> ChangePassword(Guid id, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("User does not exits");
            }
            await _userManager.RemovePasswordAsync(user);
            var resultUser = await _userManager.AddPasswordAsync(user, newPassword);
            if (!resultUser.Succeeded)
            {
                return new ApiErrorResult<bool>("An error occured, register unsuccessful");

            }
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("User does not exits");
            }
            var reult = await _userManager.DeleteAsync(user);
            if (reult.Succeeded)
                return new ApiSuccessResult<bool>();

            return new ApiErrorResult<bool>("Delete failed");
        }

        public async Task<ApiResult<AccountViewModel>> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<AccountViewModel>("User does not exist ");
            }


            var roleIList = await _userManager.GetRolesAsync(user);
            var roleList = roleIList.ToList();
            var role = string.Join("", roleList);

            if (role == "user")
            {
                var userInf = await _context.UserInformations.FindAsync(id);
                if (userInf == null)
                {
                    return new ApiErrorResult<AccountViewModel>("User does not have information exist.");
                }
                var userVm = new UserViewModel()
                {
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = userInf.FirstName,
                    LastName = userInf.LastName,
                    Sex = userInf.Sex,
                    AcademicLevel = userInf.AcademicLevel,
                    Age = userInf.Age,
                    DateCreated = user.DateCreated,
                    Address = user.Address,
                    Role = role
                };
                var accVm = new AccountViewModel()
                {
                    UserViewModel = userVm
                };
                return new ApiSuccessResult<AccountViewModel>(accVm);
            }
            else if (role == "company")
            {
                var companyInf = await _context.CompanyInformations.FindAsync(id);
                if (companyInf == null)
                {
                    return new ApiErrorResult<AccountViewModel>("Company does not have information exist.");
                }
                var companyVm = new CompanyViewModel()
                {
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Id = user.Id,
                    UserName = user.UserName,
                    Name = companyInf.Name,
                    ContactName = companyInf.ContactName,
                    WorkerNumber = companyInf.WorkerNumber,
                    Description = companyInf.Description,
                    DateCreated = user.DateCreated,
                    Address = user.Address,
                    Role = role
                };
                var accVm = new AccountViewModel()
                {
                    CompanyViewModel = companyVm
                };
                return new ApiSuccessResult<AccountViewModel>(accVm);
            }
            return new ApiErrorResult<AccountViewModel>("Your account can have more than 1 Roles. Please contact admin for support");

        }

        public async Task<ApiResult<PageResult<CompanyViewModel>>> GetCompanyAccountPaging(GetUserPagingRequest request)
        {

            var users = await _userManager.GetUsersInRoleAsync("company");
            foreach (AppUser user in users)
            {
                var userInf = await _context.CompanyInformations.FindAsync(user.Id);
                user.CompanyInformation = userInf;
            }
            var query = users.AsQueryable();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(request.Keyword)
                 || x.PhoneNumber.Contains(request.Keyword));
            }
            int totalRow = query.Count();

            var data = query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new CompanyViewModel()
                {
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    UserName = x.UserName,
                    Id = x.Id,
                    Address = x.Address,
                    DateCreated = x.DateCreated,
                    Name = x.CompanyInformation.Name,
                    Description = x.CompanyInformation.Description,
                    ContactName = x.CompanyInformation.ContactName,
                    WorkerNumber = x.CompanyInformation.WorkerNumber
                }).ToList();

            var pagedResult = new PageResult<CompanyViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PageResult<CompanyViewModel>>(pagedResult);
        }
        public async Task<ApiResult<PageResult<UserViewModel>>> GetUserAccountPaging(GetUserPagingRequest request)
        {

            var users = await _userManager.GetUsersInRoleAsync("user");

            foreach (AppUser user in users)
            {
                var userInf = await _context.UserInformations.FindAsync(user.Id);
                user.UserInformation = userInf;
            }

            var query = users.AsQueryable();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(request.Keyword)
                 || x.PhoneNumber.Contains(request.Keyword));
            }
            int totalRow = query.Count();

            var data = query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new UserViewModel()
                {
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    UserName = x.UserName,
                    Id = x.Id,
                    Address = x.Address,
                    FirstName = x.UserInformation.FirstName,
                    LastName = x.UserInformation.LastName,
                    Sex = x.UserInformation.Sex,
                    Age = x.UserInformation.Age,
                    DateCreated = x.DateCreated,
                    AcademicLevel = x.UserInformation.AcademicLevel,


                }).ToList();

            var pagedResult = new PageResult<UserViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PageResult<UserViewModel>>(pagedResult);
        }

        //public async Task<ApiResult<PageResult<UserViewModel>>> GetUserPaging(GetUserPagingRequest request)
        //{
        //    //convert thằng query dưới cho giống thằng trên
        //    //var query = _userManager.Users;
        //    var users = await _userManager.GetUsersInRoleAsync("company");
        //    var query = users.AsQueryable();

        //    if (!string.IsNullOrEmpty(request.Keyword))
        //    {
        //        query = query.Where(x => x.UserName.Contains(request.Keyword)
        //         || x.PhoneNumber.Contains(request.Keyword));
        //    }
        //    int totalRow = await query.CountAsync();

        //    var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
        //        .Take(request.PageSize)
        //        .Select(x => new UserViewModel()
        //        {
        //            Email = x.Email,
        //            PhoneNumber = x.PhoneNumber,
        //            UserName = x.UserName,
        //            Id = x.Id,
        //        }).ToListAsync();

        //    //4. Select and projection
        //    var pagedResult = new PageResult<UserViewModel>()
        //    {
        //        TotalRecords = totalRow,
        //        PageIndex = request.PageIndex,
        //        PageSize = request.PageSize,
        //        Items = data
        //    };
        //    return new ApiSuccessResult<PageResult<UserViewModel>>(pagedResult);
        //}

        public async Task<ApiResult<bool>> RegisterCompanyAccount(RegisterCompanyAccountRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null)
            {
                return new ApiErrorResult<bool>("User name already exists, please choose another User name");
            }
            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                return new ApiErrorResult<bool>("This email has already been applied to another account, please enter another email");

            }

            user = new AppUser()
            {
                DateCreated = DateTime.Now,
                Email = request.Email,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                IsSave = false
            };
            var resultUser = await _userManager.CreateAsync(user, request.Password);
            await _userManager.AddToRoleAsync(user, "company");
            if (!resultUser.Succeeded)
            {
                return new ApiErrorResult<bool>("An error occured, register unsuccessful");

            }

            var companyInf = new CompanyInformation()
            {
                UserId = user.Id,
                Name = request.Name,
                Description = request.Description,
                WorkerNumber = request.WorkerNumber,
                ContactName = request.ContactName,

            };

            await _context.CompanyInformations.AddAsync(companyInf);

            var resultUserInf = await _context.SaveChangesAsync();





            if (resultUserInf == 0)
            {
                return new ApiErrorResult<bool>("An error occured, register unsuccessful");
            }

            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> RegisterUserAccount(RegisterUserAccountRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null)
            {
                return new ApiErrorResult<bool>("User name already exists, please choose another User name");
            }
            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                return new ApiErrorResult<bool>("This email has already been applied to another account, please enter another email");

            }

            user = new AppUser()
            {
                DateCreated = DateTime.Now,
                Email = request.Email,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                IsSave = false
            };
            var resultUser = await _userManager.CreateAsync(user, request.Password);
            await _userManager.AddToRoleAsync(user, "user");
            if (!resultUser.Succeeded)
            {
                return new ApiErrorResult<bool>("An error occured, register unsuccessful");

            }
            var userInf = new UserInformation()
            {
                UserId = user.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                AcademicLevel = request.AcademicLevel,
                Age = request.Age,
                Sex = request.Sex,

            };

            await _context.UserInformations.AddAsync(userInf);

            var resultUserInf = await _context.SaveChangesAsync();
            if (resultUserInf == 0)
            {
                return new ApiErrorResult<bool>("An error occured, register unsuccessful");
            }
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateCompany(Guid id, CompanyUpdateRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            user.Address = request.Address;


            var userInf = await _context.CompanyInformations.FindAsync(id);
            userInf.Name = request.Name;
            userInf.Description = request.Description;
            userInf.WorkerNumber = request.WorkerNumber;
            userInf.ContactName = request.ContactName;
            var resultUserInf = await _context.SaveChangesAsync();

            if (resultUserInf == 0)
            {
                return new ApiErrorResult<bool>("An error occured, register unsuccessful");
            }
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest request)
        {

            var user = await _userManager.FindByIdAsync(id.ToString());
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            user.Address = request.Address;


            var userInf = await _context.UserInformations.FindAsync(id);
            userInf.Age = request.Age;
            userInf.FirstName = request.FirstName;
            userInf.AcademicLevel = request.AcademicLevel;
            userInf.LastName = request.LastName;
            userInf.Sex = request.Sex;
            var resultUserInf = await _context.SaveChangesAsync();



            if (resultUserInf == 0)
            {
                return new ApiErrorResult<bool>("An error occured, register unsuccessful");
            }
            return new ApiSuccessResult<bool>(true);
        }
    }
}
