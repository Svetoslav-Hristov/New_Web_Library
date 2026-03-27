using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Logging;
using New_Web_Library.Service.Core.Interfaces;
using New_Web_Library.ViewModels.Forum;

namespace New_Web_Library.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryService categoryService ,ILogger<CategoriesController> logger)
        {
            this._categoryService = categoryService;
            this._logger = logger;
            
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditCategory(int Id)
        {
            var result = await _categoryService.EditCategory(Id);

            if (!result.Success)
            {
                TempData["ErrorCategoryEdit"] = result.ErrorMessage;

                return RedirectToAction(nameof(Index));
            }

            return View("CreateCategory", result.Data);

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(CategoryFormModel model, [FromRoute] int Id)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _categoryService.ConfirmEditCategory(model, Id);

            if (!result.Success)
            {
                TempData["ErrorCategoryEdit"] = result.ErrorMessage;

                return RedirectToAction(nameof(Index));

            }



            return RedirectToAction(nameof(Index));

        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int Id)
        {
            var result = await _categoryService.SoftDeleteCategory(Id);

            if (!result.Success)
            {
                TempData["ErrorDeleteCategory"] = result.ErrorMessage;
            }


            return RedirectToAction(nameof(Index));


        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HardDelete(int Id)
        {
            var result = await _categoryService.HardDeleteCategory(Id);

            if (!result.Success)
            {
                TempData["ErrorHardDeleteCategory"] = result.ErrorMessage;

                return RedirectToAction("ForumSupportPreview", "Systems");
            }

            return RedirectToAction("ForumSupportPreview", "Systems");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreCategory(int Id)
        {
            var result = await _categoryService.RestoreSoftDeleteCategory(Id);

            if (!result.Success)
            {
                TempData["ErrorRestoreCategory"] = result.ErrorMessage;

                return RedirectToAction("ForumSupportPreview", "Systems");

            }

            return RedirectToAction("ForumSupportPreview", "Systems");

        }




    }
}
