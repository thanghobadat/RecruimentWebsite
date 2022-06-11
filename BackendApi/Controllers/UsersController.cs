using Application.Catalog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ViewModel.Catalog.User;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;


        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetUserInformation")]
        public async Task<IActionResult> GetUserInformation(Guid userId)
        {
            var result = await _userService.GetUserInformation(userId);
            return Ok(result);
        }

        [HttpGet("GetUserAvatar")]
        public async Task<IActionResult> GetUserAvatar(Guid userId)
        {
            var result = await _userService.GetUserAvatar(userId);
            return Ok(result);
        }
        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetAllUser()
        {
            var result = await _userService.GetAllUser();
            return Ok(result);
        }

        [HttpPost("FollowCompany")]
        public async Task<IActionResult> FollowCompany(Guid userId, Guid companyId)
        {


            var result = await _userService.FollowCompany(userId, companyId);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpPost("SubmitCV")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SubmitCV([FromForm] SubmitCVRequest request)
        {
            var result = await _userService.SubmitCV(request);
            return Ok(result);
        }

        [HttpPut("UpdateUserInformation")]
        public async Task<IActionResult> UpdateUserInformation([FromBody] UserUpdateRequest request)
        {
            var result = await _userService.UpdateUserInformation(request);

            return Ok(result);
        }

        [HttpPut("ChangePasswordUser")]
        public async Task<IActionResult> ChangePasswordUser([FromBody] ChangePasswordUserRequest request)
        {
            var result = await _userService.ChangePasswordUser(request);

            return Ok(result);
        }

        [HttpPut("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var result = await _userService.ForgotPassword(request);

            return Ok(result);
        }


        [HttpPut("UpdateUserAvatar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateUserAvatar([FromForm] int id, IFormFile thumnailImage)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.UpdateUserAvatar(id, thumnailImage);
            return Ok(result);
        }
    }
}
