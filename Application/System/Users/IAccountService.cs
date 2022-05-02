using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.Common;
using ViewModel.System.Users;

namespace Application.System.Users
{
    public interface IAccountService
    {
        //ok
        Task<ApiResult<LoginViewModel>> Authenticate(LoginRequest request);
        Task<ApiResult<List<AccountViewModel>>> GetAllAccount();
        Task<ApiResult<bool>> ChangePassword(ChangePasswordRequest request);
        Task<ApiResult<bool>> RegisterUserAccount(RegisterUserAccountRequest request);
        Task<ApiResult<bool>> RegisterCompanyAccount(RegisterCompanyAccountRequest request);
        Task<ApiResult<bool>> RegisterAdminAccount(RegisterAdminAccountRequest request);

        // chưa ok
        Task<ApiResult<bool>> Delete(Guid id);

    }
}
