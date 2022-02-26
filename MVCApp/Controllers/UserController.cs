using Microsoft.AspNetCore.Mvc;
using MVCApp.Service;
using MVCApp.ViewModels;
using System;
using System.Threading.Tasks;
using ViewModel.Exceptions;

namespace MVCApp.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        public UserController(IUserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }
        public async Task<IActionResult> Index(Guid Id)
        {
            if (Id == null)
            {
                throw new RecruimentWebsiteException("id is null, pleae try again");
            }
            var companyInfor = await _userApiClient.GetUserInformation(Id);

            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }

            if (companyInfor.IsSuccessed)
            {
                return View(companyInfor.ResultObj);

            }
            throw new RecruimentWebsiteException(companyInfor.Message);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUserInformation(Guid Id)
        {
            if (Id == null)
            {
                throw new RecruimentWebsiteException("id is null, pleae try again");
            }
            var information = await _userApiClient.GetUserInformation(Id);
            if (information.IsSuccessed)
            {
                var updateRequest = new UserInformationUpdateRequest()
                {
                    FirstName = information.ResultObj.FirstName,
                    LastName = information.ResultObj.LastName,
                    Age = information.ResultObj.Age,
                    AcademicLevel = information.ResultObj.AcademicLevel,
                    Address = information.ResultObj.Address,
                    Sex = information.ResultObj.Sex,
                    Id = Id,
                    PhoneNumber = information.ResultObj.PhoneNumber,
                    Email = information.ResultObj.Email,

                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserInformation(UserInformationUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.UpdateUserInformation(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Update Information successfull";
                return RedirectToAction("Index", new { Id = request.Id });
            }
            throw new RecruimentWebsiteException(result.Message);
        }


    }
}
