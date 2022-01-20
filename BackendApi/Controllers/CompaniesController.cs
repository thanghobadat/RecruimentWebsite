using Application.Catalog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpPut("UpdateAvatar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateAvatar([FromForm] Guid id, IFormFile thumnailImage)
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
    }
}
