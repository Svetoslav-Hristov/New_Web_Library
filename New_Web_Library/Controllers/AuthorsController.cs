using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using New_Web_Library.Service.Core.Interfaces;
using System.Threading.Tasks;

namespace New_Web_Library.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IAuthorService _authorService;
        public AuthorsController(IAuthorService authorService)
        {
            this._authorService = authorService;
        }



        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult>Index(string? search)
        {

            var result = await _authorService.GetAllAuthorsAsync(search);

            if (!result.Success)
            {
                TempData["ErrorAuthor"] = result.ErrorMessage;

            }

            return View(result.Data);


        }




        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return NotFound();
            }

            var result = await _authorService.GetAllDetailsAuthorAsync(Id);

            if (!result.Success)
            {
                TempData["ErrorAuthor"] = result.ErrorMessage;

                return RedirectToAction(nameof(Index));
               
            }

            return View(result.Data);
           
        }
    }
}
