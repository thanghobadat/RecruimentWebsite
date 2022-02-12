using Microsoft.AspNetCore.Mvc;
using MVCApp.Service;
using MVCApp.ViewModels;
using System;
using System.Threading.Tasks;
using ViewModel.Exceptions;

namespace MVCApp.Controllers
{
    public class CompanyController : BaseController
    {
        private readonly ICompanyApiClient _companyApiClient;
        public CompanyController(ICompanyApiClient companyApiClient)
        {
            _companyApiClient = companyApiClient;
        }
        public async Task<IActionResult> Index(Guid Id)
        {
            if (Id == null)
            {
                throw new RecruimentWebsiteException("id is null, pleae try again");
            }
            var companyInfor = await _companyApiClient.GetCompanyInformation(Id);
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
        public async Task<IActionResult> UpdateCompanyInformation(Guid Id)
        {
            if (Id == null)
            {
                throw new RecruimentWebsiteException("id is null, pleae try again");
            }
            var information = await _companyApiClient.GetCompanyInformation(Id);
            if (information.IsSuccessed)
            {
                var updateRequest = new CompanyInformationUpdateRequest()
                {
                    Name = information.ResultObj.Name,
                    Description = information.ResultObj.Description,
                    WorkerNumber = information.ResultObj.WorkerNumber,
                    ContactName = information.ResultObj.ContactName,
                    Id = Id,
                    PhoneNumber = information.ResultObj.PhoneNumber,
                    Email = information.ResultObj.Email,

                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCompanyInformation(CompanyInformationUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _companyApiClient.UpdateCompanyInformation(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Update successfull";
                return RedirectToAction("Index", new { Id = request.Id });
            }
            throw new RecruimentWebsiteException(result.Message);
        }
    }
}
