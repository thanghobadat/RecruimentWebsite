using MVCApp.ViewModels;
using System;
using System.Threading.Tasks;
using ViewModel.Common;
using ViewModel.System.Users;

namespace MVCApp.Service
{
    public interface IAccountApiClient
    {
        Task<string> Authenticate(LoginRequest request);
        Task<ApiResult<PageResult<CompanyAccountViewModel>>> GetCompanyAccountPagings(GetAccountPagingRequest request);
        Task<ApiResult<PageResult<UserAccountViewModel>>> GetUserAccountPagings(GetAccountPagingRequest request);
        Task<ApiResult<bool>> ChangePassword(ChangePasswordRequest request);
        Task<ApiResult<bool>> DeleteAccount(Guid Id);
    }
}
