using Application.Common;
using AutoMapper;
using Data.EF;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
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
        public async Task<ApiResult<LoginViewModel>> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null) return new ApiErrorResult<LoginViewModel>("Tài khoản này không tồn tại");
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded)
            {
                return new ApiErrorResult<LoginViewModel>("Sai mật khẩu, vui lòng nhập lại");
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
            var role = roles[0];
            var loginVM = new LoginViewModel()
            {
                Role = role,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Id = user.Id,
                UserName = request.UserName,
                Email = user.Email
            };


            return new ApiSuccessResult<LoginViewModel>(loginVM);
        }

        public async Task<ApiResult<bool>> ChangePassword(ChangePasswordRequest request)
        {
            var user = await _context.Users.FindAsync(request.Id);
            if (user == null)
            {
                return new ApiErrorResult<bool>("Account does not exits");
            }
            await _userManager.RemovePasswordAsync(user);
            var resultUser = await _userManager.AddPasswordAsync(user, request.NewPassword);
            if (!resultUser.Succeeded)
            {
                foreach (var error in resultUser.Errors)
                {
                    return new ApiErrorResult<bool>(error.Description);
                }

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
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                if (role == "company")
                {
                    var avatar = await _context.CompanyAvatars.FirstOrDefaultAsync(x => x.CompanyId == id);
                    if (avatar.ImagePath != "default-avatar")
                    {
                        await _storageService.DeleteAvatarAsync(avatar.ImagePath);
                    }
                    var coverImage = await _context.CompanyCoverImages.FirstOrDefaultAsync(x => x.CompanyId == id);
                    await _storageService.DeleteCoverImageAsync(coverImage.ImagePath);
                    var images = await _context.CompanyImages.Where(x => x.CompanyId == id).ToListAsync();
                    foreach (var image in images)
                    {
                        await _storageService.DeleteCoverImageAsync(image.ImagePath);
                    }
                }
                else
                {
                    var avatar = await _context.UserAvatars.FirstOrDefaultAsync(x => x.UserId == id);
                    if (avatar.ImagePath != "default-avatar")
                    {
                        await _storageService.DeleteAvatarAsync(avatar.ImagePath);
                    }
                }
            }
            var reult = await _userManager.DeleteAsync(user);
            if (reult.Succeeded)
                return new ApiSuccessResult<bool>();

            return new ApiErrorResult<bool>("Delete failed");
        }


        public async Task<ApiResult<bool>> RegisterCompanyAccount(RegisterCompanyAccountRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName);
            if (user != null)
            {
                return new ApiErrorResult<bool>("User name đã được sử dụng, vui lòng thử lại");
            }
            if (await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email) != null)
            {
                return new ApiErrorResult<bool>("Email này đã được sử dụng cho tài khoản khác, vui lòng chọn email khác");

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
            if (!resultUser.Succeeded)
            {
                foreach (var error in resultUser.Errors)
                {
                    return new ApiErrorResult<bool>(error.Description);
                }

            }
            await _userManager.AddToRoleAsync(user, "company");

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
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName);
            if (user != null)
            {
                return new ApiErrorResult<bool>("User name already exists, please choose another User name");
            }
            if (await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email) != null)
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
            if (!resultUser.Succeeded)
            {
                foreach (var error in resultUser.Errors)
                {
                    return new ApiErrorResult<bool>(error.Description);
                }

            }
            await _userManager.AddToRoleAsync(user, "user");
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
                await _userManager.DeleteAsync(user);
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

        public async Task<ApiResult<List<AccountViewModel>>> GetAllAccount()
        {
            var accountVMS = new List<AccountViewModel>();
            var companies = await _userManager.GetUsersInRoleAsync("company");

            var companyVMs = companies.Select(company => _mapper.Map<AccountViewModel>(company)).ToList();
            foreach (var companyVM in companyVMs)
            {
                companyVM.Role = "company";
                accountVMS.Add(companyVM);
            }

            var users = await _userManager.GetUsersInRoleAsync("user");
            var userVMs = users.Select(user => _mapper.Map<AccountViewModel>(user)).ToList();
            foreach (var userVM in userVMs)
            {
                userVM.Role = "user";
                accountVMS.Add(userVM);
            }
            var admins = await _userManager.GetUsersInRoleAsync("admin");
            var adminVMs = admins.Select(admin => _mapper.Map<AccountViewModel>(admin)).ToList();
            foreach (var adminVM in adminVMs)
            {
                adminVM.Role = "admin";
                accountVMS.Add(adminVM);
            }
            return new ApiSuccessResult<List<AccountViewModel>>(accountVMS);
        }
    }
}
