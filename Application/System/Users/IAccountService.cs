using System;
using System.Threading.Tasks;
using ViewModel.Common;
using ViewModel.System.Users;

namespace Application.System.Users
{
    public interface IAccountService
    {
        Task<string> Authenticate(LoginRequest request);
        Task<ApiResult<bool>> RegisterUserAccount(RegisterUserAccountRequest request);
        Task<ApiResult<bool>> RegisterCompanyAccount(RegisterCompanyAccountRequest request);
        Task<ApiResult<bool>> RegisterAdminAccount(RegisterAdminAccountRequest request);
        Task<ApiResult<PageResult<UserAccountViewModel>>> GetUserAccountPaging(GetAccountPagingRequest request);
        Task<ApiResult<PageResult<CompanyAccountViewModel>>> GetCompanyAccountPaging(GetAccountPagingRequest request);
        Task<ApiResult<UserAccountViewModel>> GetUserById(Guid id);
        Task<ApiResult<CompanyAccountViewModel>> GetCompanyById(Guid id);
        Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest request);
        Task<ApiResult<bool>> UpdateCompany(Guid id, CompanyUpdateRequest request);
        Task<ApiResult<bool>> Delete(Guid id);
        Task<ApiResult<bool>> ChangePassword(Guid id, string newPassword);

    }
}
