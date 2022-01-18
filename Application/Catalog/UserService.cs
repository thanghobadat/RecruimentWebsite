using Application.Common;
using Data.EF;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ViewModel.Common;

namespace Application.Catalog
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RecruimentWebsiteDbContext _context;
        private readonly IStorageService _storageService;
        public UserService(UserManager<AppUser> userManager,
            RecruimentWebsiteDbContext context, IStorageService storageService)
        {
            _userManager = userManager;
            _context = context;
            _storageService = storageService;
        }

        public async Task<ApiResult<bool>> UpdateAvatar(Guid id, IFormFile thumnailImage)
        {
            var userAva = await _context.UserAvatars.FirstOrDefaultAsync(x => x.UserId == id);
            if (userAva == null)
            {
                return new ApiErrorResult<bool>("User avatar information could not be found");
            }

            if (userAva.ImagePath != "default-avatar")
            {
                await _storageService.DeleteFileAsync(userAva.ImagePath);
            }

            userAva.FizeSize = thumnailImage.Length;
            userAva.Caption = thumnailImage.FileName;
            userAva.ImagePath = await this.SaveFile(thumnailImage);
            userAva.DateCreated = DateTime.Now;
            var result = await _context.SaveChangesAsync();



            if (result == 0)
            {
                return new ApiErrorResult<bool>("An error occured, register unsuccessful");
            }
            return new ApiSuccessResult<bool>(true);

        }

        //Save File
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

    }
}
