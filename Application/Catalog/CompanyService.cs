using Application.Common;
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
        public CompanyService(RecruimentWebsiteDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<ApiResult<bool>> CreateBranch(CreateBranchRequest request)
        {
            var company = await _context.CompanyInformations.FindAsync(request.CompanyId);
            if (company == null)
            {
                return new ApiErrorResult<bool>("Company doesn't exist, please try again");
            }

            var companyBranch = new CompanyBranch()
            {
                Address = request.Address,
                CompanyId = request.CompanyId
            };

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

            var data = branch.Select(x => new CompanyBranchViewModel()
            {
                Addresss = x.Address

            }).ToList();


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
                .Select(x => new CompanyImagesViewModel()
                {
                    DateCreated = x.DateCreated,
                    Caption = x.Caption,
                    FizeSize = x.FizeSize,
                    ImagePath = x.ImagePath
                }).ToList();

            var pagedResult = new PageResult<CompanyImagesViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PageResult<CompanyImagesViewModel>>(pagedResult);
        }

        public async Task<ApiResult<bool>> UpdateAvatar(Guid id, IFormFile thumnailImage)
        {
            var companyAva = await _context.CompanyAvatars.FirstOrDefaultAsync(x => x.CompanyId == id);
            if (companyAva == null)
            {
                return new ApiErrorResult<bool>("User avatar information could not be found");
            }

            if (companyAva.ImagePath != "default-avatar")
            {
                await _storageService.DeleteAvatarAsync(companyAva.ImagePath);
            }
            var imageIndex = thumnailImage.FileName.LastIndexOf(".");
            var imageType = thumnailImage.FileName.Substring(imageIndex + 1);
            if (imageType == "jpg" || imageType == "png")
            {
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

        //Save File
        private async Task<string> SaveAvatar(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveAvatarAsync(file.OpenReadStream(), fileName);
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
