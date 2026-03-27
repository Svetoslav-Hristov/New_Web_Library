using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels.Book;

namespace New_Web_Library.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class BooksController : Controller
    {
        private readonly IBooksService _bookService;
        private readonly ILogger<BooksController> _logger;
        public BooksController(IBooksService bookService, ILogger<BooksController> logger)
        {
            this._bookService = bookService;
            this._logger = logger;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {

            ServiceResult<BookFormModel> model = await _bookService.GetEmptyModelBookFormWithLoadedTypesAsync();

            if (!model.Success)
            {
                TempData["ErrorBook"] = "An unexpected error occurred, please try again later.";

                return RedirectToAction("Index", "Books", new { area = "" });

            }


            return View(model.Data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookFormModel formModel)
        {

            if (!ModelState.IsValid)
            {



                await _bookService.BookModelDataFillingAsync(formModel);

                return View(nameof(Create), formModel);


            }


            if (string.IsNullOrEmpty(formModel.NewAuthor) && string.IsNullOrEmpty(formModel.SelectedAuthor))
            {
                ModelState.AddModelError(nameof(formModel.NewAuthor), "Or add an author.");

                ModelState.AddModelError(nameof(formModel.SelectedAuthor), "Please select and add an author.");

                await _bookService.BookModelDataFillingAsync(formModel);

                return View(nameof(Create), formModel);

            }


            var result = await _bookService.CreateNewBookUsingBookFormModelAsync(formModel);


            if (!result.Success)
            {
                TempData["ErrorBook"] = result.ErrorMessage;

                return RedirectToAction("Index", "Books", new { area = "" });
            }

            TempData["SuccessMessage"] = "Book created successfully.";

            return RedirectToAction("Details", "Books", new { area = "", Id = result.Data!.Id });
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var model = await _bookService.EditBookUsingBookFormModelAsync(Id);

            if (!model.Success)
            {
                TempData["ErrorEdit"] = model.ErrorMessage;

                return RedirectToAction("Details", "Books", new { area = "", Id });
            }


            return View(model.Data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] Guid Id, BookFormModel model)
        {


            if (!ModelState.IsValid)
            {

                await _bookService.BookModelDataFillingAsync(model);

                return View(nameof(Edit), model);

            }

            if (string.IsNullOrEmpty(model.NewAuthor) && string.IsNullOrEmpty(model.SelectedAuthor))
            {
                ModelState.AddModelError(nameof(model.SelectedAuthor), "Please select or add an author.");

                ModelState.AddModelError(nameof(model.NewAuthor), "Or add an author.");

                await _bookService.BookModelDataFillingAsync(model);

                return View(nameof(Edit), model);

            }


            var result = await _bookService.ConfirmEditChangesUsingBookFormModelAsync(Id, model);

            if (!result.Success)
            {

                TempData["ErrorEdit"] = result.ErrorMessage;

                return RedirectToAction("Details", "Books", new { area = "", Id });
            }



            TempData["SuccessEdit"] = "You have successfully edited your book.";

            return RedirectToAction("Details", "Books", new { area = "", id = result.Data.Id });
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid Id)
        {

            var result = await _bookService.DeleteCurrentBookAsync(Id);


            if (!result.Success)
            {
                TempData["Error"] = result.ErrorMessage;

                return RedirectToAction(nameof(Details), new { Id });
            }
            else
            {
                TempData["SuccessDelete"] = "You have successfully deleted the book";
            }


            return RedirectToAction("Index", "Books", new { area = "" });
        }

    }
}
