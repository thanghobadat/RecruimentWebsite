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
using ViewModel.Catalog.Company;

namespace Testing
{
    [TestFixture]
    internal class CompanyServiceTest
    {
        private ICompanyService _companyService;
        private RecruimentWebsiteDbContext _context;
        private IStorageService _storageService;
        private static IMapper _mapper;
        private IUserService _userService;
        private UserManager<AppUser> _userManager;

        public CompanyServiceTest()
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
            mgr.Setup(x => x.GetUsersInRoleAsync(It.IsAny<string>())).Returns(Task.FromResult(It.IsAny<IList<TUser>>()));
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

            var userId1 = new Guid("e8eed1d4-73ad-46ca-0b79-08da23c12f17");
            var hasher1 = new PasswordHasher<AppUser>();
            _context.Users.Add(new AppUser
            {
                Id = userId1,
                UserName = "user3",
                NormalizedUserName = "user3",
                Email = "thanhngo1@gmail.com",
                NormalizedEmail = "thanhngo1@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher1.HashPassword(null, "Admin@123"),
                SecurityStamp = string.Empty,
                DateCreated = DateTime.Now,
                PhoneNumber = "012355",
            });

            _context.UserInformations.Add(new UserInformation
            {
                UserId = new Guid("e8eed1d4-73ad-46ca-0b79-08da23c12f17"),
                FirstName = "Ngô",
                LastName = "Thành 1",
                AcademicLevel = "12",
                Age = 20,
                Sex = Sex.Male,
                Address = "Hội An, Quảng Nam"
            });
            _context.UserAvatars.Add(new UserAvatar
            {
                Id = 4,
                DateCreated = new DateTime(),
                ImagePath = "default-avatar.jpg",
                Caption = "image caption",
                FizeSize = 1,
                UserId = new Guid("e8eed1d4-73ad-46ca-0b79-08da23c12f17")
            });

            _context.CompanyInformations.Add(new CompanyInformation
            {
                UserId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17"),
                Name = "Company 1",
                Description = "company Description",
                WorkerNumber = 1122,
                ContactName = "Thanh pro"
            });

            _context.CompanyAvatars.Add(new CompanyAvatar
            {
                Id = 2,
                DateCreated = new DateTime(),
                ImagePath = "default-avatar.jpg",
                Caption = "image caption",
                FizeSize = 1,
                CompanyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17")
            });

            _context.CompanyCoverImages.Add(new CompanyCoverImage
            {
                Id = 2,
                DateCreated = new DateTime(),
                ImagePath = "default-coverImage.jpg",
                Caption = "image caption",
                FizeSize = 1,
                CompanyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17")
            });
            _context.CompanyCoverImages.Add(new CompanyCoverImage
            {
                Id = 3,
                DateCreated = new DateTime(),
                ImagePath = "default-coverImage.jpg",
                Caption = "image caption",
                FizeSize = 1,
                CompanyId = new Guid("789f01f0-f711-4e5d-0b82-08da23c12f17")
            });

            _context.CompanyImages.Add(new CompanyImage
            {
                Id = 2,
                DateCreated = new DateTime(),
                ImagePath = "default-coverImage.jpg",
                Caption = "image caption",
                FizeSize = 1,
                CompanyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17")

            });

            _context.CompanyBranches.Add(new CompanyBranch
            {
                CompanyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17"),
                BranchId = 3,
                Address = "Address"
            });


            _context.MailSettings.Add(new MailSetting
            {
                Id = 1,
                Email = "hoangthanh01022000@gmail.com",
                DisplayName = "thanh",
                Password = "Thanhngo@123",
                Host = "smtp.gmail.com",
                Port = 587

            });

            _context.Recruitments.Add(new Recruitment
            {
                Id = 2,
                Name = "Recruitment name",
                Rank = "Nhân viên",
                Experience = "2 - 5 năm",
                DetailedExperience = "detail exporience",
                Benefits = "benefits",
                Salary = 10000,
                Education = "Cao đẳng / Đại học",
                Type = "Toàn thời gian",
                Description = "description",
                ExpirationDate = new DateTime(2022, 05, 10),
                DateCreated = new DateTime(),
                CompanyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17")
            });

            _context.Comments.Add(new Comment
            {
                Id = 2,
                RecruimentId = 2,
                AccountId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17"),
                Content = "content",
                DateCreated = new DateTime(2022, 05, 01),
                SubcommentId = null
            });


