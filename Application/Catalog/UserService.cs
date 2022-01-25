using Application.Common;
using Data.EF;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ViewModel.Catalog.User;
using ViewModel.Common;

namespace Application.Catalog
{
    public class UserService : IUserService
    {
        private readonly RecruimentWebsiteDbContext _context;
        private readonly IStorageService _storageService;
        public UserService(RecruimentWebsiteDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<ApiResult<UserInformationViewModel>> GetUserInformation(Guid userId)
        {
            var userInfor = await _context.UserInformations.FindAsync(userId);
            if (userInfor == null)
            {
                return new ApiErrorResult<UserInformationViewModel>("User information could not be found, please check again");
            }

            var userInforVM = new UserInformationViewModel()
            {
                FirstName = userInfor.FirstName,
                LastName = userInfor.LastName,
                AcademicLevel = userInfor.AcademicLevel,
                Address = userInfor.Address,
                Sex = userInfor.Sex,
                Age = userInfor.Age,
                UserId = userInfor.UserId,

            };

            var userAvatar = await this.GetUserAvatar(userId);
            userInforVM.UserAvatar = userAvatar.ResultObj;
            return new ApiSuccessResult<UserInformationViewModel>(userInforVM);
        }

        public async Task<ApiResult<UserAvatarViewModel>> GetUserAvatar(Guid userId)
        {
            var avatar = await _context.UserAvatars.FirstOrDefaultAsync(x => x.UserId == userId);
            if (avatar == null)
            {
                return new ApiErrorResult<UserAvatarViewModel>("Something wrong, Please check user id");
            }

            var avatarVM = new UserAvatarViewModel()
            {
                UserId = userId,
                Caption = avatar.Caption,
                FileSize = avatar.FizeSize,
                ImagePath = avatar.ImagePath,
                DateCreated = avatar.DateCreated,
                Id = avatar.Id
            };

            return new ApiSuccessResult<UserAvatarViewModel>(avatarVM);
        }

        public async Task<ApiResult<bool>> UpdateUserAvatar(int id, IFormFile thumnailImage)
        {
            var userAva = await _context.UserAvatars.FindAsync(id);
            if (userAva == null)
            {
                return new ApiErrorResult<bool>("User avatar information could not be found");
            }


            var imageIndex = thumnailImage.FileName.LastIndexOf(".");
            var imageType = thumnailImage.FileName.Substring(imageIndex + 1);
            if (imageType == "jpg" || imageType == "png")
            {
                if (userAva.ImagePath != "default-avatar")
                {
                    await _storageService.DeleteAvatarAsync(userAva.ImagePath);
                }
                userAva.FizeSize = thumnailImage.Length;
                userAva.Caption = thumnailImage.FileName;
                userAva.ImagePath = await this.SaveAvatar(thumnailImage);
                userAva.DateCreated = DateTime.Now;
                var result = await _context.SaveChangesAsync();



                if (result == 0)
                {
                    return new ApiErrorResult<bool>("An error occured, register unsuccessful");
                }
                return new ApiSuccessResult<bool>(true);
            }

            return new ApiErrorResult<bool>("Please choose jpg or png image file");
        }

        //Save File
        private async Task<string> SaveAvatar(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveAvatarAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

    }
}
