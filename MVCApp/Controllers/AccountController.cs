using Microsoft.AspNetCore.Mvc;
using MVCApp.Service;
using MVCApp.ViewModels;
using System;
using System.Threading.Tasks;
using ViewModel.Exceptions;
using ViewModel.System.Users;

namespace MVCApp.Controllers
{
    public class AccountController : BaseController
    {

        private readonly IAccountApiClient _accountApiClient;
        public AccountController(IAccountApiClient accountApiClient)
        {
            _accountApiClient = accountApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 1)
        {
            var request = new GetAccountPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var accountCompany = await _accountApiClient.GetCompanyAccountPagings(request);
            var accountUser = await _accountApiClient.GetUserAccountPagings(request);

            var accountVM = new AccountsViewModel()
            {
                CompanyAccount = accountCompany.ResultObj,
                UserAccount = accountUser.ResultObj
            };
            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }

            return View(accountVM);
        }


        [HttpGet]
        public IActionResult ChangePassword(Guid id)
        {
            var changePassRequest = new ChangePasswordRequest()
            {
                Id = id,
                Password = null,
                ConfirmPassword = null
            };
            return View(changePassRequest);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            if (request.Password != request.ConfirmPassword)
            {
                ModelState.AddModelError("", "Password confirm is not match");
                return View(request);
            }

            var result = await _accountApiClient.ChangePassword(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Change password successfull";
                return RedirectToAction("Index");
            }
            throw new RecruimentWebsiteException(result.Message);
        }

        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            if (id == null)
            {
                throw new RecruimentWebsiteException("id is null, pleae try again");
            }


            var result = await _accountApiClient.DeleteAccount(id);
            if (result.IsSuccessed)
            {
                TempData["resultImage"] = "Delete successfull";
                return RedirectToAction("Index");
            }
            throw new RecruimentWebsiteException(result.Message);
        }
    }
}
