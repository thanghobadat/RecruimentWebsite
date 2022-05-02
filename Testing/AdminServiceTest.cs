using Application.AutoMapper;
using Application.Catalog;
using AutoMapper;
using Data.EF;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using ViewModel.Catalog.Admin;

namespace Testing
{
    [TestFixture]
    internal class AdminServiceTest
    {
        private IAdminService _adminService;
        private RecruimentWebsiteDbContext _context;
        private static IMapper _mapper;


        public AdminServiceTest()
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
        [OneTimeSetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<RecruimentWebsiteDbContext>()
                .UseInMemoryDatabase(databaseName: "DatabaseName")
                .Options;
            _context = new RecruimentWebsiteDbContext(options);

            _context.Careers.Add(new Career
            {
                Id = 1,
                Name = "Career 1",
                DateCreated = new DateTime(2022, 04, 09, 10, 12, 20)
            });
            _context.Careers.Add(new Career
            {
                Id = 2,
                Name = "Career 2",
                DateCreated = new DateTime(2022, 04, 10, 11, 20, 00)
            });
            _context.Careers.Add(new Career
            {
                Id = 3,
                Name = "Career 3",
                DateCreated = new DateTime(2022, 04, 11, 12, 22, 10)
            });
            _context.Branches.Add(new Branch
            {
                Id = 1,
                City = "Branch 1"
            });
            _context.Branches.Add(new Branch
            {
                Id = 2,
                City = "Branch 2"
            });
            _context.Branches.Add(new Branch
            {
                Id = 3,
                City = "Branch 3"
            });




            _context.SaveChanges();

            _adminService = new AdminService(_context, _mapper);
        }
        [Test]
        public async Task CanGetAllBranch()
        {
            // Arrange

            // Act
            var result = await _adminService.GetAllBranchPaging();
            // Assert
            Assert.IsNotNull(result);
        }
        [Test]
        public async Task CanGetAllBranchCareer()
        {
            // Arrange

            // Act
            var result = await _adminService.GetAllCareerPaging();
            // Assert
            Assert.IsNotNull(result);
        }
        [Test]
        public async Task CanGetBranchById()
        {
            // Arrange
            int id = 2;
            // Act
            var result = await _adminService.GetBranchById(id);
            // Assert
            Assert.IsTrue(result.IsSuccessed);
            Assert.IsNotNull(result.ResultObj);
        }
        [Test]
        public async Task CanGetCareerById()
        {
            // Arrange
            int id = 2;
            // Act
            var result = await _adminService.GetCareerById(id);
            // Assert
            Assert.IsTrue(result.IsSuccessed);
            Assert.IsNotNull(result.ResultObj);
        }
        [Test]
        public async Task CanCreateBranch()
        {
            // Arrange
            var request = new BranchViewModel()
            {
                City = "Branch 4",
            };
            // Act
            var result = await _adminService.CreateBranch(request);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanCreateCareer()
        {
            // Arrange
            var request = new CareerCreateRequest()
            {
                Name = "Career 4",
            };
            // Act
            var result = await _adminService.CreateCareer(request);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanUpdateBranch()
        {
            // Arrange
            var request = new BranchViewModel()
            {
                Id = 4,
                City = "Branch Updated",
            };
            // Act
            var result = await _adminService.UpdateBranch(request);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanUpdateCareer()
        {
            // Arrange
            var request = new CareerUpdateRequest()
            {
                Id = 4,
                Name = "Career Updated",
            };
            // Act
            var result = await _adminService.UpdateCareer(request);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanDeleteBranch()
        {
            // Arrange
            int id = 1;
            // Act
            var result = await _adminService.DeleteBranch(id);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        [Test]
        public async Task CanDeleteCareer()
        {
            // Arrange
            int id = 1;
            // Act
            var result = await _adminService.DeleteCareer(id);
            // Assert
            Assert.IsTrue(result.ResultObj);
        }
        
    }
}
