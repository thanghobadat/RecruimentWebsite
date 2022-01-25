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
        Task<ApiResult<CompanyAvatarViewModel>> GetCompanyAvatar(Guid companyId);
        Task<ApiResult<CompanyCoverImageViewModel>> GetCompanyCoverImage(Guid companyId);
        Task<ApiResult<CompanyInformationViewModel>> GetCompanyInformation(Guid companyId);
        Task<ApiResult<bool>> CreateBranch(CreateBranchRequest request);
        Task<ApiResult<bool>> CreateCoverImage(CreateCoverImageRequest request);
        Task<ApiResult<bool>> CreateCompanyImages(CreateCompanyImageRequest request);
        Task<ApiResult<bool>> UpdateBranch(UpdateBranchRequest request);
        Task<ApiResult<bool>> UpdateAvatar(int id, IFormFile thumnailImage);
        Task<ApiResult<bool>> UpdateCoverImage(int id, IFormFile thumnailImage);
        Task<ApiResult<bool>> DeleteBranch(int id);
        Task<ApiResult<bool>> DeleteCoverImage(int id);
        Task<ApiResult<bool>> DeleteImages(List<int> listId);

    }
}
