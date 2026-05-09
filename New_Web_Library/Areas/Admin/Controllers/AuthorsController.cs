using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using New_Web_Library.Service.Core.Interfaces;
using New_Web_Library.ViewModels.Author;
using System.Security.Claims;

namespace New_Web_Library.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorsController : Controller
    {
        private readonly IAuthorService _authorService;
        public AuthorsController(IAuthorService authorService)
        {
            this._authorService = authorService;

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string? search)
        {
            var result = await _authorService.GetAllAuthorsAsync(search);

            if (!result.Success)
            {
                TempData["ErrorAuthor"] = result.ErrorMessage;

            }

            return View(result.Data);


        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Create()
        {
            Guid creatorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _authorService.CreateNewAuthorProfileAsync(creatorId);

            if (!result.Success)
            {
                TempData["ErrorAuthor"] = result.ErrorMessage;

                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);

        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Create (AuthorDetailsForm model)
        {
            if (!ModelState.IsValid)
            {
                _authorService.AuthorModelImageFiling(model);

                return View(model);
            }

            Guid creatorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _authorService.ConfirmNewAuthorProfileAsync(model, creatorId);

            if (!result.Success)
            {
                TempData["ErrorAuthor"] = result.ErrorMessage;

                return RedirectToAction(nameof(Index));

            }

            TempData["SuccessAuthor"] = "Author profile creation successful.";

            return RedirectToAction("Details", "Authors", new { area = "", id = result.Data });


        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return NotFound();
            }

            Guid changerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _authorService.EditAuthorProfileAsync(Id, changerId);

            if (!result.Success)
            {
                TempData["ErrorAuthor"] = result.ErrorMessage;

                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);

        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AuthorDetailsForm model, Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _authorService.AuthorModelImageFiling(model);

                return View(model);

            }

            Guid changerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);


            var result = await _authorService.ConfirmEditAuthorProfileAsync(model, Id, changerId);

            if (!result.Success)
            {
                TempData["ErrorAuthor"] = result.ErrorMessage;


            }
            else
            {

                TempData["SuccessAuthor"] = "Author profile update successful.";

            }
            
            return RedirectToAction("Details", "Authors", new {area="", id=Id });


        }



        [HttpPost]
        [Authorize(Roles ="Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return NotFound();
            }

            Guid changerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _authorService.HardDeleteAuthorProfileAsync(Id, changerId);

            if (!result.Success)
            {
                TempData["ErrorAuthor"] = result.ErrorMessage;
            }
            else
            {
                TempData["SuccessAuthor"] = "Author profile delete successful.";
            }


            return RedirectToAction(nameof(Index));


        }

    }
}
