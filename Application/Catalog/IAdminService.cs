using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.Catalog.Admin;
using ViewModel.Common;

namespace Application.Catalog
{
    public interface IAdminService
    {
        Task<List<BranchViewModel>> GetAllBranchPaging();
        Task<List<CareerViewModel>> GetAllCareerPaging();
        Task<ApiResult<BranchViewModel>> GetBranchById(int id);
        Task<ApiResult<CareerViewModel>> GetCareerById(int id);
        Task<ApiResult<bool>> CreateBranch(BranchViewModel request);
        Task<ApiResult<bool>> CreateCareer(CareerCreateRequest request);
        Task<ApiResult<bool>> UpdateBranch(BranchViewModel request);
        Task<ApiResult<bool>> UpdateCareer(CareerUpdateRequest request);
        Task<ApiResult<bool>> DeleteBranch(int id);
        Task<ApiResult<bool>> DeleteCareer(int id);


    }
}
