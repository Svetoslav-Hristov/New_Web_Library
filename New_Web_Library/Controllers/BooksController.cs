using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using New_Web_Library.GCommon.Enums;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels.Book;


namespace New_Web_Library.Controllers
{
    public class BooksController : Controller
    {


        private readonly IBookService _bookService;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBookService bookService, ILogger<BooksController> logger)
        {
            this._bookService = bookService;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string? search, Genre? genre, int page = 1)
        {

            int pagesize = 4;

            BookPagingPreview books = await _bookService
                .GetAllBooksOrderedByTitleThanByAuthorAscAsync(search, genre, page, pagesize);



            return View(books);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid Id)
        {

            var newBook = await _bookService.GetCurrentModelAsync(Id);

            if (!newBook.Success)
            {

                TempData["ErrorBook"] = newBook.ErrorMessage;

                return RedirectToAction(nameof(Index));
            }


            return View(newBook.Data);

        }



    }
}
