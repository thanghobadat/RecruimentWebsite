using Application.Catalog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

        [HttpPut("UpdateUserAvatar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateUserAvatar([FromForm] int id, IFormFile thumnailImage)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.UpdateUserAvatar(id, thumnailImage);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
