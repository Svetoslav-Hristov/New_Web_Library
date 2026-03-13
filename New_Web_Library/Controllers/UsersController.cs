using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using New_Web_Library.Data;
using New_Web_Library.Data.Models;
using New_Web_Library.GCommon.Enums;
using New_Web_Library.Services.Core;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels.User;
using System.Security.Claims;

namespace Web_Library.Controllers
{
    public class UsersController : Controller
    {
       
        private readonly IUsersService _usersService;
        private readonly SignInManager<User> _signInManager;

        public UsersController( IUsersService usersService, SignInManager<User> signInManager)
        {
           
            this._usersService = usersService;
            this._signInManager = signInManager;
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