            _context.Chats.Add(new Chat
            {
                Id = 1,
                UserId = new Guid("5e9962d7-8b63-4e1a-943c-24c17d2f25de"),
                CompanyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17"),
                Content = "Xin chào",
                DateCreated = DateTime.Now,
                Performer = "user"
            });

            _context.CareerRecruitments.Add(new CareerRecruitment
            {
                RecruimentId = 2,
                CareerId = 3
            });
            _context.BranchRecruitments.Add(new BranchRecruitment
            {
                RecruimentId = 2,
                BranchId = 3
            });

            _context.CurriculumVitaes.Add(new CurriculumVitae
            {
                UserId = new Guid("e8eed1d4-73ad-46ca-0b79-08da23c12f17"),
                RecruimentId = 2,
                FilePath = "path",
                DateCreated = DateTime.Now
            });
            _context.CurriculumVitaes.Add(new CurriculumVitae
            {
                UserId = new Guid("5e9962d7-8b63-4e1a-943c-24c17d2f25de"),
                RecruimentId = 2,
                FilePath = "path",
                DateCreated = DateTime.Now
            });

            _context.SaveChanges();
            if (_storageService == null)
            {
                var mockStorage = new Mock<IStorageService>();
                _storageService = mockStorage.Object;
            }
            if (_userService == null)
            {
                var mockUserService = new Mock<IUserService>();
                _userService = mockUserService.Object;
            }
            var listAppUser = new List<AppUser>();
            if (_userManager == null)
            {
                _userManager = MockUserManager<AppUser>(listAppUser).Object;
            }
            _companyService = new CompanyService(_context, _storageService, _mapper, _userService, _userManager);
        }

        [Test]
        public async Task CanGetCompanyAvatar()
        {
            // Arrange
            var companyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17");
            // Act
            var result = await _companyService.GetCompanyAvatar(companyId);
            // Assert
            Assert.IsTrue(result.IsSuccessed);
            Assert.IsNotNull(result.ResultObj);
        }
        [Test]
        public async Task CanGetCompanyCoverImage()
        {
            // Arrange
            var companyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17");
            // Act
            var result = await _companyService.GetCompanyCoverImage(companyId);
            // Assert
            Assert.IsTrue(result.IsSuccessed);
            Assert.IsNotNull(result.ResultObj);
        }
        [Test]
        public async Task CanGetAllCompanyImages()
        {
            // Arrange
            var request = new GetCompanyImagesRequest()
            {
                PageIndex = 1,
                PageSize = 10,
                CompanyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17")
            };
            // Act
            var result = await _companyService.GetAllImages(request);
            // Assert
            Assert.IsTrue(result.IsSuccessed);
            Assert.IsNotNull(result.ResultObj);
        }
        [Test]
        public async Task CanGetCompanyBranches()
        {
            // Arrange
            var companyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17");

            // Act
            var result = await _companyService.GetCompanyBranch(companyId);
            // Assert
            Assert.IsTrue(result.IsSuccessed);
            Assert.IsNotNull(result.ResultObj);
        }
        [Test]
        public async Task CanGetListCompanyRecruitment()
        {
            // Arrange
            var companyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17");

            // Act
            var result = await _companyService.GetListCompanyRecruitment(companyId);
            // Assert
            Assert.IsTrue(result.IsSuccessed);
            Assert.IsNotNull(result.ResultObj);
        }
        [Test]
        public async Task CanGetRecruitmentById()
        {
            // Arrange
            var id = 2;

            // Act
            var result = await _companyService.GetRecruitmentById(id);
            // Assert
            Assert.IsTrue(result.IsSuccessed);
            Assert.IsNotNull(result.ResultObj);
        }

