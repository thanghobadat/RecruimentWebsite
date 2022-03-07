using System.Threading.Tasks;
using ViewModel.Catalog.Admin;
using ViewModel.Common;

namespace Application.Catalog
{
    public interface IAdminService
    {
        Task<PageResult<BranchViewModel>> GetAllBranchPaging(GetBranchPagingRequest request);
        Task<PageResult<CareerViewModel>> GetAllCareerPaging(GetCareerPagingRequest request);
        Task<ApiResult<BranchViewModel>> GetBranchById(int id);
        Task<ApiResult<CareerViewModel>> GetCareerById(int id);
        Task<ApiResult<bool>> CreateBranch(BranchViewModel request);
        Task<ApiResult<bool>> CreateCareer(CareerViewModel request);
        Task<ApiResult<bool>> UpdateBranch(BranchViewModel request);
        Task<ApiResult<bool>> UpdateCareer(CareerViewModel request);
        Task<ApiResult<bool>> DeleteBranch(int id);
        Task<ApiResult<bool>> DeleteCareer(int id);


    }
}
