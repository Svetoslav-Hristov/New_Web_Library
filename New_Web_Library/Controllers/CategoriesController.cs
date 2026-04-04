using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {

            IEnumerable<IndexForumModel> category = await _categoryService.IndexPreview();


            return View(category);
        }


        
       
       
        

       



    }
}
