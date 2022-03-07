using System;
using System.Threading.Tasks;
using ViewModel.Catalog.Company;
using ViewModel.Common;


namespace Application.Catalog
{
    public interface ICompanyService
    {

        Task<ApiResult<PageResult<CompanyImagesViewModel>>> GetAllImagesPaging(GetCompanyImagesRequest request);
        Task<ApiResult<CompanyAvatarViewModel>> GetCompanyAvatar(Guid companyId);
        Task<ApiResult<CompanyCoverImageViewModel>> GetCompanyCoverImage(Guid companyId);
        Task<ApiResult<CompanyInformationViewModel>> GetCompanyInformation(Guid companyId);
        Task<ApiResult<bool>> CreateCoverImage(CreateCoverImageRequest request);
        Task<ApiResult<bool>> CreateCompanyImages(CreateCompanyImageRequest request);
        Task<ApiResult<bool>> UpdateCompanyInformation(CompanyUpdateRequest request);
        Task<ApiResult<bool>> UpdateAvatar(int Id, AvatarUpdateRequest request);
        Task<ApiResult<bool>> UpdateCoverImage(int id, UpdateCoverImageRequest request);
        Task<ApiResult<bool>> DeleteCoverImage(int id);
        Task<ApiResult<bool>> DeleteImages(int id);

        Task<ApiResult<bool>> AddBranch(AddBranchViewModel request);
        Task<ApiResult<bool>> RemoveBranch(int id, Guid companyId);


    }
}
