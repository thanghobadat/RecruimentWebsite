using Application.Common;
using AutoMapper;
using Data.EF;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Catalog.Admin;
using ViewModel.Common;

namespace Application.Catalog
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly RecruimentWebsiteDbContext _context;
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;
        public AdminService(RecruimentWebsiteDbContext context, IStorageService storageService,
            IMapper mapper, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            _storageService = storageService;
            _mapper = mapper;
        }

        public async Task<ApiResult<bool>> CreateBranch(BranchViewModel request)
        {
            var branch = await _context.Branches.FirstOrDefaultAsync(x => x.City.Contains(request.City));

            if (branch != null)
            {
                return new ApiErrorResult<bool>("branch is exist");
            }

            branch = new Branch()
            {
                City = request.City,
            };
            await _context.Branches.AddAsync(branch);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("Create unsuccessful");
            }

            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> CreateCareer(CareerViewModel request)
        {
            var career = await _context.Careers.FirstOrDefaultAsync(x => x.Name.Contains(request.Name));

            if (career != null)
            {
                return new ApiErrorResult<bool>("Career is exist");
            }

            career = new Career()
            {
                Name = request.Name,
                DateCreated = DateTime.Now
            };
            await _context.Careers.AddAsync(career);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("Create unsuccessful");
            }

            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> DeleteBranch(int id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch == null)
            {
                return new ApiErrorResult<bool>("Branch doesn't exist");
            }

            _context.Branches.Remove(branch);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("Delete unsuccessful");
            }
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> DeleteCareer(int id)
        {
            var career = await _context.Careers.FindAsync(id);
            if (career == null)
            {
                return new ApiErrorResult<bool>("Career doesn't exist");
            }

            _context.Careers.Remove(career);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("Delete unsuccessful");
            }
            return new ApiSuccessResult<bool>();
        }

        public async Task<PageResult<BranchViewModel>> GetAllBranchPaging(GetBranchPagingRequest request)
        {
            var query = await _context.Branches.ToListAsync();
            var Branches = query.AsQueryable();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                Branches = Branches.Where(x => x.City.Contains(request.Keyword));
            }


            int totalRow = Branches.Count();

            var data = Branches.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new BranchViewModel()
                {
                    Id = x.Id,
                    City = x.City,
                }).ToList();

            var pagedResult = new PageResult<BranchViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };

            return pagedResult;
        }

        public async Task<PageResult<CareerViewModel>> GetAllCareerPaging(GetCareerPagingRequest request)
        {
            var query = await _context.Careers.ToListAsync();
            var careers = query.AsQueryable();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                careers = careers.Where(x => x.Name.Contains(request.Keyword));
            }


            int totalRow = careers.Count();

            var data = careers.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new CareerViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    DateCreated = x.DateCreated
                }).ToList();

            var pagedResult = new PageResult<CareerViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };

            return pagedResult;
        }

        public async Task<ApiResult<BranchViewModel>> GetBranchById(int id)
        {
            var branch = await _context.Branches.FindAsync(id);

            if (branch == null)
            {
                return new ApiErrorResult<BranchViewModel>("Branch doesn't exits");
            }

            var branchViewModel = new BranchViewModel()
            {
                Id = id,
                City = branch.City,
            };
            return new ApiSuccessResult<BranchViewModel>(branchViewModel);
        }

        public async Task<ApiResult<CareerViewModel>> GetCareerById(int id)
        {
            var career = await _context.Careers.FindAsync(id);

            if (career == null)
            {
                return new ApiErrorResult<CareerViewModel>("Career doesn't exits");
            }

            var careerViewModel = new CareerViewModel()
            {
                Id = id,
                Name = career.Name,
            };
            return new ApiSuccessResult<CareerViewModel>(careerViewModel);
        }

        public async Task<ApiResult<bool>> UpdateBranch(BranchViewModel request)
        {
            if (await _context.Branches.AnyAsync(x => x.City == request.City && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Category is exist");
            }

            var branch = await _context.Branches.FindAsync(request.Id);

            branch.City = request.City;

            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("You entered duplicate data, please try again");
            }

            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> UpdateCareer(CareerViewModel request)
        {
            if (await _context.Careers.AnyAsync(x => x.Name == request.Name && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Career is exist");
            }

            var career = await _context.Careers.FindAsync(request.Id);

            career.Name = request.Name;

            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("You entered duplicate data, please try again");
            }

            return new ApiSuccessResult<bool>();
        }
    }
}
