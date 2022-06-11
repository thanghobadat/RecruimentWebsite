using Application.Catalog;
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
            var result = await _companyService.GetAllImages(request);
            return Ok(result);
        }

        [HttpGet("GetBranchesNotExist")]
        public async Task<IActionResult> GetBranchesNotExist(Guid companyId)
        {
            var result = await _companyService.GetBranchesNotExist(companyId);
            return Ok(result);
        }

        [HttpGet("GetCompanyBrahches")]
        public async Task<IActionResult> GetCompanyBrahches(Guid companyId)
        {
            var result = await _companyService.GetCompanyBranch(companyId);
            return Ok(result);
        }


        [HttpGet("GetAllRecruitmentPaging")]
        public async Task<IActionResult> GetCompanyImages([FromQuery] GetRecruitmentRequest request)
        {
            var result = await _companyService.GetAllRecruitmentPaging(request);
            return Ok(result);
        }

        [HttpGet("GetAllCompanyRecruitment")]
        public async Task<IActionResult> GetAllCompanyRecruitment(Guid companyId)
        {
            var result = await _companyService.GetListCompanyRecruitment(companyId);
            return Ok(result);
        }
        [HttpGet("GetRecruitmentById")]
        public async Task<IActionResult> GetRecruitmentById(int id)
        {
            var result = await _companyService.GetRecruitmentById(id);
            return Ok(result);
        }
        [HttpGet("GetCareerRecruitmentNotExist")]
        public async Task<IActionResult> GetCareerRecruitmentNotExist(int id)
        {
            var result = await _companyService.GetCareersRecruitmentNotExist(id);
            return Ok(result);
        }
        [HttpGet("GetCareerRecruitmentExist")]
        public async Task<IActionResult> GetCareerRecruitmentExist(int id)
        {
            var result = await _companyService.GetCareersRecruitmentExist(id);
            return Ok(result);
        }
        [HttpGet("GetBranchesRecruitmentNotExist")]
        public async Task<IActionResult> GetBranchesRecruitmentNotExist(int id)
        {
            var result = await _companyService.GetBranchesRecruitmentNotExist(id);
            return Ok(result);
        }
        [HttpGet("GetBranchesRecruitmentExist")]
        public async Task<IActionResult> GetBranchesRecruitmentExist(int id)
        {
            var result = await _companyService.GetBranchesRecruitmentExist(id);
            return Ok(result);
        }
        [HttpGet("GetAllCV")]
        public async Task<IActionResult> GetAllCV(int id)
        {
            var result = await _companyService.GetAllCV(id);
            return Ok(result);
        }

        [HttpGet("DownloadCV")]
        public IActionResult DownloadCV(string fileName)
        {
            var response = _companyService.DownloadCV(fileName);
            return File(response.ArchiveData, response.FiltType, response.AchiveName);
        }


        [HttpGet("GetAllChat")]
        public async Task<IActionResult> GetAllChat(Guid userId, Guid companyId, string role)
        {
            var result = await _companyService.GetAllChat(userId, companyId, role);
            return Ok(result);
        }
        [HttpGet("GetAllPersonChatChat")]
        public async Task<IActionResult> GetAllPersonChatChat(Guid id, string role)
        {
            var result = await _companyService.GetAllPersonChat(id, role);
            return Ok(result);
        }

        [HttpGet("GetAllCompany")]
        public async Task<IActionResult> GetAllCompany()
        {
            var result = await _companyService.GetAllCompany();
            return Ok(result);
        }
        [HttpGet("GetAllNotify")]
        public async Task<IActionResult> GetAllNotify(Guid id)
        {
            var result = await _companyService.GetAllNotify(id);
            return Ok(result);
        }

        [HttpPost("CreateCompanyImages")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateCompanyImages([FromForm] CreateCompanyImageRequest request)
        {


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


            var result = await _companyService.AddBranch(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpPost("AddBranchToRecruitment")]
        public async Task<IActionResult> AddBranchToRecruitment(int recruimentId, int branchId)
        {


            var result = await _companyService.AddBranchToRecruitment(recruimentId, branchId);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpPost("AddCareerToRecruitment")]
        public async Task<IActionResult> AddCareerToRecruitment(int recruimentId, int careerId)
        {


            var result = await _companyService.AddCareerToRecruitment(recruimentId, careerId);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpPost("RemoveBranchFromRecruitment")]
        public async Task<IActionResult> RemoveBranchFromRecruitment(int recruitmentId, int branchId)
        {
            var result = await _companyService.RemoveBranchFromRecruitment(recruitmentId, branchId);
            return Ok(result);
        }

        [HttpPost("RemoveCareerFromRecruitment")]
        public async Task<IActionResult> RemoveCareerFromRecruitment(int recruitmentId, int careerId)
        {
            var result = await _companyService.RemoveCareerFromRecruitment(recruitmentId, careerId);
            return Ok(result);
        }
        [HttpPost("CreateNewRecruiment")]
        public async Task<IActionResult> CreateNewRecruiment([FromBody] RecruitmentCreateRequest request)
        {
            var result = await _companyService.CreateNewRecruitment(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPost("Comment")]
        public async Task<IActionResult> Comment([FromBody] CommentRequest request)
        {
            var result = await _companyService.Comment(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpPost("ExtendRecruitment")]
        public async Task<IActionResult> ExtendRecruitment([FromBody] ExtendRecruitmentRequest request)
        {
            var result = await _companyService.ExtendRecruitment(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPost("Chat")]
        public async Task<IActionResult> Chat([FromBody] ChatRequest request)
        {
            var result = await _companyService.Chat(request);
            return Ok(result);
        }

        [HttpPut("UpdateCompanyName")]
        public async Task<IActionResult> UpdateCompanyName([FromBody] CompanyNameUpdateRequest request)
        {


            var result = await _companyService.UpdateCompanyName(request.Id, request.Name);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPut("UpdateCompanyDescription")]
        public async Task<IActionResult> UpdateCompanyDescription([FromBody] CompanyDescriptionUpdateRequest request)
        {


            var result = await _companyService.UpdateCompanyDescription(request.Id, request.Description);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPut("UpdateCompanyWorkerNumber")]
        public async Task<IActionResult> UpdateCompanyWorkerNumber([FromBody] CompanyWorkerNumberUpdateRequest request)
        {


            var result = await _companyService.UpdateCompanyWorkerNumber(request.Id, request.WorkerNumber);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPut("UpdateCompanyContactName")]
        public async Task<IActionResult> UpdateCompanyContactName([FromBody] CompanyContactNameUpdateRequest request)
        {


            var result = await _companyService.UpdateCompanyContactName(request.Id, request.ContactName);
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


            var result = await _companyService.UpdateCoverImage(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("UpdateRecruitmentName")]
        public async Task<IActionResult> UpdateRecruitmentName([FromBody] UpdateRecruitmentNameRequest request)
        {
            var result = await _companyService.UpdateRecruitmentName(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPut("UpdateRecruitmentSalary")]
        public async Task<IActionResult> UpdateRecruitmentSalary([FromBody] UpdateSalaryRecruitment request)
        {
            var result = await _companyService.UpdateRecruitmentSalary(request);
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

        [HttpDelete("AcceptCV")]
        public async Task<IActionResult> AcceptCV(int recruitmentId, Guid userId)
        {
            var result = await _companyService.AcceptCV(recruitmentId, userId);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpDelete("RefuseCV")]
        public async Task<IActionResult> RefuseCV(int recruitmentId, Guid userId)
        {
            var result = await _companyService.RefuseCV(recruitmentId, userId);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpDelete("DeleteRecruitment")]
        public async Task<IActionResult> DeleteRecruitment(int id)
        {
            var result = await _companyService.DeleteRecruitment(id);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
