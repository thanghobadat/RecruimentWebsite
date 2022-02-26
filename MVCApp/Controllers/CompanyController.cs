using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCApp.Service;
using MVCApp.ViewModels;
using System;
using System.Threading.Tasks;
using ViewModel.Exceptions;

namespace MVCApp.Controllers
{
    [Authorize]
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
        public IActionResult CreateBranch(Guid Id)
        {
            ViewBag.CompanyId = Id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBranch(CompanyBranchCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _companyApiClient.CreateBranch(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Create Branch successfull";
                return RedirectToAction("Index", new { Id = request.CompanyId });

            }
            ModelState.AddModelError("", result.Message);


            return View(request);
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
                TempData["result"] = "Update Information successfull";
                return RedirectToAction("Index", new { Id = request.Id });
            }
            throw new RecruimentWebsiteException(result.Message);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateBranch(int id, Guid companyId)
        {
            if (id <= 0)
            {
                throw new RecruimentWebsiteException("id must be greater than 0");
            }

            if (companyId == null)
            {
                throw new RecruimentWebsiteException("id is null, pleae try again");
            }
            ViewBag.companyId = companyId;
            var branch = await _companyApiClient.GetCompanyBranchById(id);
            if (branch.IsSuccessed)
            {

                var updateRequest = new CompanyBranchUpdateRequest()
                {
                    Id = id,
                    Address = branch.ResultObj.Address,

                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBranch(CompanyBranchUpdateRequest request, Guid companyId)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _companyApiClient.UpdateBranch(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Update Branch successfull";
                return RedirectToAction("Index", new { Id = companyId });
            }
            throw new RecruimentWebsiteException(result.Message);
        }



        public async Task<IActionResult> DeleteBranch(int id, Guid companyId)
        {
            if (id <= 0)
            {
                throw new RecruimentWebsiteException("id must be greater than 0");
            }

            var result = await _companyApiClient.DeleteBranch(id);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Delete Branch Successfull";
                return RedirectToAction("Index", new { Id = companyId });
            }
            throw new RecruimentWebsiteException(result.Message);
        }

        [HttpGet]
        public IActionResult UpdateCompanyAvatar(int id, Guid companyId)
        {
            if (id <= 0)
            {
                throw new RecruimentWebsiteException("id must be greater than 0");
            }

            if (companyId == null)
            {
                throw new RecruimentWebsiteException("id is null, pleae try again");
            }
            ViewBag.companyId = companyId;
            var updateRequest = new CompanyAvatarUpdateRequest()
            {
                Id = id,
                ThumnailImage = null,

            };
            return View(updateRequest);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCompanyAvatar(CompanyAvatarUpdateRequest request, Guid companyId)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _companyApiClient.UpdateCompanyAvatar(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Update Company Avatar successfull";
                return RedirectToAction("Index", new { Id = companyId });
            }
            throw new RecruimentWebsiteException(result.Message);
        }

        [HttpGet]
        public IActionResult CreateCompanyCoverImage(Guid Id)
        {
            ViewBag.CompanyId = Id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompanyCoverImage(CompanyCoverImageCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _companyApiClient.CreateCompanyCoverImage(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Create Cover Image successfull";
                return RedirectToAction("Index", new { Id = request.CompanyId });

            }
            ModelState.AddModelError("", result.Message);


            return View(request);
        }


        [HttpGet]
        public IActionResult UpdateCompanyCoverImage(int id, Guid companyId)
        {
            if (id <= 0)
            {
                throw new RecruimentWebsiteException("id must be greater than 0");
            }

            if (companyId == null)
            {
                throw new RecruimentWebsiteException("id is null, pleae try again");
            }
            ViewBag.companyId = companyId;
            var updateRequest = new CompanyCoverImageUpdateRequest()
            {
                Id = id,
                ThumnailImage = null,

            };
            return View(updateRequest);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCompanyCoverImage(CompanyCoverImageUpdateRequest request, Guid companyId)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _companyApiClient.UpdateCompanyCoverImage(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Update Company Avatar successfull";
                return RedirectToAction("Index", new { Id = companyId });
            }
            throw new RecruimentWebsiteException(result.Message);
        }

        public async Task<IActionResult> DeleteCompanyCoverImage(int id, Guid companyId)
        {
            if (id <= 0)
            {
                throw new RecruimentWebsiteException("id must be greater than 0");
            }

            var result = await _companyApiClient.DeleteCompanyCoverImage(id);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Delete Cover Image Successfull";
                return RedirectToAction("Index", new { Id = companyId });
            }
            throw new RecruimentWebsiteException(result.Message);
        }


        [HttpGet]
        public IActionResult CreateCompanyImages(Guid Id)
        {
            ViewBag.CompanyId = Id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompanyImages(CompanyImagesCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _companyApiClient.CreateCompanyImages(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Create Images successfull";
                return RedirectToAction("Index", new { Id = request.CompanyId });

            }
            ModelState.AddModelError("", result.Message);


            return View(request);
        }

        public async Task<IActionResult> DeleteCompanyImages(int id, Guid companyId)
        {
            if (id <= 0)
            {
                throw new RecruimentWebsiteException("id must be greater than 0");
            }

            var result = await _companyApiClient.DeleteCompanyImages(id);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Delete Image Successfull";
                return RedirectToAction("Index", new { Id = companyId });
            }
            throw new RecruimentWebsiteException(result.Message);
        }

    }
}
