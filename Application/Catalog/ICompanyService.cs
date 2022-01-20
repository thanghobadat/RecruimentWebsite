using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.Catalog.Company;
using ViewModel.Common;

namespace Application.Catalog
{
    public interface ICompanyService
    {
        Task<ApiResult<List<CompanyBranchViewModel>>> GetAllBranchPaging(GetCompanyBranchRequest request);
        Task<ApiResult<PageResult<CompanyImagesViewModel>>> GetAllImagesPaging(GetCompanyImagesRequest request);

        Task<ApiResult<bool>> CreateBranch(CreateBranchRequest request);
        Task<ApiResult<bool>> CreateCompanyImages(CreateCompanyImageRequest request);
        Task<ApiResult<bool>> UpdateBranch(UpdateBranchRequest request);

        Task<ApiResult<bool>> UpdateAvatar(Guid id, IFormFile thumnailImage);
        Task<ApiResult<bool>> DeleteBranch(int id);

    }
}
