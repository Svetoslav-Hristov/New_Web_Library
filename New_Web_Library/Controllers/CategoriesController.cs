using Microsoft.AspNetCore.Mvc;
using New_Web_Library.Service.Core;
using New_Web_Library.Service.Core.Interfaces;
using New_Web_Library.ViewModels.Forum;

namespace New_Web_Library.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }



        [HttpGet]
        public async Task<IActionResult> Index()
        {

            IEnumerable<IndexForumModel> category = await _categoryService.IndexPreview();


            return View(category);
        }


        [HttpGet]
        //[Authorize(Roles ="Admin")]
        public IActionResult CreateCategory()
        {
            var result = _categoryService.CreateNewCategory();

            if (!result.Success)
            {
                return Redirect(nameof(Index));
            }


            return View(result.Data);

        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryFormModel model)
        {
            if (!ModelState.IsValid)
            {

                return View(model);

            }

            var result = await _categoryService.ConfirmNewCategory(model);

            if (!result.Success)
            {
                TempData["ErrorCategory"] = result.ErrorMessage;

                return RedirectToAction(nameof(Index));

            }


            TempData["SuccessCategory"] = "Тhe new category was created successfully.";

            return RedirectToAction(nameof(Index));

        }
    }
}
