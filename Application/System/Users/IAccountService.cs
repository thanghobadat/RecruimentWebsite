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
        Task<ApiResult<PageResult<UserViewModel>>> GetUserAccountPaging(GetUserPagingRequest request);
        Task<ApiResult<PageResult<CompanyViewModel>>> GetCompanyAccountPaging(GetUserPagingRequest request);
        Task<ApiResult<UserViewModel>> GetUserById(Guid id);
        Task<ApiResult<CompanyViewModel>> GetCompanyById(Guid id);
        Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest request);
        Task<ApiResult<bool>> UpdateCompany(Guid id, CompanyUpdateRequest request);
        Task<ApiResult<bool>> Delete(Guid id);
        Task<ApiResult<bool>> ChangePassword(Guid id, string newPassword);

    }
}
