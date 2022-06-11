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
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ViewModel.Catalog.Admin;
using ViewModel.Catalog.Company;
using ViewModel.Common;

namespace Application.Catalog
{
    public class CompanyService : ICompanyService
    {

        private readonly RecruimentWebsiteDbContext _context;
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        public CompanyService(RecruimentWebsiteDbContext context, IStorageService storageService,
            IMapper mapper, IUserService userService, UserManager<AppUser> userManager)
        {
            _context = context;
            _storageService = storageService;
            _mapper = mapper;
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<ApiResult<bool>> AddBranch(AddBranchViewModel request)
        {
            var companyBranch = new CompanyBranch()
            {
                BranchId = request.BranchId,
                CompanyId = request.CompanyId,
                Address = request.Address
            };

            await _context.CompanyBranches.AddAsync(companyBranch);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("An error occured, add branch unsuccessful");
            }
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> AddBranchToRecruitment(int recruimentId, int branchId)
        {
            var recruitment = await _context.Recruitments.FindAsync(recruimentId);
            if (recruitment == null)
            {
                return new ApiErrorResult<bool>("Recruitment doesn't exist, please re-enter");
            }

            var branch = await _context.Branches.FindAsync(branchId);
            if (branch == null)
            {
                return new ApiErrorResult<bool>("Branch doesn't exist, please re-enter");
            }

            var branchRecruitment = new BranchRecruitment()
            {
                RecruimentId = recruimentId,
                BranchId = branchId
            };

            await _context.BranchRecruitments.AddAsync(branchRecruitment);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("An error occured, register unsuccessful");
            }
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> AddCareerToRecruitment(int recruimentId, int careerId)
        {
            var recruitment = await _context.Recruitments.FindAsync(recruimentId);
            if (recruitment == null)
            {
                return new ApiErrorResult<bool>("Recruitment doesn't exist, please re-enter");
            }

            var career = await _context.Careers.FindAsync(careerId);
            if (career == null)
            {
                return new ApiErrorResult<bool>("Career doesn't exist, please re-enter");
            }

            var careerRecruitment = new CareerRecruitment()
            {
                RecruimentId = recruimentId,
                CareerId = careerId
            };

            await _context.CareerRecruitments.AddAsync(careerRecruitment);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("An error occured, register unsuccessful");
            }
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> Comment(CommentRequest request)
        {
            if (request.Role == "user")
            {
                var user = await _context.UserInformations.FindAsync(request.AccountId);
                if (user == null)
                {
                    return new ApiErrorResult<bool>("User doesn't exist, please re-enter");
                }
            }
            else if (request.Role == "company")
            {
                var company = await _context.CompanyInformations.FindAsync(request.AccountId);
                if (company == null)
                {
                    return new ApiErrorResult<bool>("Company doesn't exist, please re-enter");
                }
            }
            else
            {
                return new ApiErrorResult<bool>("Only user and company accounts can comment");
            }

            var recruitment = await _context.Recruitments.FindAsync(request.RecruitmentId);
            if (recruitment == null)
            {
                return new ApiErrorResult<bool>("Recruitment doesn't exist, please re-enter");
            }

            var comment = new Comment()
            {
                AccountId = request.AccountId,
                RecruimentId = request.RecruitmentId,
                Content = request.Content,
                DateCreated = DateTime.Now,
                SubcommentId = request.SubCommentId
            };
            await _context.Comments.AddAsync(comment);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("An error occured, comment unsuccessful");
            }
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> CreateCompanyImages(CreateCompanyImageRequest request)
        {
            var company = await _context.CompanyInformations.FindAsync(request.CompanyId);
            if (company == null)
            {
                return new ApiErrorResult<bool>("Company doesn't exist, please re-enter");
            }


            var imageIndex = request.Image.FileName.LastIndexOf(".");
            var imageType = request.Image.FileName.Substring(imageIndex + 1);
            if (imageType == "jpg" || imageType == "png")
            {
                var companyImage = new CompanyImage()
                {
                    FizeSize = request.Image.Length,
                    Caption = request.Image.FileName,
                    DateCreated = DateTime.Now,
                    CompanyId = request.CompanyId,
                    ImagePath = await this.SaveImages(request.Image)
                };

                await _context.CompanyImages.AddAsync(companyImage);
            }
            else
            {
                return new ApiErrorResult<bool>("Please choose jpg or png image file");
            }


            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("An error occured, create unsuccessful");
            }
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> CreateCoverImage(CreateCoverImageRequest request)
        {
            var company = await _context.CompanyInformations.FindAsync(request.CompanyId);
            if (company == null)
            {
                return new ApiErrorResult<bool>("Company doesn't exist, please re-enter");
            }

            var coverImage = new CompanyCoverImage()
            {
                FizeSize = request.ThumnailImage.Length,
                Caption = request.ThumnailImage.FileName,
                DateCreated = DateTime.Now,
                CompanyId = request.CompanyId,
                ImagePath = await this.SaveCoverImage(request.ThumnailImage)
            };

            await _context.CompanyCoverImages.AddAsync(coverImage);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("An error occured, register unsuccessful");
            }
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> CreateNewRecruitment(RecruitmentCreateRequest request)
        {
            var company = await _context.CompanyInformations.FindAsync(request.CompanyId);
            if (company == null)
            {
                return new ApiErrorResult<bool>("Công ty không tồn tại, vui lòng kiểm tra lại");
            }

            var recruitment = new Recruitment()
            {
                Name = request.Name,
                Rank = request.Rank,
                Education = request.Education,
                Experience = request.Experience,
                Description = request.Description,
                DetailedExperience = request.DetailedExperience,
                Salary = request.Salary,
                Type = request.Type,
                ExpirationDate = request.ExpirationDate,
                Benefits = request.Benefits,
                DateCreated = DateTime.Now,
                CompanyId = request.CompanyId,
            };

            await _context.Recruitments.AddAsync(recruitment);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("Có vấn đề xảy ra, vui lòng kiểm tra lại");
            }

            var usersFollow = await _context.Follows.Where(x => x.CompanyId == request.CompanyId).ToListAsync();

            foreach (var user in usersFollow)
            {
                var noti = new Notification()
                {
                    AccountId = user.UserId,
                    Content = company.Name + " vừa đăng một bài tuyển dụng mới mà có thể bạn quan tâm.",
                    DateCreated = DateTime.Now
                };
                await _context.Notifications.AddAsync(noti);
                await _context.SaveChangesAsync();
            }
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> DeleteCoverImage(int id)
        {
            var coverImage = await _context.CompanyCoverImages.FindAsync(id);
            if (coverImage == null)
            {
                return new ApiErrorResult<bool>("Cover Image doesn't exist");
            }

            await _storageService.DeleteCoverImageAsync(coverImage.ImagePath);
            _context.CompanyCoverImages.Remove(coverImage);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("An error occured, delete unsuccessful");
            }
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> DeleteImages(int id)
        {

            var image = await _context.CompanyImages.FindAsync(id);
            if (image == null)
            {
                return new ApiErrorResult<bool>("Image doesn't exist");
            }
            await _storageService.DeleteImagesAsync(image.ImagePath);
            _context.CompanyImages.Remove(image);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("An error occured, delete unsuccessful");
            }
            return new ApiSuccessResult<bool>(true);
        }



