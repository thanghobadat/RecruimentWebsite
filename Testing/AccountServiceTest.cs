using Application.Common;
using Application.System.Users;
using AutoMapper;
using Data.EF;
using Data.Entities;
using Data.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.System.Users;

namespace Testing
{
    [TestFixture]
    internal class AccountServiceTest
    {
        private IAccountService _accountService;
        private RecruimentWebsiteDbContext _context;
        private IStorageService _storageService;
        private static IMapper _mapper;
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;
        private RoleManager<AppRole> _roleManager;
        public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));
            mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.RemovePasswordAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.AddPasswordAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.AddToRoleAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            mgr.Setup(x => x.ChangePasswordAsync(It.IsAny<TUser>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            return mgr;
        }
        [OneTimeSetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<RecruimentWebsiteDbContext>()
                .UseInMemoryDatabase(databaseName: "DatabaseName")
                .Options;
            _context = new RecruimentWebsiteDbContext(options);

            var listAppUser = new List<AppUser>();
            var companyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17");
            var hasher1 = new PasswordHasher<AppUser>();
            _context.Users.Add(new AppUser
            {
                Id = companyId,
                UserName = "company",
                NormalizedUserName = "company",
                Email = "hoangthanhdev123@gmail.com",
                NormalizedEmail = "hoangthanhdev123@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher1.HashPassword(null, "Admin@123"),
                SecurityStamp = string.Empty,
                DateCreated = DateTime.Now,
                PhoneNumber = "0123222",
            });


            var userId = new Guid("5e9962d7-8b63-4e1a-943c-24c17d2f25de");
            var hasher = new PasswordHasher<AppUser>();
            _context.Users.Add(new AppUser
            {
                Id = userId,
                UserName = "user",
                NormalizedUserName = "user1",
                Email = "hoangthanh01022000@gmail.com",
                NormalizedEmail = "hoangthanh01022000@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin@123"),
                SecurityStamp = string.Empty,
                DateCreated = DateTime.Now,
                PhoneNumber = "0123",
            });

            var roleId = new Guid("65272a1f-fa97-4d8c-bf8f-2754f55ed9f8");
            _context.Roles.Add(new AppRole
            {
                Id = roleId,
                Name = "company",
                NormalizedName = "company",
                ConcurrencyStamp = "fda77e60-c267-4252-ba3f-2a6d735690ee",
                Description = "description"
            });
            var role1Id = new Guid("f7cde378-6cff-4a68-81ba-34675d108a5c");
            _context.Roles.Add(new AppRole
            {
                Id = role1Id,
                Name = "user",
                NormalizedName = "user",
                ConcurrencyStamp = "9913bba0-8c62-4e89-abb1-fb4494223da8",
                Description = "description"
            });

            _context.UserRoles.Add(new IdentityUserRole<Guid>
            {
                UserId = userId,
                RoleId = role1Id
            });
            _context.UserRoles.Add(new IdentityUserRole<Guid>
            {
                UserId = companyId,
                RoleId = roleId
            });

            _context.SaveChanges();
            if (_storageService == null)
            {
                var mockStorage = new Mock<IStorageService>();
                _storageService = mockStorage.Object;
            }
            if (_userManager == null)
            {
                _userManager = MockUserManager<AppUser>(listAppUser).Object;
            }
            _accountService = new AccountService(_userManager, _signInManager, _roleManager, _config, _context, _mapper, _storageService);
        }


        [Test]
        public async Task CanCreateUser()
        {
            // Arrange
            var request = new RegisterUserAccountRequest()
            {
                FirstName = "Ngô",
                LastName = "Thành",
                AcademicLevel = "12",
                Age = 21,
                Sex = Sex.Male,
                Address = "Hội An, Quảng Nam, Việt Nam",
                Email = "user1@gmail.com",
                PhoneNumber = "0123456",
                UserName = "user2",
                Password = "Admin@123",
                ConfirmPassword = "Admin@123"
            };
            // Act
            var result = await _accountService.RegisterUserAccount(request);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanCreateCompany()
        {
            // Arrange
            var request = new RegisterCompanyAccountRequest()
            {
                Name = "compay1",
                Description = "description",
                ContactName = "Thanh",
                WorkerNumber = 1111,
                Email = "user1@gmail.com",
                PhoneNumber = "0123456",
                UserName = "user2",
                Password = "Admin@123",
                ConfirmPassword = "Admin@123"
            };
            // Act
            var result = await _accountService.RegisterCompanyAccount(request);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanCreateAdmin()
        {
            // Arrange
            var request = new RegisterAdminAccountRequest()
            {
                Email = "user1@gmail.com",
                PhoneNumber = "0123456",
                UserName = "user2",
                Password = "Admin@123",
                ConfirmPassword = "Admin@123"
            };
            // Act
            var result = await _accountService.RegisterAdminAccount(request);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanChangePassword()
        {
            // Arrange
            var request = new ChangePasswordRequest()
            {
                Id = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17"),
                NewPassword = "Changepass123@123"
            };
            // Act
            var result = await _accountService.ChangePassword(request);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
    }
}
