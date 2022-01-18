using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using ViewModel.Catalog.Company;
using ViewModel.Common;

namespace Application.Catalog
{
    public interface ICompanyService
    {
        Task<ApiResult<bool>> CreateBranch(CreateBranchRequest request);
        Task<ApiResult<PageResult<CompanyBranchViewModel>>> GetAllBranchPaging(GetCompanyBranchRequest request);

        Task<ApiResult<bool>> UpdateAvatar(Guid id, IFormFile thumnailImage);
    }
}
