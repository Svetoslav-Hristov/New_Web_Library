using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using New_Web_Library.Service.Core.Interfaces;

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
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Index(string? search)
        {
            var result = await _authorService.GetAllAuthorsAsync(search);

            if (!result.Success)
            {
                TempData["ErrorAuthor"] = result.ErrorMessage;
               
            }

            return View(result.Data);


        }
    }
}
