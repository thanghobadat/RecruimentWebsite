using Application.Common;
using AutoMapper;
using Data.EF;
using Data.Entities;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Collections.Generic;
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
                return new ApiErrorResult<UserInformationViewModel>("Không tìm thấy thông tin người dùng, vui lòng");
            }

            var userInforVM = _mapper.Map<UserInformationViewModel>(userInfor);
            var user = await _context.Users.FindAsync(userId);
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
                return new ApiErrorResult<UserAvatarViewModel>("Có lỗi xảy ra, vui lòng kiểm tra lại");
            }

            var avatarVM = _mapper.Map<UserAvatarViewModel>(avatar);

            return new ApiSuccessResult<UserAvatarViewModel>(avatarVM);
        }

        public async Task<ApiResult<bool>> UpdateUserAvatar(int id, IFormFile thumnailImage)
        {
            var userAva = await _context.UserAvatars.FindAsync(id);
            if (userAva == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy avatar");
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
                    return new ApiErrorResult<bool>("Đã có lỗi xảy ra, vui lòng kiểm tra lại");
                }
                return new ApiSuccessResult<bool>(true);
            }

            return new ApiErrorResult<bool>("Vui lòng chọn hình ảnh có đuôi jpg hoặc png");
        }

        //Save File
        private async Task<string> SaveAvatar(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveAvatarAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        private async Task<string> SaveCV(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveCVAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public async Task<ApiResult<bool>> UpdateUserInformation(UserUpdateRequest request)
        {

            var user = await _context.Users.FindAsync(request.UserId);
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
                return new ApiErrorResult<bool>("Đã có lỗi xảy ra, vui lòng kiểm tra lại");
            }
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> FollowCompany(Guid userId, Guid companyId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return new ApiErrorResult<bool>("Người dùng không tồn tại");
            }
            var userinf = await _context.UserInformations.FindAsync(userId);
            var company = await _context.Users.FindAsync(companyId);
            if (company == null)
            {
                return new ApiErrorResult<bool>("Công ty không tồn tại");
            }

            var follow = new Follow()
            {
                UserId = userId,
                CompanyId = companyId
            };

            var noti = new Notification()
            {
                AccountId = companyId,
                Content = userinf.FirstName + " " + userinf.LastName + " vừa theo dõi công ty bạn",
                DateCreated = DateTime.Now
            };

            await _context.Follows.AddAsync(follow);
            await _context.Notifications.AddAsync(noti);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("Đã có lỗi xảy ra, vui lòng kiểm tra lại");
            }
            return new ApiSuccessResult<bool>(true);


        }

        public async Task<ApiResult<bool>> SubmitCV(SubmitCVRequest request)
        {
            var user = await _context.UserInformations.FindAsync(request.UserId);
            if (user == null)
            {
                return new ApiErrorResult<bool>("tài khoản không tồn tại, vui lòng thử lại");
            }
            var recruitment = await _context.Recruitments.FindAsync(request.RecruitmentId);
            if (DateTime.Now > recruitment.ExpirationDate)
            {
                return new ApiErrorResult<bool>("Đã hết hạn úng tuyển, vui lòng quay lại sau!");
            }
            var imageIndex = request.File.FileName.LastIndexOf(".");
            var imageType = request.File.FileName.Substring(imageIndex + 1);
            if (imageType == "pdf")
            {
                var CV = new CurriculumVitae()
                {
                    RecruimentId = request.RecruitmentId,
                    UserId = request.UserId,
                    FilePath = await this.SaveCV(request.File),
                    DateCreated = DateTime.Now
                };

                await _context.CurriculumVitaes.AddAsync(CV);
                var result = await _context.SaveChangesAsync();
                if (result == 0)
                {
                    return new ApiErrorResult<bool>("Có lỗi xảy ra, vui lòng thử lại");
                }

                var noti = new Notification()
                {
                    AccountId = recruitment.CompanyId,
                    Content = user.FirstName + " " + user.LastName + " vừa nộp CV vào bài tuyển dụng " + recruitment.Name,
                    DateCreated = DateTime.Now
                };

                await _context.Notifications.AddAsync(noti);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>(true);
            }


            return new ApiErrorResult<bool>("Vui lòng chuyển CV sang file pdf trước khi nộp");

        }

        public async Task<ApiResult<bool>> ChangePasswordUser(ChangePasswordUserRequest request)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    return new ApiErrorResult<bool>(item.Description);
                }
            }
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> ForgotPassword(ForgotPasswordRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName);
            if (user == null)
            {
                return new ApiErrorResult<bool>("Tài khoản đăng nhập hiện không tồn tại!");
            }
            if (request.Email != user.Email)
            {
                return new ApiErrorResult<bool>("Email không chính xác, vui lòng kiểm tra lại");

            }
            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, "NewPassword@123");
            string to = request.Email;
            string subject = "Mật khẩu của bạn đã được thay đổi";
            string body = "Mật khẩu mới của bạn là: NewPassword@123";

            var mailSetting = await _context.MailSettings.FirstOrDefaultAsync();

            var email = new MimeMessage();

            email.Sender = new MailboxAddress(mailSetting.DisplayName, mailSetting.Email);
            email.From.Add(new MailboxAddress(mailSetting.DisplayName, mailSetting.Email));
            email.To.Add(new MailboxAddress(to, to));
            email.Subject = subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                //kết nối máy chủ
                await smtp.ConnectAsync(mailSetting.Host, mailSetting.Port, SecureSocketOptions.StartTls);
                // xác thực
                await smtp.AuthenticateAsync(mailSetting.Email, mailSetting.Password);
                //gởi
                await smtp.SendAsync(email);
            }
            catch (Exception e)
            {
                return new ApiErrorResult<bool>("there was an error when sending mail but still created the idea. Error: " + e.Message);
            }
            smtp.Disconnect(true);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<List<AllUserResult>>> GetAllUser()
        {
            var results = new List<AllUserResult>();
            var users = await _userManager.GetUsersInRoleAsync("user");
            foreach (var user in users)
            {
                var infor = await this.GetUserInformation(user.Id);
                var result = new AllUserResult()
                {
                    Id = user.Id,
                    Name = infor.ResultObj.FirstName + " " + infor.ResultObj.LastName,
                    Address = infor.ResultObj.Address,
                    AcademicLevel = infor.ResultObj.AcademicLevel,
                    AvatarPath = infor.ResultObj.UserAvatar.ImagePath
                };
                results.Add(result);
            }
            return new ApiSuccessResult<List<AllUserResult>>(results);
        }


    }
}
