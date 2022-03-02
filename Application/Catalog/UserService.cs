using Application.Common;
using AutoMapper;
using Data.EF;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;
        private readonly RecruimentWebsiteDbContext _context;
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;
        public UserService(RecruimentWebsiteDbContext context, IStorageService storageService,
            IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _storageService = storageService;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ApiResult<UserInformationViewModel>> GetUserInformation(Guid userId)
        {
            var userInfor = await _context.UserInformations.FindAsync(userId);
            if (userInfor == null)
            {
                return new ApiErrorResult<UserInformationViewModel>("User information could not be found, please check again");
            }

            var userInforVM = _mapper.Map<UserInformationViewModel>(userInfor);
            var user = await _userManager.FindByIdAsync(userId.ToString());
            userInforVM.Email = user.Email;
            userInforVM.PhoneNumber = user.PhoneNumber;
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

            var avatarVM = _mapper.Map<UserAvatarViewModel>(avatar);

            return new ApiSuccessResult<UserAvatarViewModel>(avatarVM);
        }

        public async Task<ApiResult<bool>> UpdateUserAvatar(int id, IFormFile thumnailImage)
        {
            var userAva = await _context.UserAvatars.FindAsync(id);
            if (userAva == null)
            {
                return new ApiErrorResult<bool>("User avatar could not be found");
            }


            var imageIndex = thumnailImage.FileName.LastIndexOf(".");
            var imageType = thumnailImage.FileName.Substring(imageIndex + 1);
            if (imageType == "jpg" || imageType == "png")
            {
                if (userAva.ImagePath != "default-avatar.jpg")
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



        public async Task<ApiResult<bool>> UpdateUserInformation(UserUpdateRequest request)
        {

            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;


            var userInf = await _context.UserInformations.FindAsync(request.UserId);
            userInf.Age = request.Age;
            userInf.FirstName = request.FirstName;
            userInf.AcademicLevel = request.AcademicLevel;
            userInf.LastName = request.LastName;
            userInf.Sex = request.Sex;
            userInf.Address = request.Address;
            var resultUserInf = await _context.SaveChangesAsync();
            if (resultUserInf == 0)
            {
                return new ApiErrorResult<bool>("An error occured, register unsuccessful");
            }
            return new ApiSuccessResult<bool>(true);
        }
    }
}
