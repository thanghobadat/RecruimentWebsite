using Application.Common;
using AutoMapper;
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
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly RecruimentWebsiteDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStorageService _storageService;
        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager, IConfiguration config, RecruimentWebsiteDbContext context,
            IMapper mapper, IStorageService storageService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
            _context = context;
            _mapper = mapper;
            _storageService = storageService;
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
            //var roles = await _userManager.GetRolesAsync(user);
            //foreach (var role in roles)
            //{
            //    if (role == "company")
            //    {
            //        await _storageService.DeleteAvatarAsync()
            //    }
            //}
            var reult = await _userManager.DeleteAsync(user);
            if (reult.Succeeded)
                return new ApiSuccessResult<bool>();

            return new ApiErrorResult<bool>("Delete failed");
        }

        public async Task<ApiResult<UserAccountViewModel>> GetUserById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<UserAccountViewModel>("User does not exist ");
            }

            var userVm = _mapper.Map<UserAccountViewModel>(user);

            return new ApiSuccessResult<UserAccountViewModel>(userVm);
        }

        public async Task<ApiResult<CompanyAccountViewModel>> GetCompanyById(Guid id)
        {
            var company = await _userManager.FindByIdAsync(id.ToString());
            if (company == null)
            {
                return new ApiErrorResult<CompanyAccountViewModel>("User does not exist ");
            }

            var companyVM = _mapper.Map<CompanyAccountViewModel>(company);

            return new ApiSuccessResult<CompanyAccountViewModel>(companyVM);
        }


        public async Task<ApiResult<PageResult<CompanyAccountViewModel>>> GetCompanyAccountPaging(GetAccountPagingRequest request)
        {


            var companies = await _userManager.GetUsersInRoleAsync("company");

            var query = companies.AsQueryable();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(request.Keyword));
            }
            int totalRow = query.Count();

            var data = query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(company => _mapper.Map<CompanyAccountViewModel>(company)).ToList();

            var pagedResult = new PageResult<CompanyAccountViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PageResult<CompanyAccountViewModel>>(pagedResult);
        }
        public async Task<ApiResult<PageResult<UserAccountViewModel>>> GetUserAccountPaging(GetAccountPagingRequest request)
        {
            var users = await _userManager.GetUsersInRoleAsync("user");


            var query = users.AsQueryable();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(request.Keyword));
            }
            int totalRow = query.Count();


            var data = query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(user => _mapper.Map<UserAccountViewModel>(user)).ToList();

            var pagedResult = new PageResult<UserAccountViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PageResult<UserAccountViewModel>>(pagedResult);
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
            var companyAvatar = new CompanyAvatar()
            {
                CompanyId = user.Id,
                FizeSize = 1,
                DateCreated = DateTime.Now,
                ImagePath = "default-avatar.jpg",
                Caption = "default-avatar"
            };
            await _context.CompanyAvatars.AddAsync(companyAvatar);

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
                Address = request.Address

            };
            await _context.UserInformations.AddAsync(userInf);
            var userAvatar = new UserAvatar()
            {
                UserId = user.Id,
                FizeSize = 1,
                DateCreated = DateTime.Now,
                ImagePath = "default-avatar.jpg",
                Caption = "default-avatar"
            };
            await _context.UserAvatars.AddAsync(userAvatar);
            var resultUserInf = await _context.SaveChangesAsync();
            if (resultUserInf == 0)
            {
                return new ApiErrorResult<bool>("An error occured, register unsuccessful");
            }
            return new ApiSuccessResult<bool>(true);
        }





        public async Task<ApiResult<bool>> RegisterAdminAccount(RegisterAdminAccountRequest request)
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
                IsSave = false
            };
            var resultUser = await _userManager.CreateAsync(user, request.Password);
            await _userManager.AddToRoleAsync(user, "admin");
            if (!resultUser.Succeeded)
            {
                return new ApiErrorResult<bool>("An error occured, register unsuccessful");

            }

            return new ApiSuccessResult<bool>(true);
        }
    }
}