        public async Task<ApiResult<List<CompanyImagesViewModel>>> GetAllImages(GetCompanyImagesRequest request)
        {
            var company = await _context.CompanyInformations.FindAsync(request.CompanyId);
            if (company == null)
            {
                return new ApiErrorResult<List<CompanyImagesViewModel>>("Company doesn't exist, please re-enter");
            }

            var companyImages = await _context.CompanyImages.Where(x => x.CompanyId == request.CompanyId).ToListAsync();


            var data = companyImages.Select(image => _mapper.Map<CompanyImagesViewModel>(image)).ToList();


            return new ApiSuccessResult<List<CompanyImagesViewModel>>(data);
        }

        public async Task<ApiResult<PageResult<RecruitmentPagingResult>>> GetAllRecruitmentPaging(GetRecruitmentRequest request)
        {
            var query = await _context.Recruitments.OrderByDescending(x => x.DateCreated).ToListAsync();

            if (request.CareerId != 0)
            {
                query.Clear();
                var careerrecruitments = await _context.CareerRecruitments.Where(x => x.CareerId == request.CareerId).ToListAsync();
                foreach (var item in careerrecruitments)
                {
                    var recruitment = await _context.Recruitments.FindAsync(item.RecruimentId);
                    query.Add(recruitment);
                }
            }
            if (request.BranchId != 0)
            {
                var query1 = query;
                var query2 = new List<Recruitment>();
                query2.Clear();
                var branchrecruitments = await _context.BranchRecruitments.Where(x => x.BranchId == request.BranchId).ToListAsync();
                foreach (var item in branchrecruitments)
                {
                    var recruitment = await _context.Recruitments.FindAsync(item.RecruimentId);
                    query2.Add(recruitment);
                }
                query = query1.Intersect(query2).ToList();
            }

            var recruiments = query.AsQueryable();
            if (!string.IsNullOrEmpty(request.Rank))
            {
                recruiments = recruiments.Where(x => x.Rank.ToUpper() == request.Rank.ToUpper());
            }
            if (!string.IsNullOrEmpty(request.Experience))
            {
                recruiments = recruiments.Where(x => x.Experience.ToUpper() == request.Experience.ToUpper());
            }
            if (request.Salary != 0)
            {
                if (request.Salary == 1000000)
                {
                    recruiments = recruiments.Where(x => x.Salary >= 0 && x.Salary <= request.Salary);
                }
                else if (request.Salary == 3000000)
                {
                    recruiments = recruiments.Where(x => x.Salary >= 1000000 && x.Salary <= request.Salary);
                }
                else if (request.Salary == 8000000)
                {
                    recruiments = recruiments.Where(x => x.Salary >= 3000000 && x.Salary <= request.Salary);
                }
                else if (request.Salary == 20000000)
                {
                    recruiments = recruiments.Where(x => x.Salary >= 8000000 && x.Salary <= request.Salary);
                }
                else if (request.Salary == 50000000)
                {
                    recruiments = recruiments.Where(x => x.Salary >= 20000000 && x.Salary <= request.Salary);
                }
                else if (request.Salary > 50000000)
                {
                    recruiments = recruiments.Where(x => x.Salary >= 50000000);
                }

            }
            if (!string.IsNullOrEmpty(request.Education))
            {
                recruiments = recruiments.Where(x => x.Education.ToUpper() == request.Education.ToUpper());
            }
            if (!string.IsNullOrEmpty(request.Type))
            {
                recruiments = recruiments.Where(x => x.Type.ToUpper() == request.Type.ToUpper());
            }

            int totalRow = recruiments.Count();
            var data = recruiments.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new RecruitmentPagingResult()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Rank = x.Rank,
                    Salary = x.Salary,
                    DateCreated = x.DateCreated,
                    Branches = new List<string>()
                }).ToList();
            int length;
            if (request.PageSize < recruiments.Count())
            {
                length = request.PageSize;
            }
            else
            {
                length = recruiments.Count();
            }
            for (int i = 0; i < length; i++)
            {
                var infor = await this.GetCompanyInformation(recruiments.ElementAt(i).CompanyId);
                data[i].CompanyName = infor.ResultObj.Name;
                data[i].AvatarPath = infor.ResultObj.CompanyAvatar.ImagePath;

                var branchRecruitment = await _context.BranchRecruitments.Where(x => x.RecruimentId == recruiments.ElementAt(i).Id).ToListAsync();
                foreach (var item in branchRecruitment)
                {
                    //var branch = await _context.Branches.FindAsync(item.BranchId);
                    data[i].Branches.Add(item.Branch.City);

                }
            }

