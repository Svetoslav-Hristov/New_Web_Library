using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels.System;
using System.Security.Claims;

namespace New_Web_Library.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SystemsController : Controller
    {

        private readonly ISystemService _systemsService;
        private readonly ILogger<SystemsController> _logger;

        public SystemsController(ISystemService systemsService, ILogger<SystemsController> logger)
        {
            this._systemsService = systemsService;
            this._logger = logger;

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register(string? search)
        {
            IEnumerable<RegisterModelView> currentRecords = await _systemsService.AllUserWhoHaveActiveLoanOrReservationAsync(search);

            return View(currentRecords);

        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateLoan()
        {

            CreateLoanView model = await _systemsService.CreateNewLoanAsync();


            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLoan(CreateLoanView model)
        {
            if (!ModelState.IsValid)
            {

                TempData["ErrorMessage"] = "Invalid data provided.";

                return RedirectToAction(nameof(Register));

            }


            var result = await _systemsService.ConfirmNewLoanAsync(model);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;

                return RedirectToAction(nameof(Register));
            }


            TempData["SuccessMessage"] = "Loan created successfully.";

            return RedirectToAction(nameof(Register));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditLoan(int Id)
        {

            var model = await _systemsService.EditCurrentLoanModelAsync(Id);

            if (!model.Success)
            {
                TempData["Unchanged"] = model.ErrorMessage;

                return RedirectToAction(nameof(Register));
            }



            return View(model.Data)
;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLoan([FromRoute] int Id, CreateLoanView model)
        {


            if (!ModelState.IsValid)
            {

                return View("EditLoan", model);

            }

            var editRecord = await _systemsService.ConfirmEditLoanModelAsync(Id, model);


            if (!editRecord.Success)
            {
                TempData["Unchanged"] = editRecord.ErrorMessage;


                return RedirectToAction(nameof(Register));
            }


            TempData["ConfirmOrEdit"] = "You have successfully modified or created the loan.";


            return RedirectToAction(nameof(Register))
;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLoan(int Id)
        {

            var deleteResult = await _systemsService.DeleteLoanAsync(Id);

            if (!deleteResult.Success)
            {

                TempData["DeleteError"] = deleteResult.ErrorMessage;

            }
            else
            {

                TempData["SuccessDelete"] = "The Loan record was deleted successfully.";

            }


            return RedirectToAction(nameof(Register))
;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchByCriteria(CreateReserveModel model)
        {

            var result = await _systemsService.FindUserByCriteriaAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError(nameof(model.SearchingCriteria), result.ErrorMessage);

            }


            return View("CreateReservation", result.Data);


        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ForumSupportPreview()
        {
            var result = await _systemsService.GetAllDeleteItems();


            return View(result);

        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UsersComplaints()
        {
            var result = await _systemsService.GetSpecialArea();

            if (!result.Success)
            {
                _logger.LogWarning(result.ErrorMessage);

                TempData["ErrorUserComplaints"] = result.ErrorMessage;

                return RedirectToAction(nameof(ForumSupportPreview));

            }


            return View(result.Data);
        }




    }
}