        [Test]
        public async Task CanGetCommentRecruitment()
        {
            // Arrange
            var id = 2;

            // Act
            var result = await _companyService.GetCommentRecruitment(id);
            // Assert
            Assert.IsTrue(result.IsSuccessed);
            Assert.IsNotNull(result.ResultObj);
        }
        [Test]
        public async Task CanGetBranchNotExist()
        {
            // Arrange
            var companyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17");

            // Act
            var result = await _companyService.GetBranchesNotExist(companyId);
            // Assert
            Assert.IsNotNull(result);
        }
        [Test]
        public async Task CanGetBranchRecruitmentNotExist()
        {
            // Arrange
            var id = 1;

            // Act
            var result = await _companyService.GetBranchesRecruitmentNotExist(id);
            // Assert
            Assert.IsNotNull(result);
        }
        [Test]
        public async Task CanGetBranchRecruitmentExist()
        {
            // Arrange
            var id = 1;

            // Act
            var result = await _companyService.GetBranchesRecruitmentExist(id);
            // Assert
            Assert.IsNotNull(result);
        }
        [Test]
        public async Task CanGetCareerRecruitmentNotExist()
        {
            // Arrange
            var id = 1;

            // Act
            var result = await _companyService.GetCareersRecruitmentNotExist(id);
            // Assert
            Assert.IsNotNull(result);
        }
        [Test]
        public async Task CanGetCareerRecruitmentExist()
        {
            // Arrange
            var id = 1;

            // Act
            var result = await _companyService.GetCareersRecruitmentExist(id);
            // Assert
            Assert.IsNotNull(result);
        }
        [Test]
        public async Task CanGetAllRecruitmentPaging()
        {
            // Arrange
            var request = new GetRecruitmentRequest()
            {
                PageIndex = 1,
                PageSize = 10
            };

            // Act
            var result = await _companyService.GetAllRecruitmentPaging(request);
            // Assert
            Assert.IsTrue(result.IsSuccessed);

            Assert.IsNotNull(result);
        }
        [Test]
        public async Task CanGetAllChat()
        {
            // Arrange
            var userId = new Guid("5e9962d7-8b63-4e1a-943c-24c17d2f25de");
            var companyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17");
            var role = "user";

            // Act
            var result = await _companyService.GetAllChat(userId, companyId, role);
            // Assert
            Assert.IsTrue(result.IsSuccessed);
            Assert.IsNotNull(result.ResultObj);
        }

        [Test]
        public async Task CanGetAllPersonChat()
        {
            // Arrange
            var companyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17");
            var role = "user";

            // Act
            var result = await _companyService.GetAllPersonChat(companyId, role);
            // Assert
            Assert.IsTrue(result.IsSuccessed);
            Assert.IsNotNull(result.ResultObj);
        }


        [Test]
        public async Task CanGetAllInformation()
        {
            // Arrange
            var companyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17");

            // Act
            var result = await _companyService.GetCompanyInformation(companyId);
            // Assert
            Assert.IsTrue(result.IsSuccessed);
            Assert.IsNotNull(result.ResultObj);
        }

        [Test]
        public async Task CanCreateCoverImage()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
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
            var request = new CreateCoverImageRequest()
            {
                CompanyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17"),
                ThumnailImage = file
            };

            // Act
            var result = await _companyService.CreateCoverImage(request);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanCreateCompanyImage()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var content = "Hello World from a Fake File";
            var fileName = "test.jpg";
            var name = "test.jpg";
            var contentDisposition = "form-data; name=\"image\"; filename=\"test.jpg\"";
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
            var request = new CreateCompanyImageRequest()
            {
                CompanyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17"),
                Image = file
            };

            // Act
            var result = await _companyService.CreateCompanyImages(request);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanUpdateAvatar()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
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
            var id = 2;
            var request = new AvatarUpdateRequest()
            {
                ThumnailImage = file
            };

            // Act
            var result = await _companyService.UpdateAvatar(id, request);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanUpdateCoverImage()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
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
            var id = 3;
            var request = new UpdateCoverImageRequest()
            {
                ThumnailImage = file
            };

            // Act
            var result = await _companyService.UpdateCoverImage(id, request);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanDeleteImage()
        {
            // Arrange

            var id = 2;
            // Act
            var result = await _companyService.DeleteImages(id);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }


