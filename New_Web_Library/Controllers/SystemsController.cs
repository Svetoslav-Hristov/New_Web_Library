using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using New_Web_Library.Data;
using New_Web_Library.Data.Models;
using New_Web_Library.GCommon.Enums;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels.System;

namespace New_Web_Library.Controllers
{
    using static New_Web_Library.GCommon.EntityValidations.UsersBooks;
    public class SystemsController : Controller
    {
      
        private readonly ISystemsService _systemsService;

        public SystemsController( ISystemsService systemsService)
        {
           
            this._systemsService = systemsService;
        }


        [HttpGet]
        public async Task<IActionResult> Register(string? search)
        {


            IEnumerable<RegisterModelView> currentRecords = await _systemsService.AllUserWhoHaveActiveLoanOrReservationAsync(search);

            return View(currentRecords);

        }


        [HttpGet]

        public async Task<IActionResult> CreateLoan()
        {

            CreateLoanView model = await _systemsService.CreateNewLoanAsync();


            return View(model);
        }


        [HttpPost]

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
        public async Task<IActionResult> CreateReservation(Guid bookId)
        {


            var model = await _systemsService.CreateNewReservationAsync(bookId);

            if (!model.Success)
            {
                TempData["ErrorMessage"] = model.ErrorMessage;

                return RedirectToAction("Index", "Books");

            }



            return View(model.Data);

        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation(CreateReserveModel model)
        {
            if (!ModelState.IsValid)
            {

                await _systemsService.RestoreReservationModelAsync(model);

                return View("CreateReservation", model);

            }


            var reservation = await _systemsService.ConfirmNewReservationAsync(model);



            if (!reservation.Success)
            {
                await _systemsService.RestoreReservationModelAsync(model);

                ModelState.AddModelError(nameof(model.SearchingCriteria), reservation.ErrorMessage);

                return View("CreateReservation", model);



            }



            TempData["SuccessReservation"] = "You have successfully reserved the book you selected.";


            return RedirectToAction("Details", "Books", new { Id = model.BookId });


        }

        [HttpGet]
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


            TempData["ConfirmOrEdit"] = "You have successfully changed or created your Loan.";


            return RedirectToAction(nameof(Register))
;
        }

        [HttpPost]
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

        public async Task<IActionResult> SearchByCriteria(CreateReserveModel model)
        {

            var result = await _systemsService.FindUserByCriteriaAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError(nameof(model.SearchingCriteria), result.ErrorMessage);

            }


            return View("CreateReservation", result.Data);


        }




    }
}
