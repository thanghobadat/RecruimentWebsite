using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using ViewModel.Common;

namespace Application.Catalog
{
    public interface IUserService
    {
        Task<ApiResult<bool>> UpdateAvatar(Guid id, IFormFile thumnailImage);
    }
}
