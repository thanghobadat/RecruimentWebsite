﻿using Application.Common;
using AutoMapper;
using Data.EF;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ViewModel.Catalog.Company;
using ViewModel.Common;

namespace Application.Catalog
{
    public class CompanyService : ICompanyService
    {
        private readonly RecruimentWebsiteDbContext _context;
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;
        public CompanyService(RecruimentWebsiteDbContext context, IStorageService storageService,
            IMapper mapper)
        {
            _context = context;
            _storageService = storageService;
            _mapper = mapper;
        }

        public async Task<ApiResult<bool>> CreateBranch(CreateBranchRequest request)
        {
            var company = await _context.CompanyInformations.FindAsync(request.CompanyId);
            if (company == null)
            {
                return new ApiErrorResult<bool>("Company doesn't exist, please try again");
            }

            var companyBranch = _mapper.Map<CompanyBranch>(request);

            await _context.CompanyBranches.AddAsync(companyBranch);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("An error occured, register unsuccessful");
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

            foreach (var image in request.Images)
            {
                var imageIndex = image.FileName.LastIndexOf(".");
                var imageType = image.FileName.Substring(imageIndex + 1);
                if (imageType == "jpg" || imageType == "png")
                {
                    var companyImage = new CompanyImage()
                    {
                        FizeSize = image.Length,
                        Caption = image.FileName,
                        DateCreated = DateTime.Now,
                        CompanyId = request.CompanyId,
                        ImagePath = await this.SaveImages(image)
                    };

                    await _context.CompanyImages.AddAsync(companyImage);
                }
                else
                {
                    return new ApiErrorResult<bool>("Please choose jpg or png image file");
                }

            }

            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("An error occured, register unsuccessful");
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
                FizeSize = request.ThumnailIage.Length,
                Caption = request.ThumnailIage.FileName,
                DateCreated = DateTime.Now,
                CompanyId = request.CompanyId,
                ImagePath = await this.SaveCoverImage(request.ThumnailIage)
            };

            await _context.CompanyCoverImages.AddAsync(coverImage);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("An error occured, register unsuccessful");
            }
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> DeleteBranch(int id)
        {
            var branch = await _context.CompanyBranches.FindAsync(id);
            if (branch == null)
            {
                return new ApiErrorResult<bool>("Branch doesn't exist");
            }


            _context.CompanyBranches.Remove(branch);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("An error occured, delete unsuccessful");
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

        public async Task<ApiResult<bool>> DeleteImages(List<int> listId)
        {
            foreach (var id in listId)
            {
                var image = await _context.CompanyImages.FindAsync(id);
                if (image == null)
                {
                    return new ApiErrorResult<bool>("Image doesn't exist");
                }
                await _storageService.DeleteImagesAsync(image.ImagePath);
                _context.CompanyImages.Remove(image);
            }


            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("An error occured, delete unsuccessful");
            }
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<List<CompanyBranchViewModel>>> GetAllBranchPaging(GetCompanyBranchRequest request)
        {
            var query = await _context.CompanyBranches.Where(x => x.CompanyId == request.CompanyId).ToListAsync();

            if (query == null)
            {
                return new ApiErrorResult<List<CompanyBranchViewModel>>("Branch doesn't exist, Please check again");
            }

            var branch = query.AsQueryable();
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                branch = branch.Where(x => x.Address.Contains(request.Keyword));

            }

            var data = branch.Select(branch => _mapper.Map<CompanyBranchViewModel>(branch)).ToList();


            return new ApiSuccessResult<List<CompanyBranchViewModel>>(data);
        }

        public async Task<ApiResult<PageResult<CompanyImagesViewModel>>> GetAllImagesPaging(GetCompanyImagesRequest request)
        {
            var company = await _context.CompanyInformations.FindAsync(request.CompanyId);
            if (company == null)
            {
                return new ApiErrorResult<PageResult<CompanyImagesViewModel>>("Company doesn't exist, please re-enter");
            }

            var query = await _context.CompanyImages.Where(x => x.CompanyId == request.CompanyId).ToListAsync();
            var companyImages = query.AsQueryable();
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                companyImages = companyImages.Where(x => x.Caption.Contains(request.Keyword));
            }

            int totalRow = companyImages.Count();
            var data = companyImages.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(image => _mapper.Map<CompanyImagesViewModel>(image)).ToList();

            var pagedResult = new PageResult<CompanyImagesViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PageResult<CompanyImagesViewModel>>(pagedResult);
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

            var queryBranchs = await _context.CompanyBranches.Where(x => x.CompanyId == companyId).ToListAsync();
            var companyBranchs = queryBranchs.AsQueryable();

            var companyBranchsVM = companyBranchs.Select(branch => _mapper.Map<CompanyBranchViewModel>(branch)).ToList();

            companyInforVM.CompanyBranches = companyBranchsVM;

            var queryImages = await _context.CompanyImages.Where(x => x.CompanyId == companyId).ToListAsync();
            var companyImages = queryImages.AsQueryable();

            var companyImagesVM = companyImages.Select(image => _mapper.Map<CompanyImagesViewModel>(image)).ToList();

            companyInforVM.CompanyImages = companyImagesVM;
            return new ApiSuccessResult<CompanyInformationViewModel>(companyInforVM);
        }

        public async Task<ApiResult<bool>> UpdateAvatar(int id, IFormFile thumnailImage)
        {
            var companyAva = await _context.CompanyAvatars.FindAsync(id);
            if (companyAva == null)
            {
                return new ApiErrorResult<bool>("User avatar information could not be found");
            }


            var imageIndex = thumnailImage.FileName.LastIndexOf(".");
            var imageType = thumnailImage.FileName.Substring(imageIndex + 1);
            if (imageType == "jpg" || imageType == "png")
            {
                if (companyAva.ImagePath != "default-avatar")
                {
                    await _storageService.DeleteAvatarAsync(companyAva.ImagePath);
                }
                companyAva.FizeSize = thumnailImage.Length;
                companyAva.Caption = thumnailImage.FileName;
                companyAva.ImagePath = await this.SaveAvatar(thumnailImage);
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

        public async Task<ApiResult<bool>> UpdateBranch(UpdateBranchRequest request)
        {
            var branch = await _context.CompanyBranches.FindAsync(request.Id);
            if (branch == null)
            {
                return new ApiErrorResult<bool>("Branch doesn't exist");
            }

            branch.Address = request.Address;
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new ApiErrorResult<bool>("An error occured, register unsuccessful");
            }
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateCoverImage(int id, IFormFile thumnailImage)
        {
            var companyCoverImage = await _context.CompanyCoverImages.FindAsync(id);
            if (companyCoverImage == null)
            {
                return new ApiErrorResult<bool>("User avatar information could not be found");
            }
            var imageIndex = thumnailImage.FileName.LastIndexOf(".");
            var imageType = thumnailImage.FileName.Substring(imageIndex + 1);
            if (imageType == "jpg" || imageType == "png")
            {
                await _storageService.DeleteCoverImageAsync(companyCoverImage.ImagePath);
                companyCoverImage.FizeSize = thumnailImage.Length;
                companyCoverImage.Caption = thumnailImage.FileName;
                companyCoverImage.ImagePath = await this.SaveCoverImage(thumnailImage);
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
    }
}
