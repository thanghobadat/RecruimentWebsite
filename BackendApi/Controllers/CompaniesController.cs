﻿using Application.Catalog;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ViewModel.Catalog.Company;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
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



        [HttpGet("GetCompanyImages")]
        public async Task<IActionResult> GetCompanyImages([FromQuery] GetCompanyImagesRequest request)
        {
            var result = await _companyService.GetAllImagesPaging(request);
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

        [HttpPost("AddBranchToCompany")]
        public async Task<IActionResult> AddBranchToCompany([FromBody] AddBranchViewModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _companyService.AddBranch(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPut("UpdateCompanyInformation")]
        public async Task<IActionResult> UpdateCompanyInformation([FromBody] CompanyUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _companyService.UpdateCompanyInformation(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        [HttpPut("UpdateAvatar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateAvatar(int id, [FromForm] AvatarUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _companyService.UpdateAvatar(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("UpdateCoverImage")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateCoverImage(int id, [FromForm] UpdateCoverImageRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _companyService.UpdateCoverImage(id, request);
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



        [HttpDelete("DeleteImages")]
        public async Task<IActionResult> DeleteImages(int id)
        {
            var result = await _companyService.DeleteImages(id);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("RemoveBranch")]
        public async Task<IActionResult> RemoveBranch(int id, Guid companyId)
        {
            var result = await _companyService.RemoveBranch(id, companyId);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }


    }
}
