using AutoMapper;
using Data.EF;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Catalog.Admin;
using ViewModel.Common;

namespace Application.Catalog
{
    public class AdminService : IAdminService
    {

        private readonly RecruimentWebsiteDbContext _context;
        private readonly IMapper _mapper;
        public AdminService(RecruimentWebsiteDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResult<bool>> CreateBranch(BranchViewModel request)
        {
            var branch = await _context.Branches.FirstOrDefaultAsync(x => x.City.Contains(request.City));

            if (branch != null)
            {
                return new ApiErrorResult<bool>("Tỉnh thành đã tồn tại, vui lòng nhập lại");
            }

            branch = new Branch()
            {
                City = request.City,
            };
            await _context.Branches.AddAsync(branch);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("Tỉnh thành đã tồn tại, vui lòng nhập lại");
            }

            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> CreateCareer(CareerCreateRequest request)
        {
            var career = await _context.Careers.FirstOrDefaultAsync(x => x.Name.Contains(request.Name));

            if (career != null)
            {
                return new ApiErrorResult<bool>("Công việc đã tồn tại, vui lòng nhập lại");
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
                return new ApiErrorResult<bool>("Công việc đã tồn tại, vui lòng nhập lại");
            }

            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> DeleteBranch(int id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch == null)
            {
                return new ApiErrorResult<bool>("Tỉnh thành này không tồn tại, vui lòng thử lại");
            }

            _context.Branches.Remove(branch);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("Xóa không thành công, vui lòng thử lại");
            }
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> DeleteCareer(int id)
        {
            var career = await _context.Careers.FindAsync(id);
            if (career == null)
            {
                return new ApiErrorResult<bool>("Công việc này không tồn tại, vui lòng thử lại");
            }

            _context.Careers.Remove(career);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("Xóa không thành công, vui lòng thử lại");
            }
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<List<BranchViewModel>> GetAllBranchPaging()
        {
            var query = await _context.Branches.OrderBy(x => x.City).ToListAsync();
            var data = query.Select(branch => _mapper.Map<BranchViewModel>(branch)).OrderBy(x => x.City).ToList();
            return data;
        }

        public async Task<List<CareerViewModel>> GetAllCareerPaging()
        {
            var query = await _context.Careers.OrderBy(x => x.Name).ToListAsync();

            var data = query.Select(x => new CareerViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                DateCreated = x.DateCreated
            }).ToList();



            return data;
        }

        public async Task<ApiResult<BranchViewModel>> GetBranchById(int id)
        {
            var branch = await _context.Branches.FindAsync(id);

            if (branch == null)
            {
                return new ApiErrorResult<BranchViewModel>("Tỉnh thành này không tồn tại, vui lòng thử lại");
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
                return new ApiErrorResult<CareerViewModel>("Công việc này không tồn tại, vui lòng thử lại");
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
                return new ApiErrorResult<bool>("Tỉnh thành này đã tồn tại, vui lòng nhập lại");
            }

            var branch = await _context.Branches.FindAsync(request.Id);

            branch.City = request.City;

            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("Tỉnh thành này đã tồn tại, vui lòng nhập lại");
            }

            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateCareer(CareerUpdateRequest request)
        {
            if (await _context.Careers.AnyAsync(x => x.Name == request.Name && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Công việc này đã tồn tại, vui lòng nhập lại");
            }

            var career = await _context.Careers.FindAsync(request.Id);

            career.Name = request.Name;

            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("Công việc này đã tồn tại, vui lòng nhập tên khác");
            }

            return new ApiSuccessResult<bool>(true);
        }
    }
}