        [Test]
        public async Task CanRemoveBranch()
        {
            // Arrange
            var companyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17");
            int branchId = 3;


            // Act
            var result = await _companyService.RemoveBranch(branchId, companyId);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanAddBranch()
        {
            // Arrange

            var request = new AddBranchViewModel()
            {
                CompanyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17"),
                BranchId = 2,
                Address = "Address Detail"
            };
            // Act
            var result = await _companyService.AddBranch(request);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanUpdateCompanyName()
        {
            // Arrange
            var companyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17");
            var name = "new name";

            // Act
            var result = await _companyService.UpdateCompanyName(companyId, name);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }

        [Test]
        public async Task CanUpdateCompanyDescription()
        {
            // Arrange
            var companyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17");
            var description = "new des";

            // Act
            var result = await _companyService.UpdateCompanyDescription(companyId, description);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanUpdateCompanyContactName()
        {
            // Arrange
            var companyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17");
            var contactName = "new contact name";

            // Act
            var result = await _companyService.UpdateCompanyContactName(companyId, contactName);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanUpdateCompanyWorkerNumber()
        {
            // Arrange
            var companyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17");
            var workerNumber = 1121;

            // Act
            var result = await _companyService.UpdateCompanyWorkerNumber(companyId, workerNumber);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanCreateRecruitment()
        {
            // Arrange
            var request = new RecruitmentCreateRequest()
            {
                Name = "Recruitment name 1",
                Rank = "Nhân viên",
                Experience = "2 - 5 năm",
                DetailedExperience = "detail exporience 1",
                Benefits = "benefits 1",
                Salary = 100001,
                Education = "Cao đẳng / Đại học",
                Type = "Toàn thời gian",
                Description = "description 1",
                ExpirationDate = new DateTime(2022, 05, 12),
                CompanyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17")

            };


            // Act
            var result = await _companyService.CreateNewRecruitment(request);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanUpdateRecruitmentName()
        {
            // Arrange
            var request = new UpdateRecruitmentNameRequest()
            {
                Name = "Recruitment name update",

                Id = 2
            };
            // Act
            var result = await _companyService.UpdateRecruitmentName(request);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanUpdateRecruitmentSalary()
        {
            // Arrange
            var request = new UpdateSalaryRecruitment()
            {
                Salary = 44444,
                Id = 2
            };
            // Act
            var result = await _companyService.UpdateRecruitmentSalary(request);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanAddBranchToRecruitment()
        {
            // Arrange
            var branchId = 2;
            var recruitmentId = 2;
            // Act
            var result = await _companyService.AddBranchToRecruitment(recruitmentId, branchId);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanAddCareerToRecruitment()
        {
            // Arrange
            var careerId = 2;
            var recruitmentId = 2;
            // Act
            var result = await _companyService.AddCareerToRecruitment(recruitmentId, careerId);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanRemoveCareerFromRecruitment()
        {
            // Arrange
            var careerId = 3;
            var recruitmentId = 2;
            // Act
            var result = await _companyService.RemoveCareerFromRecruitment(recruitmentId, careerId);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanRemoveBranchFromRecruitment()
        {
            // Arrange
            var branchId = 3;
            var recruitmentId = 2;
            // Act
            var result = await _companyService.RemoveBranchFromRecruitment(recruitmentId, branchId);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanExtendRecruitment()
        {
            // Arrange
            var request = new ExtendRecruitmentRequest()
            {
                Id = 2,
                NewExpirationDate = new DateTime(2022, 05, 15)
            };
            // Act
            var result = await _companyService.ExtendRecruitment(request);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }

        [Test]
        public async Task CanComment()
        {
            // Arrange
            var request = new CommentRequest()
            {
                AccountId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17"),
                RecruitmentId = 2,
                SubCommentId = null,
                Content = "saccs",
                Role = "company"
            };
            // Act
            var result = await _companyService.Comment(request);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        //[Test]
        //public async Task CanAcceptCV()
        //{
        //    // Arrange
        //    int recruitmentId = 2;
        //    var userId = new Guid("e8eed1d4-73ad-46ca-0b79-08da23c12f17");
        //    // Act
        //    var result = await _companyService.AcceptCV(recruitmentId, userId);
        //    // Assert
        //    Assert.IsTrue(result.ResultObj);
        //}
        [Test]
        public async Task CanRefuseCV()
        {
            // Arrange
            int recruitmentId = 2;
            var userId = new Guid("5e9962d7-8b63-4e1a-943c-24c17d2f25de");
            // Act
            var result = await _companyService.RefuseCV(recruitmentId, userId);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanChat()
        {
            // Arrange
            var request = new ChatRequest()
            {
                UserId = new Guid("5e9962d7-8b63-4e1a-943c-24c17d2f25de"),
                CompanyId = new Guid("9ffb4c7f-c67d-422a-0b7d-08da23c12f17"),
                Content = "content",
                Performer = "company"
            };
            // Act
            var result = await _companyService.Chat(request);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }

    }
}
