using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using New_Web_Library.Data.Models;
using New_Web_Library.Services.Core.Interfaces;

namespace New_Web_Library.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class UsersController : Controller
    {
        private readonly IUserService _usersService;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService usersService, SignInManager<User> signInManager,
            ILogger<UsersController> logger)
        {
            this._usersService = usersService;
            this._signInManager = signInManager;
            this._logger = logger;

            
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string? search, int page = 1)
        {

            int pageSize = 10;

            var usersCollection = await _usersService.GetAllUsersWithOrWithoutSearchCriteriaAsync(search , page , pageSize);


            if (!usersCollection.Success)
            {
                TempData["EmptyCollection"] = usersCollection.ErrorMessage;

                return View(usersCollection.Data);

            }



            return View(usersCollection.Data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
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
