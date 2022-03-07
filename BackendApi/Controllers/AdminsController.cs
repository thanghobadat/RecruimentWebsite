using Application.Catalog;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ViewModel.Catalog.Admin;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AdminsController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminsController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpGet("GetBranchPaging")]
        public async Task<IActionResult> GetBranchPaging([FromQuery] GetBranchPagingRequest request)
        {
            var branches = await _adminService.GetAllBranchPaging(request);
            return Ok(branches);
        }
        [HttpGet("GetCareerPaging")]
        public async Task<IActionResult> GetCareerPaging([FromQuery] GetCareerPagingRequest request)
        {
            var careers = await _adminService.GetAllCareerPaging(request);
            return Ok(careers);
        }


        [HttpGet("GetBranchById")]
        public async Task<IActionResult> GetBranchById(int id)
        {
            var branch = await _adminService.GetBranchById(id);
            return Ok(branch);
        }

        [HttpGet("GetCareerById")]
        public async Task<IActionResult> GetCareerById(int id)
        {
            var career = await _adminService.GetCareerById(id);
            return Ok(career);
        }

        [HttpPost("CreateBranch")]
        public async Task<IActionResult> CreateBranch([FromBody] BranchViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _adminService.CreateBranch(request);
            return Ok(result);
        }
        [HttpPost("CreateCareer")]
        public async Task<IActionResult> CreateCareer([FromBody] CareerViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _adminService.CreateCareer(request);
            return Ok(result);
        }

        [HttpPut("UpdateBranch")]
        public async Task<IActionResult> UpdateBranch(BranchViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _adminService.UpdateBranch(request);
            return Ok(result);
        }

        [HttpPut("UpdateCareer")]
        public async Task<IActionResult> UpdateCareer(CareerViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _adminService.UpdateCareer(request);
            return Ok(result);
        }
        [HttpDelete("DeleteBranch")]
        public async Task<IActionResult> DeleteBranch(int id)
        {

            var result = await _adminService.DeleteBranch(id);
            return Ok(result);
        }
        [HttpDelete("DeleteCareer")]
        public async Task<IActionResult> DeleteCareer(int id)
        {

            var result = await _adminService.DeleteCareer(id);
            return Ok(result);
        }
    }
}
