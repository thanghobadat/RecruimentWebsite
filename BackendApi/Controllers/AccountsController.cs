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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultToken = await _accountService.Authenticate(request);


            if (string.IsNullOrEmpty(resultToken))
            {
                return BadRequest(resultToken);
            }
            return Ok(resultToken);
        }

        [HttpPost("RegisterAdmin")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminAccountRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _accountService.RegisterAdminAccount(request);
            if (!result.ResultObj)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("RegisterUser")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserAccountRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _accountService.RegisterUserAccount(request);
            if (!result.ResultObj)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("RegisterCompany")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterCompany([FromBody] RegisterCompanyAccountRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _accountService.RegisterCompanyAccount(request);
            if (!result.ResultObj)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetCompanyAccount")]
        public async Task<IActionResult> GetCompanyAccountPaging([FromQuery] GetAccountPagingRequest request)
        {
            var result = await _accountService.GetCompanyAccountPaging(request);
            return Ok(result);
        }
        [HttpGet("GetUserAccount")]
        public async Task<IActionResult> GetUserAccountPaging([FromQuery] GetAccountPagingRequest request)
        {
            var products = await _accountService.GetUserAccountPaging(request);
            return Ok(products);
        }
        [HttpGet("GetUserAccountById")]
        public async Task<IActionResult> GetUserAccountById(Guid id)
        {
            var user = await _accountService.GetUserById(id);
            return Ok(user);
        }
        [HttpGet("GetCompanyAccountById")]
        public async Task<IActionResult> GetCompanyAccountById(Guid id)
        {
            var company = await _accountService.GetCompanyById(id);
            return Ok(company);
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(Guid id, string newPassword)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _accountService.ChangePassword(id, newPassword);
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
