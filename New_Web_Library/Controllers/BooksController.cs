using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using New_Web_Library.Data;
using New_Web_Library.Data.Models;
using New_Web_Library.GCommon.Enums;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels.Book;


namespace New_Web_Library.Controllers
{
    public class BooksController : Controller
    {
        
        
        private readonly IBooksService _bookService;

        public BooksController(  IBooksService bookService)
        {
          
         
            this._bookService = bookService;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string? search, Genre? genre)
        {

            IEnumerable<FullPreviewModelBook> books = await _bookService
                .GetAllBooksOrderedByTitleThanByAuthorAscAsync(search, genre);

            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {

            var newBook = await _bookService.GetCurrentModelAsync(Id);

            if (!newBook.Success)
            {
                TempData["WrongBook"] = newBook.ErrorMessage;

                return RedirectToAction(nameof(Index));
                
            }


            return View(newBook.Data);

        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {

            ServiceResult<BookFormModel> model = await _bookService.GetEmptyModelBookFormWithLoadedTypesAsync();

            if (!model.Success)
            {
                TempData["ErrorBook"] = "An unexpected error occurred, please try again later.";

                return RedirectToAction(nameof(Index));

            }


            return View(model.Data);
        }

        [HttpPost]
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

                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Book created successfully.";

            return RedirectToAction(nameof(Details), new { Id = result.Data!.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var model = await _bookService.EditBookUsingBookFormModelAsync(Id);

            if (!model.Success)
            {
                TempData["ErrorEdit"] = model.ErrorMessage;

                return RedirectToAction(nameof(Details), new { Id });
            }


            return View(model.Data);
        }

        [HttpPost]
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

                return RedirectToAction(nameof(Details), new { Id });

            }



            TempData["SuccessEdit"] = "You have successfully edited your book.";

            return RedirectToAction(nameof(Details), new { id = result.Data.Id });
        }

        [HttpPost]
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

            
            return RedirectToAction(nameof(Index));
        }





    }
}
