using MVCApp.ViewModels;
using System;
using System.Threading.Tasks;
using ViewModel.Catalog.User;
using ViewModel.Common;

namespace MVCApp.Service
{
    public interface IUserApiClient
    {
        Task<ApiResult<UserInformationViewModel>> GetUserInformation(Guid Id);
        Task<ApiResult<bool>> UpdateUserInformation(Guid Id, UserInformationUpdateRequest request);
    }
}
