using Application.Common;
using Data.EF;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
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

        public async Task<ApiResult<PageResult<CompanyBranchViewModel>>> GetAllBranchPaging(GetCompanyBranchRequest request)
        {
            throw new NotImplementedException();
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
                await _storageService.DeleteFileAsync(companyAva.ImagePath);
            }

            companyAva.FizeSize = thumnailImage.Length;
            companyAva.Caption = thumnailImage.FileName;
            companyAva.ImagePath = await this.SaveFile(thumnailImage);
            companyAva.DateCreated = DateTime.Now;
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
