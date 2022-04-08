using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.Catalog.Admin;
using ViewModel.Catalog.Company;
using ViewModel.Common;


namespace Application.Catalog
{
    public interface ICompanyService
    {
        // đã dùng 
        Task<ApiResult<CompanyInformationViewModel>> GetCompanyInformation(Guid companyId);
        Task<ApiResult<CompanyAvatarViewModel>> GetCompanyAvatar(Guid companyId);
        Task<ApiResult<CompanyCoverImageViewModel>> GetCompanyCoverImage(Guid companyId);
        Task<ApiResult<List<CompanyImagesViewModel>>> GetAllImages(GetCompanyImagesRequest request);
        Task<ApiResult<List<CompanyBranchViewModel>>> GetCompanyBranch(Guid companyId);
        Task<List<BranchViewModel>> GetBranchesNotExist(Guid companyId);

        Task<ApiResult<bool>> CreateCoverImage(CreateCoverImageRequest request);
        Task<ApiResult<bool>> CreateCompanyImages(CreateCompanyImageRequest request);
        Task<ApiResult<bool>> UpdateAvatar(int Id, AvatarUpdateRequest request);
        Task<ApiResult<bool>> UpdateCoverImage(int id, UpdateCoverImageRequest request);
        Task<ApiResult<bool>> DeleteImages(int id);
        Task<ApiResult<bool>> AddBranch(AddBranchViewModel request);
        Task<ApiResult<bool>> RemoveBranch(int id, Guid companyId);
        Task<ApiResult<bool>> UpdateCompanyName(Guid id, string name);
        Task<ApiResult<bool>> UpdateCompanyDescription(Guid id, string description);
        Task<ApiResult<bool>> UpdateCompanyContactName(Guid id, string contactName);
        Task<ApiResult<bool>> UpdateCompanyWorkerNumber(Guid id, int workerNumber);


        // chưa chắc xài

        Task<ApiResult<bool>> DeleteCoverImage(int id);



        //chưa test
        Task<ApiResult<bool>> CreateNewRecruitment(RecruitmentCreateRequest request);
        Task<ApiResult<bool>> AddBranchToRecruitment(int recruimentId, int branchId);
        Task<ApiResult<bool>> AddCareerToRecruitment(int recruimentId, int careerId);
        Task<ApiResult<bool>> Comment(CommentRequest request);
        Task<ApiResult<List<CommentViewModel>>> GetCommentRecruitment(int id);
        Task<ApiResult<PageResult<RecruitmentPagingResult>>> GetAllRecruitmentPaging(GetRecruitmentRequest request);
        Task<ApiResult<RecruitmentViewModel>> GetRecruitmentById(int id);

    }
}
