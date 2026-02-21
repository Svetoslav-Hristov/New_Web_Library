using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using New_Web_Library.Data;
using New_Web_Library.Data.Models;
using New_Web_Library.GCommon.Enums;
using New_Web_Library.Services.Core;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels.User;

namespace Web_Library.Controllers
{
    public class UsersController : Controller
    {
       
        private readonly IUsersService _usersService;

        public UsersController( IUsersService usersService)
        {
           
            this._usersService = usersService;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string? search)
        {

            var usersCollection = await _usersService.GetAllUsersWithOrWithoutSearchCriteriaAsync(search);


            if (!usersCollection.Success)
            {
                TempData["EmptyCollection"] = usersCollection.ErrorMessage;

                return View(usersCollection.Data);

            }



            return View(usersCollection.Data);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(Guid Id)
        {

            var result = await _usersService.ChangeUserStatusAsync(Id);


            if (!result.Success)
            {

                TempData["ErrorStatus"] = result.ErrorMessage;

                return RedirectToAction(nameof(Index));

            }



            TempData["SuccessStatus"] = "User status has been changed successfully.";


            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {

            var model = _usersService.CreateNewUserUsingFormModel();


            return View(model.Data);
        }


        [HttpPost]
        public async Task<IActionResult> Create(UserFormModel model)
        {

            if (!ModelState.IsValid)
            {

                return View(model);

            }


            var newUser = await _usersService.ConfirmRegistrationNewUserAsync(model);



            if (!newUser.Success)
            {
                ModelState.AddModelError("", newUser.ErrorMessage);

                return View(newUser.Data);

            }



            TempData["SuccessRegistration"] = "The user was successfully registered.";

            return RedirectToAction("Index", "Books");

        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {

            var model = await _usersService.GetAllUserDetailsAsync(Id);

            if (!model.Success)
            {
                TempData["MissingUser"] = model.ErrorMessage;

                return RedirectToAction(nameof(Index));
            }
            

            return View(model.Data);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var formModel = await _usersService.EditUserRegistrationAsync(Id);

            if (!formModel.Success)
            {
                TempData["ErrorEdit"] = formModel.ErrorMessage;

                return RedirectToAction(nameof(Index));
            }


            return View(formModel.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] Guid Id, UserFormModel formModel)
        {

            if (!ModelState.IsValid)
            {

                return View(formModel);
            }


            var model = await _usersService.ConfirmEditChangesAsync(Id, formModel);


            if (!model.Success)
            {

                ModelState.AddModelError("", model.ErrorMessage);

                return View(model.Data);

            }



            TempData["SuccessMessage"] = "New changes saved successfully";


            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var result = await _usersService.DeleteUserProfileAsync(Id);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;

                return RedirectToAction(nameof(Index));

            }


            TempData["SuccessDelete"] = "You have successfully deleted the user.";

            return RedirectToAction(nameof(Index));

        }
    }
}
