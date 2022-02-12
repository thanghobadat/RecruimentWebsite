using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using ViewModel.Catalog.User;
using ViewModel.Common;

namespace Application.Catalog
{
    public interface IUserService
    {

        Task<ApiResult<UserInformationViewModel>> GetUserInformation(Guid userId);
        Task<ApiResult<UserAvatarViewModel>> GetUserAvatar(Guid userId);
        Task<ApiResult<bool>> UpdateUserInformation(Guid id, UserUpdateRequest request);

        Task<ApiResult<bool>> UpdateUserAvatar(int id, IFormFile thumnailImage);

    }
}
