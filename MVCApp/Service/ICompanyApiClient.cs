using MVCApp.ViewModels;
using System;
using System.Threading.Tasks;
using ViewModel.Catalog.Company;
using ViewModel.Common;

namespace MVCApp.Service
{
    public interface ICompanyApiClient
    {
        Task<ApiResult<CompanyInformationViewModel>> GetCompanyInformation(Guid Id);
        Task<ApiResult<CompanyBranchViewModel>> GetCompanyBranchById(int Id);
        Task<ApiResult<bool>> UpdateCompanyInformation(Guid Id, CompanyInformationUpdateRequest request);
        Task<ApiResult<bool>> CreateBranch(CompanyBranchCreateRequest request);
        Task<ApiResult<bool>> UpdateBranch(CompanyBranchUpdateRequest request);
        Task<ApiResult<bool>> DeleteBranch(int id);
    }
}
