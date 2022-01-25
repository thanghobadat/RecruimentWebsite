using Application.Catalog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.Catalog.Company;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }


        [HttpGet("GetCompanyInformation")]
        public async Task<IActionResult> GetCompanyInformation(Guid companyId)
        {
            var result = await _companyService.GetCompanyInformation(companyId);
            return Ok(result);
        }


        [HttpGet("GetCompanyAvatar")]
        public async Task<IActionResult> GetCompanyAvatar(Guid companyId)
        {
            var result = await _companyService.GetCompanyAvatar(companyId);
            return Ok(result);
        }


        [HttpGet("GetCompanyCoverImage")]
        public async Task<IActionResult> GetCompanyCoverImage(Guid companyId)
        {
            var result = await _companyService.GetCompanyCoverImage(companyId);
            return Ok(result);
        }

        [HttpGet("GetCompanyBranches")]
        public async Task<IActionResult> GetCompanyBranches([FromQuery] GetCompanyBranchRequest request)
        {
            var result = await _companyService.GetAllBranchPaging(request);
            return Ok(result);
        }
        [HttpGet("GetCompanyImages")]
        public async Task<IActionResult> GetCompanyImages([FromQuery] GetCompanyImagesRequest request)
        {
            var result = await _companyService.GetAllImagesPaging(request);
            return Ok(result);
        }


        [HttpPost("CreateNewBranch")]
        public async Task<IActionResult> CreateNewBranch([FromBody] CreateBranchRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _companyService.CreateBranch(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("CreateCompanyImages")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateCompanyImages([FromForm] CreateCompanyImageRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _companyService.CreateCompanyImages(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("CreateCoverImages")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateCoverImages([FromForm] CreateCoverImageRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _companyService.CreateCoverImage(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("UpdateAvatar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateAvatar([FromForm] int id, IFormFile thumnailImage)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _companyService.UpdateAvatar(id, thumnailImage);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("UpdateCoverImage")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateCoverImage([FromForm] int id, IFormFile thumnailImage)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _companyService.UpdateCoverImage(id, thumnailImage);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("UpdateBranch")]
        public async Task<IActionResult> UpdateBranch([FromBody] UpdateBranchRequest request)
        {
            var result = await _companyService.UpdateBranch(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("DeleteCoverImage")]
        public async Task<IActionResult> DeleteCoverImage(int id)
        {
            var result = await _companyService.DeleteCoverImage(id);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("DeleteBranch")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var result = await _companyService.DeleteBranch(id);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("DeleteImages")]
        public async Task<IActionResult> DeleteImages(List<int> id)
        {
            var result = await _companyService.DeleteImages(id);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
