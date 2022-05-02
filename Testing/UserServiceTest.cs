using Application.AutoMapper;
using Application.Catalog;
using Application.Common;
using AutoMapper;
using Data.EF;
using Data.Entities;
using Data.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ViewModel.Catalog.User;

namespace Testing
{
    [TestFixture]
    internal class UserServiceTest
    {
        private IUserService _userService;
        private RecruimentWebsiteDbContext _context;
        private IStorageService _storageService;
        private static IMapper _mapper;
        private UserManager<AppUser> _userManager;

        public UserServiceTest()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }


        }
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
            //mgr.Setup(x => x.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(It.IsAny<TUser>()));
            //mgr.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(It.IsAny<TUser>()));
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




            _context.UserInformations.Add(new UserInformation
            {
                UserId = new Guid("5e9962d7-8b63-4e1a-943c-24c17d2f25de"),
                FirstName = "Ngô",
                LastName = "Thành",
                AcademicLevel = "12",
                Age = 20,
                Sex = Sex.Male,
                Address = "Hội An, Quảng Nam"
            });
            _context.UserAvatars.Add(new UserAvatar
            {
                Id = 2,
                DateCreated = new DateTime(),
                ImagePath = "default-avatar.jpg",
                Caption = "image caption",
                FizeSize = 1,
                UserId = new Guid("5e9962d7-8b63-4e1a-943c-24c17d2f25de")
            });



            var listAppUser = new List<AppUser>();

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
            _userService = new UserService(_context, _storageService, _mapper, _userManager);
        }
        [Test]
        public async Task CanGetUserInformation()
        {
            // Arrange
            var userId = new Guid("5e9962d7-8b63-4e1a-943c-24c17d2f25de");
            // Act
            var result = await _userService.GetUserInformation(userId);
            // Assert
            Assert.IsTrue(result.IsSuccessed);
            Assert.IsNotNull(result.ResultObj);
        }
        [Test]
        public async Task CanGetUserAvatar()
        {
            // Arrange
            var userId = new Guid("5e9962d7-8b63-4e1a-943c-24c17d2f25de");
            // Act
            var result = await _userService.GetUserAvatar(userId);
            // Assert
            Assert.IsTrue(result.IsSuccessed);
            Assert.IsNotNull(result.ResultObj);
        }
        [Test]
        public async Task CanUpdateUserInformation()
        {
            // Arrange
            var request = new UserUpdateRequest()
            {
                UserId = new Guid("5e9962d7-8b63-4e1a-943c-24c17d2f25de"),
                FirstName = "Ngô",
                LastName = "Thành",
                AcademicLevel = "12",
                Age = 21,
                Sex = Sex.Male,
                Address = "Hội An, Quảng Nam, Việt Nam",
                Email = "hoangthanh01022000@gmail.com",
                PhoneNumber = "0123456"

            };
            // Act
            var result = await _userService.UpdateUserInformation(request);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanUpdateAvatar()
        {
            // Arrange
            int id = 1;

            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.jpg";
            var name = "test.jpg";
            var contentDisposition = "form-data; name=\"thumnailImage\"; filename=\"test.jpg\"";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(_ => _.Name).Returns(name);
            fileMock.Setup(_ => _.ContentDisposition).Returns(contentDisposition);

            var file = fileMock.Object;

            // Act
            var result = await _userService.UpdateUserAvatar(id, file);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanFollowCompany()
        {
            // Arrange
            var userId = new Guid("5e9962d7-8b63-4e1a-943c-24c17d2f25de");
            var companyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17");
            // Act
            var result = await _userService.FollowCompany(userId, companyId);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanChangePassword()
        {
            // Arrange
            var request = new ChangePasswordUserRequest()
            {
                UserId = new Guid("5e9962d7-8b63-4e1a-943c-24c17d2f25de"),
                OldPassword = "Admin@123",
                NewPassword = "NewPassW123"
            };
            // Act
            var result = await _userService.ChangePasswordUser(request);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        //[Test]
        //public async Task CanForogtPassword()
        //{
        //    // Arrange
        //    var request = new ForgotPasswordRequest()
        //    {
        //        UserId = new Guid("5e9962d7-8b63-4e1a-943c-24c17d2f25de"),
        //        Email = "hoangthanh01022000@gmail.com"
        //    };
        //    // Act
        //    var result = await _userService.ForgotPassword(request);
        //    // Assert
        //    Assert.IsTrue(result.ResultObj);
        //}
        [Test]
        public async Task CanSubmitCV()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var name = "test.pdf";
            var contentDisposition = "form-data; name=\"file\"; filename=\"test.pdf\"";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(_ => _.Name).Returns(name);
            fileMock.Setup(_ => _.ContentDisposition).Returns(contentDisposition);

            var file = fileMock.Object;
            var request = new SubmitCVRequest()
            {
                UserId = new Guid("5e9962d7-8b63-4e1a-943c-24c17d2f25de"),
                RecruitmentId = 2,
                File = file

            };
            // Act
            var result = await _userService.SubmitCV(request);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
    }
}