            var pagedResult = new PageResult<RecruitmentPagingResult>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PageResult<RecruitmentPagingResult>>(pagedResult);

        }

        public async Task<ApiResult<List<CommentViewModel>>> GetCommentRecruitment(int id)
        {
            var recruitment = await _context.Recruitments.FindAsync(id);
            if (recruitment == null)
            {
                return new ApiErrorResult<List<CommentViewModel>>("The job posting does not exist, please check again");
            }
            var commentVMs = new List<CommentViewModel>();
            var comments = await _context.Comments.Where(x => x.RecruimentId == id).ToListAsync();
            foreach (var comment in comments)
            {
                var informationCompany = await _context.CompanyInformations.FindAsync(comment.AccountId);
                if (informationCompany == null)
                {
                    var informationUser = await _context.UserInformations.FindAsync(comment.AccountId);
                    var avatar = await _context.UserAvatars.FirstOrDefaultAsync(x => x.UserId == comment.AccountId);
                    if (comment.SubcommentId != null)
                    {
                        var childCommentVM = new ChildCommentViewModel()
                        {
                            Id = comment.Id,
                            Content = comment.Content,
                            DateCreated = comment.DateCreated,
                            Name = informationUser.FirstName + " " + informationUser.LastName,
                            AvatarPath = avatar.ImagePath
                        };
                        var subComment = commentVMs.FirstOrDefault(x => x.Id == int.Parse(comment.SubcommentId));
                        subComment.ChildComments.Add(childCommentVM);
                    }
                    else
                    {
                        var commentVM = new CommentViewModel()
                        {
                            Id = comment.Id,
                            Content = comment.Content,
                            DateCreated = comment.DateCreated,
                            ChildComments = new List<ChildCommentViewModel>(),
                            Name = informationUser.FirstName + " " + informationUser.LastName,
                            AvatarPath = avatar.ImagePath
                        };
                        commentVMs.Add(commentVM);
                    }

                }
                else
                {
                    var avatar = await _context.CompanyAvatars.FirstOrDefaultAsync(x => x.CompanyId == comment.AccountId);
                    if (comment.SubcommentId != null)
                    {

                        var childCommentVM = new ChildCommentViewModel()
                        {
                            Id = comment.Id,
                            Content = comment.Content,
                            DateCreated = comment.DateCreated,
                            Name = informationCompany.Name,
                            AvatarPath = avatar.ImagePath
                        };
                        var subComment = commentVMs.FirstOrDefault(x => x.Id == int.Parse(comment.SubcommentId));
                        subComment.ChildComments.Add(childCommentVM);
                    }
                    else
                    {
                        var commentVM = new CommentViewModel()
                        {
                            Id = comment.Id,
                            Content = comment.Content,
                            DateCreated = comment.DateCreated,
                            ChildComments = new List<ChildCommentViewModel>(),
                            Name = informationCompany.Name,
                            AvatarPath = avatar.ImagePath
                        };
                        commentVMs.Add(commentVM);
                    }

                }
            }
            return new ApiSuccessResult<List<CommentViewModel>>(commentVMs);

        }

        public async Task<ApiResult<CompanyAvatarViewModel>> GetCompanyAvatar(Guid companyId)
        {
            var avatar = await _context.CompanyAvatars.FirstOrDefaultAsync(x => x.CompanyId == companyId);
            if (avatar == null)
            {
                return new ApiErrorResult<CompanyAvatarViewModel>("Something wrong, Please check company id");
            }

            var avatarVM = _mapper.Map<CompanyAvatarViewModel>(avatar);

            return new ApiSuccessResult<CompanyAvatarViewModel>(avatarVM);
        }



        public async Task<ApiResult<CompanyCoverImageViewModel>> GetCompanyCoverImage(Guid companyId)
        {
            var coverImage = await _context.CompanyCoverImages.FirstOrDefaultAsync(x => x.CompanyId == companyId);
            if (coverImage == null)
            {
                return new ApiErrorResult<CompanyCoverImageViewModel>("This company doesn't have cover image, Please upload cover image first");
            }

            var coverImageVM = _mapper.Map<CompanyCoverImageViewModel>(coverImage);

            return new ApiSuccessResult<CompanyCoverImageViewModel>(coverImageVM);
        }

        public async Task<ApiResult<CompanyInformationViewModel>> GetCompanyInformation(Guid companyId)
        {

            var companyInfor = await _context.CompanyInformations.FindAsync(companyId);
            if (companyInfor == null)
            {
                return new ApiErrorResult<CompanyInformationViewModel>("Company information could not be found, please check again");
            }

            var companyInforVM = _mapper.Map<CompanyInformationViewModel>(companyInfor);


            var companyAvatar = await this.GetCompanyAvatar(companyId);
            companyInforVM.CompanyAvatar = companyAvatar.ResultObj;
            var companyCoverImage = await this.GetCompanyCoverImage(companyId);
            companyInforVM.CompanyCoverImage = companyCoverImage.ResultObj;

            var queryImages = await _context.CompanyImages.Where(x => x.CompanyId == companyId).ToListAsync();
            var companyImages = queryImages.AsQueryable();

            var companyImagesVM = companyImages.Select(image => _mapper.Map<CompanyImagesViewModel>(image)).ToList();

            companyInforVM.CompanyImages = companyImagesVM;
            var companyBranch = await this.GetCompanyBranch(companyId);
            companyInforVM.CompanyBranches = companyBranch.ResultObj;

            var companyRecruitmentVMs = new List<CompanyRecruitmentViewModel>();
            var companyRecruitments = await _context.Recruitments.Where(x => x.CompanyId == companyId).ToListAsync();
            foreach (var recruitment in companyRecruitments)
            {
                var companyRecruitmentVM = new CompanyRecruitmentViewModel()
                {
                    Id = recruitment.Id,
                    Name = recruitment.Name,
                    StartDay = recruitment.DateCreated,
                    EndDate = recruitment.ExpirationDate,
                    Salary = recruitment.Salary,
                    RecruitmentBranches = new List<string>()
                };
                var branches = await _context.BranchRecruitments.Where(x => x.RecruimentId == recruitment.Id).ToListAsync();
                foreach (var item in branches)
                {
                    var branch = await _context.Branches.FindAsync(item.BranchId);
                    companyRecruitmentVM.RecruitmentBranches.Add(branch.City);
                }
                companyRecruitmentVMs.Add(companyRecruitmentVM);
            }
            companyInforVM.CompanyRecruitments = companyRecruitmentVMs;
            return new ApiSuccessResult<CompanyInformationViewModel>(companyInforVM);
        }

        public async Task<ApiResult<RecruitmentViewModel>> GetRecruitmentById(int id)
        {
            var recruitment = await _context.Recruitments.FindAsync(id);
            if (recruitment == null)
            {
                return new ApiErrorResult<RecruitmentViewModel>("Recruitment doesn't exits");
            }
            var recruitmentVM = _mapper.Map<RecruitmentViewModel>(recruitment);
            recruitmentVM.Branches = new List<string>();
            recruitmentVM.Careers = new List<string>();
            var branchRecruitment = await _context.BranchRecruitments.Where(x => x.RecruimentId == id).ToListAsync();
            foreach (var item in branchRecruitment)
            {
                var branch = await _context.Branches.FindAsync(item.BranchId);
                recruitmentVM.Branches.Add(branch.City);
            }
            var careerRecruitment = await _context.CareerRecruitments.Where(x => x.RecruimentId == id).ToListAsync();
            foreach (var item in careerRecruitment)
            {
                var career = await _context.Careers.FindAsync(item.CareerId);
                recruitmentVM.Careers.Add(career.Name);
            }
            var comments = await this.GetCommentRecruitment(id);
            if (comments.ResultObj == null)
            {
                recruitmentVM.ListComment = new List<CommentViewModel>();
            }
            else
            {
                recruitmentVM.ListComment = comments.ResultObj;

            }


            var information = await _context.CompanyInformations.FindAsync(recruitment.CompanyId);
            recruitmentVM.CompanyName = information.Name;
            recruitmentVM.CompanyId = information.UserId;
            var avatar = await _context.CompanyAvatars.FirstOrDefaultAsync(x => x.CompanyId == recruitment.CompanyId);
            recruitmentVM.AvatarPath = avatar.ImagePath;

            return new ApiSuccessResult<RecruitmentViewModel>(recruitmentVM);
        }

        public async Task<ApiResult<bool>> RemoveBranch(int id, Guid companyId)
        {
            var companyBranch = await _context.CompanyBranches.FirstOrDefaultAsync(x => x.BranchId == id && x.CompanyId == companyId);
            if (companyBranch == null)
            {
                return new ApiErrorResult<bool>("branch is currently not assigned to the company");
            }

            _context.CompanyBranches.Remove(companyBranch);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("Delete unsuccessful");
            }
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateAvatar(int Id, AvatarUpdateRequest request)
        {
            var companyAva = await _context.CompanyAvatars.FindAsync(Id);
            if (companyAva == null)
            {
                return new ApiErrorResult<bool>("Company avatar information could not be found");
            }


            var imageIndex = request.ThumnailImage.FileName.LastIndexOf(".");
            var imageType = request.ThumnailImage.FileName.Substring(imageIndex + 1);
            if (imageType == "jpg" || imageType == "png")
            {
                if (companyAva.ImagePath != "default-avatar.jpg")
                {
                    await _storageService.DeleteAvatarAsync(companyAva.ImagePath);
                }
                companyAva.FizeSize = request.ThumnailImage.Length;
                companyAva.Caption = request.ThumnailImage.FileName;
                companyAva.ImagePath = await this.SaveAvatar(request.ThumnailImage);
                companyAva.DateCreated = DateTime.Now;
                var result = await _context.SaveChangesAsync();

                if (result == 0)
                {
                    return new ApiErrorResult<bool>("An error occured, register unsuccessful");
                }
                return new ApiSuccessResult<bool>(true);
            }
            return new ApiErrorResult<bool>("Please choose jpg or png image file");

        }

        public async Task<ApiResult<bool>> UpdateCompanyName(Guid id, string name)
        {
            var user = await _context.CompanyInformations.FindAsync(id);
            if (user == null)
            {
                return new ApiErrorResult<bool>("Tài khoản không tồn tại, vui lòng thử lại");
            }
            user.Name = name;
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("Vui lòng thay đổi dữ liệu");
            }
            return new ApiSuccessResult<bool>(true);
        }
        public async Task<ApiResult<bool>> UpdateCompanyContactName(Guid id, string contactName)
        {
            var user = await _context.CompanyInformations.FindAsync(id);
            if (user == null)
            {
                return new ApiErrorResult<bool>("Tài khoản không tồn tại, vui lòng thử lại");
            }
            user.ContactName = contactName;
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("Vui lòng thay đổi dữ liệu");
            }
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateCompanyDescription(Guid id, string description)
        {
            var user = await _context.CompanyInformations.FindAsync(id);
            if (user == null)
            {
                return new ApiErrorResult<bool>("Tài khoản không tồn tại, vui lòng thử lại");
            }
            user.Description = description;
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("Vui lòng thay đổi dữ liệu");
            }
            return new ApiSuccessResult<bool>(true);
        }
        public async Task<ApiResult<bool>> UpdateCompanyWorkerNumber(Guid id, int workerNumber)
        {
            var user = await _context.CompanyInformations.FindAsync(id);
            if (user == null)
            {
                return new ApiErrorResult<bool>("Tài khoản không tồn tại, vui lòng thử lại");
            }
            user.WorkerNumber = workerNumber;
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("Vui lòng thay đổi dữ liệu");
            }
            return new ApiSuccessResult<bool>(true);
        }


        public async Task<ApiResult<bool>> UpdateCoverImage(int id, UpdateCoverImageRequest request)
        {
            var companyCoverImage = await _context.CompanyCoverImages.FindAsync(id);
            if (companyCoverImage == null)
            {
                return new ApiErrorResult<bool>("Company Cover Image could not be found");
            }
            var imageIndex = request.ThumnailImage.FileName.LastIndexOf(".");
            var imageType = request.ThumnailImage.FileName.Substring(imageIndex + 1);
            if (imageType == "jpg" || imageType == "png")
            {
                await _storageService.DeleteCoverImageAsync(companyCoverImage.ImagePath);
                companyCoverImage.FizeSize = request.ThumnailImage.Length;
                companyCoverImage.Caption = request.ThumnailImage.FileName;
                companyCoverImage.ImagePath = await this.SaveCoverImage(request.ThumnailImage);
                companyCoverImage.DateCreated = DateTime.Now;
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

        private async Task<string> SaveCoverImage(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveCoverImageAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        private async Task<string> SaveImages(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveImagesAsync(file.OpenReadStream(), fileName);
            return fileName;
        }



        public async Task<ApiResult<List<CompanyBranchViewModel>>> GetCompanyBranch(Guid companyId)
        {
            var company = await _context.CompanyInformations.FindAsync(companyId);
            if (company == null)
            {
                return new ApiErrorResult<List<CompanyBranchViewModel>>("Không tìm thấy công ty hiện tại, vui lòng kiểm tra lại");
            }
            var branches = await _context.CompanyBranches.Where(x => x.CompanyId == companyId).ToListAsync();
            var branchVM = branches.Select(companyBranch => _mapper.Map<CompanyBranchViewModel>(companyBranch)).ToList();
            foreach (var item in branchVM)
            {
                var branch = await _context.Branches.FindAsync(item.BranchId);
                item.City = branch.City;
            }
            branchVM = branchVM.OrderBy(x => x.City).ToList();

            return new ApiSuccessResult<List<CompanyBranchViewModel>>(branchVM);
        }

        public async Task<List<BranchViewModel>> GetBranchesNotExist(Guid companyId)
        {
            var branches = await _context.Branches.ToListAsync();
            var branchExists = await _context.CompanyBranches.Where(x => x.CompanyId == companyId).ToListAsync();
            foreach (var item in branchExists)
            {
                var branch = await _context.Branches.FindAsync(item.BranchId);
                branches.Remove(branch);

            }
            var branchVM = branches.Select(branch => _mapper.Map<BranchViewModel>(branch)).OrderBy(x => x.City).ToList();
            return branchVM;

        }

        public async Task<ApiResult<bool>> UpdateRecruitmentName(UpdateRecruitmentNameRequest request)
        {
            var recruitment = await _context.Recruitments.FindAsync(request.Id);
            if (recruitment == null)
            {
                return new ApiErrorResult<bool>("Recruitment doesn't exist");
            }
            recruitment.Name = request.Name;
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateRecruitmentSalary(UpdateSalaryRecruitment request)
        {
            var recruitment = await _context.Recruitments.FindAsync(request.Id);
            if (recruitment == null)
            {
                return new ApiErrorResult<bool>("Recruitment doesn't exist");
            }
            recruitment.Salary = request.Salary;
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<List<ListCompanyRecruitment>>> GetListCompanyRecruitment(Guid companyId)
        {
            var companyRecruitments = await _context.Recruitments.Where(x => x.CompanyId == companyId).OrderBy(x => x.ExpirationDate).ToListAsync();
            var listCompanyRecruitment = new List<ListCompanyRecruitment>();
            foreach (var recruitment in companyRecruitments)
            {
                var companyRecruitment = _mapper.Map<ListCompanyRecruitment>(recruitment);
                companyRecruitment.Branches = new List<string>();
                companyRecruitment.Careers = new List<string>();
                var branchRecruitment = await _context.BranchRecruitments.Where(x => x.RecruimentId == recruitment.Id).ToListAsync();
                foreach (var item in branchRecruitment)
                {
                    var branch = await _context.Branches.FindAsync(item.BranchId);
                    companyRecruitment.Branches.Add(branch.City);
                }
                var careerRecruitment = await _context.CareerRecruitments.Where(x => x.RecruimentId == recruitment.Id).ToListAsync();
                foreach (var item in careerRecruitment)
                {
                    var career = await _context.Careers.FindAsync(item.CareerId);
                    companyRecruitment.Careers.Add(career.Name);
                }
                listCompanyRecruitment.Add(companyRecruitment);
            }
            return new ApiSuccessResult<List<ListCompanyRecruitment>>(listCompanyRecruitment);

        }

        public async Task<List<CareerViewModel>> GetCareersRecruitmentNotExist(int id)
        {
            var careers = await _context.Careers.ToListAsync();
            var careerExists = await _context.CareerRecruitments.Where(x => x.RecruimentId == id).ToListAsync();
            foreach (var item in careerExists)
            {
                var career = await _context.Careers.FindAsync(item.CareerId);
                careers.Remove(career);

            }
            var careerVM = careers.Select(career => _mapper.Map<CareerViewModel>(career)).OrderBy(x => x.Name).ToList();
            return careerVM;
        }

        public async Task<List<BranchViewModel>> GetBranchesRecruitmentNotExist(int id)
        {
            var branches = await _context.Branches.ToListAsync();
            var branchExists = await _context.BranchRecruitments.Where(x => x.RecruimentId == id).ToListAsync();
            foreach (var item in branchExists)
            {
                var branch = await _context.Branches.FindAsync(item.BranchId);
                branches.Remove(branch);

            }
            var branchVM = branches.Select(branch => _mapper.Map<BranchViewModel>(branch)).OrderBy(x => x.City).ToList();
            return branchVM;

        }

        public async Task<ApiResult<bool>> RemoveCareerFromRecruitment(int recruimentId, int careerId)
        {
            var careerRecruitment = await _context.CareerRecruitments.FirstOrDefaultAsync(x => x.RecruimentId == recruimentId && x.CareerId == careerId);
            if (careerRecruitment == null)
            {
                return new ApiErrorResult<bool>("Nghề này chưa được thêm, vui lòng kiểm tra lại");
            }
            _context.CareerRecruitments.Remove(careerRecruitment);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> RemoveBranchFromRecruitment(int recruimentId, int branchId)
        {
            var branchRecruitment = await _context.BranchRecruitments.FirstOrDefaultAsync(x => x.RecruimentId == recruimentId && x.BranchId == branchId);
            if (branchRecruitment == null)
            {
                return new ApiErrorResult<bool>("Việc làm chưa có chi nhánh ở thành phố này, vui lòng kiểm tra lại");
            }
            _context.BranchRecruitments.Remove(branchRecruitment);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<List<CareerViewModel>> GetCareersRecruitmentExist(int id)
        {
            var careerList = new List<Career>();
            var careerExists = await _context.CareerRecruitments.Where(x => x.RecruimentId == id).ToListAsync();
            foreach (var item in careerExists)
            {
                var career = await _context.Careers.FindAsync(item.CareerId);
                careerList.Add(career);

            }
            var careerVM = careerList.Select(career => _mapper.Map<CareerViewModel>(career)).OrderBy(x => x.Name).ToList();
            return careerVM;
        }

        public async Task<List<BranchViewModel>> GetBranchesRecruitmentExist(int id)
        {
            var branches = new List<Branch>();
            var branchExists = await _context.BranchRecruitments.Where(x => x.RecruimentId == id).ToListAsync();
            foreach (var item in branchExists)
            {
                var branch = await _context.Branches.FindAsync(item.BranchId);
                branches.Add(branch);

            }
            var branchVM = branches.Select(branch => _mapper.Map<BranchViewModel>(branch)).OrderBy(x => x.City).ToList();
            return branchVM;
        }

        public async Task<ApiResult<bool>> ExtendRecruitment(ExtendRecruitmentRequest request)
        {
            var recruitment = await _context.Recruitments.FindAsync(request.Id);
            recruitment.ExpirationDate = request.NewExpirationDate;
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<List<CVViewModel>>> GetAllCV(int id)
        {
            var CVViewModel = new List<CVViewModel>();
            var CVs = await _context.CurriculumVitaes.Where(x => x.RecruimentId == id).OrderBy(x => x.DateCreated).ToListAsync();
            foreach (var item in CVs)
            {
                var userInfor = await _userService.GetUserInformation(item.UserId);
                var recruitment = await _context.Recruitments.FindAsync(item.RecruimentId);
                var CVVM = new CVViewModel()
                {
                    DateSubit = item.DateCreated,
                    FilePath = item.FilePath,
                    User = userInfor.ResultObj,
                    RecruitmentId = item.RecruimentId,
                    UserId = item.UserId,
                    RecruitmentName = recruitment.Name
                };
                CVViewModel.Add(CVVM);
            }
            return new ApiSuccessResult<List<CVViewModel>>(CVViewModel);

        }

        public async Task<ApiResult<bool>> AcceptCV(int recruitmentId, Guid userId)
        {
            var CV = await _context.CurriculumVitaes.FirstOrDefaultAsync(x => x.UserId == userId && x.RecruimentId == recruitmentId);
            if (CV == null)
            {
                return new ApiErrorResult<bool>("CV này hiện không tồn tại, vui lòng kiểm tra lại");
            }

            _context.CurriculumVitaes.Remove(CV);
            var recruitment = await _context.Recruitments.FindAsync(recruitmentId);
            var company = await _context.CompanyInformations.FindAsync(recruitment.CompanyId);
            var noti = new Notification()
            {
                AccountId = userId,
                DateCreated = DateTime.Now,
                Content = company.Name + " vừa chấp thuận CV của bạn, vui lòng liên hệ trực tiếp để được giải quyết!"
            };
            _context.Notifications.Add(noti);
            await _context.SaveChangesAsync();
            var user = await _context.Users.FindAsync(userId);
            string to = user.Email;
            string subject = "Chúc mừng bạn đã ứng tuyển thành công";
            string body = "Bạn đã được công ty " + company.Name + " đồng ý ứng tuyển, vui lòng vào website liên hệ với công ty.";

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

        public async Task<ApiResult<bool>> RefuseCV(int recruitmentId, Guid userId)
        {
            var CV = await _context.CurriculumVitaes.FirstOrDefaultAsync(x => x.UserId == userId && x.RecruimentId == recruitmentId);
            if (CV == null)
            {
                return new ApiErrorResult<bool>("CV này hiện không tồn tại, vui lòng kiểm tra lại");
            }

            _context.CurriculumVitaes.Remove(CV);
            var recruitment = await _context.Recruitments.FindAsync(recruitmentId);
            var company = await _context.CompanyInformations.FindAsync(recruitment.CompanyId);
            var noti = new Notification()
            {
                AccountId = userId,
                DateCreated = DateTime.Now,
                Content = company.Name + " vừa chấp thuận CV của bạn, vui lòng liên hệ trực tiếp để được giải quyết!"
            };
            _context.Notifications.Add(noti);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true);
        }

        public DownloadFileViewModel DownloadCV(string filePath)
        {
            var fileDownload = _storageService.DownloadZip(filePath);
            return fileDownload;
        }
        public async Task<ApiResult<ChatViewModel>> GetAllChat(Guid userId, Guid companyId, string role)
        {
            var chats = await _context.Chats.Where(x => x.UserId == userId && x.CompanyId == companyId).OrderBy(x => x.DateCreated).ToListAsync();
            var chatVM = new ChatViewModel();
            var data = chats.Select(chat => _mapper.Map<ListChatContent>(chat)).ToList();
            if (role == "user")
            {
                var company = await this.GetCompanyInformation(companyId);
                chatVM = new ChatViewModel()
                {
                    Name = company.ResultObj.Name,
                    AvatarPath = company.ResultObj.CompanyAvatar.ImagePath,
                    Content = data
                };
            }
            else
            {
                var user = await _userService.GetUserInformation(userId);
                chatVM = new ChatViewModel()
                {
                    Name = user.ResultObj.FirstName + " " + user.ResultObj.LastName,
                    AvatarPath = user.ResultObj.UserAvatar.ImagePath,
                    Content = data
                };
            }
            return new ApiSuccessResult<ChatViewModel>(chatVM);
        }

        public async Task<ApiResult<bool>> Chat(ChatRequest request)
        {

            var chat = new Chat()
            {
                CompanyId = request.CompanyId,
                UserId = request.UserId,
                Content = request.Content,
                Performer = request.Performer,
                DateCreated = DateTime.Now
            };
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<List<PersonChat>>> GetAllPersonChat(Guid id, string role)
        {
            var persons = new List<PersonChat>();
            var query = await _context.Chats.OrderByDescending(x => x.DateCreated).ToListAsync();

            var queryArr = query.ToArray();
            var chats = new List<Chat>();
            if (role == "user")
            {
                var idExists = new List<Guid>();
                for (int i = 0; i < queryArr.Length; i++)
                {
                    if (i == 0)
                    {
                        chats.Add(queryArr[i]);
                        idExists.Add(queryArr[i].CompanyId);
                    }
                    else
                    {
                        if (!idExists.Any(id => id == queryArr[i].CompanyId))
                        {
                            chats.Add(queryArr[i]);
                            idExists.Add(queryArr[i].CompanyId);
                        }
                    }
                }
            }
            else
            {
                var idExists = new List<Guid>();
                for (int i = 0; i < queryArr.Length; i++)
                {
                    if (i == 0)
                    {
                        chats.Add(queryArr[i]);
                        idExists.Add(queryArr[i].UserId);
                    }
                    else
                    {
                        if (!idExists.Any(id => id == queryArr[i].UserId))
                        {
                            chats.Add(queryArr[i]);
                            idExists.Add(queryArr[i].UserId);
                        }
                    }
                }
            }

            foreach (var chat in chats)
            {
                if (role == "user")
                {
                    var company = await this.GetCompanyInformation(chat.CompanyId);
                    var newPerson = new PersonChat()
                    {
                        Id = chat.CompanyId,
                        LastContent = chat.Content,
                        DateCreated = chat.DateCreated,
                        Name = company.ResultObj.Name,
                        AvatarPath = company.ResultObj.CompanyAvatar.ImagePath
                    };
                    persons.Add(newPerson);
                }
                else
                {
                    var user = await _userService.GetUserInformation(chat.UserId);
                    var newPerson = new PersonChat()
                    {
                        Id = chat.UserId,
                        LastContent = chat.Content,
                        DateCreated = chat.DateCreated,
                        Name = user.ResultObj.FirstName + " " + user.ResultObj.LastName,
                        AvatarPath = user.ResultObj.UserAvatar.ImagePath
                    };
                    persons.Add(newPerson);
                }

            }

            return new ApiSuccessResult<List<PersonChat>>(persons);
        }

        public async Task<ApiResult<List<AllCompanyResult>>> GetAllCompany()
        {
            var results = new List<AllCompanyResult>();
            var companies = await _userManager.GetUsersInRoleAsync("company");
            foreach (var company in companies)
            {
                var infor = await this.GetCompanyInformation(company.Id);
                var recruitment = await this.GetListCompanyRecruitment(company.Id);
                var branch = await this.GetCompanyBranch(company.Id);
                var result = new AllCompanyResult()
                {
                    Id = company.Id,
                    Name = infor.ResultObj.Name,
                    AvatarPath = infor.ResultObj.CompanyAvatar.ImagePath,
                    CountRecruitment = recruitment.ResultObj.Count,
                    Branches = branch.ResultObj,
                    WorkerNumber = infor.ResultObj.WorkerNumber
                };
                results.Add(result);
            }
            return new ApiSuccessResult<List<AllCompanyResult>>(results);
        }

        public async Task<ApiResult<List<NotifyViewModel>>> GetAllNotify(Guid id)
        {
            var notifies = await _context.Notifications.Where(x => x.AccountId == id).OrderByDescending(x => x.DateCreated).ToListAsync();
            var result = notifies.Select(x => new NotifyViewModel()
            {
                Id = x.Id,
                Content = x.Content,
                DateCreated = x.DateCreated
            }).ToList();
            return new ApiSuccessResult<List<NotifyViewModel>>(result);
        }

        public async Task<ApiResult<bool>> DeleteRecruitment(int id)
        {
            var recruitment = await _context.Recruitments.FindAsync(id);
            var CVs = await _context.CurriculumVitaes.Where(x => x.RecruimentId == id).ToListAsync();
            foreach (var cv in CVs)
            {
                _context.CurriculumVitaes.Remove(cv);
                await _context.SaveChangesAsync();
            }
            var comments = await _context.Comments.Where(x => x.RecruimentId == id).ToListAsync();
            foreach (var comment in comments)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
            _context.Recruitments.Remove(recruitment);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true);
        }
    }
}
