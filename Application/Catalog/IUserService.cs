using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.Catalog.User;
using ViewModel.Common;

namespace Application.Catalog
{
    public interface IUserService
    {

        Task<ApiResult<UserInformationViewModel>> GetUserInformation(Guid userId);
        Task<ApiResult<UserAvatarViewModel>> GetUserAvatar(Guid userId);
        Task<ApiResult<List<AllUserResult>>> GetAllUser();
        Task<ApiResult<bool>> UpdateUserInformation(UserUpdateRequest request);
        Task<ApiResult<bool>> UpdateUserAvatar(int id, IFormFile thumnailImage);
        Task<ApiResult<bool>> FollowCompany(Guid userId, Guid companyId);
        Task<ApiResult<bool>> ChangePasswordUser(ChangePasswordUserRequest request);
        Task<ApiResult<bool>> ForgotPassword(ForgotPasswordRequest request);
        Task<ApiResult<bool>> SubmitCV(SubmitCVRequest request);


    }
}
