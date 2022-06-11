using Application.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ViewModel.System.Users;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost("Authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {

            var resultToken = await _accountService.Authenticate(request);



            return Ok(resultToken);
        }

        [HttpPost("RegisterAdmin")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminAccountRequest request)
        {

            var result = await _accountService.RegisterAdminAccount(request);

            return Ok(result);
        }

        [HttpPost("RegisterUser")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserAccountRequest request)
        {

            var result = await _accountService.RegisterUserAccount(request);

            return Ok(result);
        }

        [HttpPost("RegisterCompany")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterCompany([FromBody] RegisterCompanyAccountRequest request)
        {

            var result = await _accountService.RegisterCompanyAccount(request);

            return Ok(result);
        }

        [HttpGet("GetAllAccount")]
        public async Task<IActionResult> GetAllAccount()
        {
            var result = await _accountService.GetAllAccount();
            return Ok(result);
        }



        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _accountService.ChangePassword(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpDelete("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            var result = await _accountService.Delete(id);
            return Ok(result);
        }
    }
}
