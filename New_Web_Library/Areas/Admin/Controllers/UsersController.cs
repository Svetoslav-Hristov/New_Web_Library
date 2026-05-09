using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using New_Web_Library.Data.Models;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels.User;
using System.Security.Claims;

namespace New_Web_Library.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
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

            var usersCollection = await _usersService.GetAllUsersWithOrWithoutSearchCriteriaAsync(search, page, pageSize);


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
            if (Id == Guid.Empty)
            {
                return NotFound();
            }

            var result = await _usersService.ChangeUserStatusAsync(Id);


            if (!result.Success)
            {

                TempData["ErrorUser"] = result.ErrorMessage;



            }
            else
            {

                TempData["SuccessUser"] = "User status has been changed successfully.";

            }


            return RedirectToAction(nameof(Details),new {id=Id });

        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Edit(Guid Id)
        {

            if (Id == Guid.Empty)
            {
                return NotFound();
            }
            
            Guid changerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _usersService.EditUserProfileAsync(Id, changerId);

            if (!result.Success)
            {
                TempData["ErrorUser"] = result.ErrorMessage;

                return RedirectToAction(nameof(Index));

            }

            return View(result.Data);

        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Edit(UserFormModel model ,[FromRoute]Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Guid changerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _usersService.ConfirmEditUserProfileAsync(model, Id, changerId);

            if (!result.Success)
            {
                TempData["ErrorUser"] = result.ErrorMessage;

            }
            else
            {
                TempData["SuccessUser"] = "You have successfully changed the user profile";
            }

            return RedirectToAction(nameof(Details), new { id = Id });

        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return NotFound();
            }

            var model = await _usersService.GetAllUserDetailsAsync(Id);

            if (!model.Success)
            {
                TempData["ErrorUser"] = model.ErrorMessage;

                return RedirectToAction(nameof(Index));
            }


            return View(model.Data);

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return NotFound();
            }

            var result = await _usersService.DeleteUserProfileAsync(Id);

            if (!result.Success)
            {
                TempData["ErrorUser"] = result.ErrorMessage;

            }
            else
            {
                TempData["SuccessUser"] = "You have successfully deleted the user.";
            }


            return RedirectToAction(nameof(Index));

        }





    }

}
