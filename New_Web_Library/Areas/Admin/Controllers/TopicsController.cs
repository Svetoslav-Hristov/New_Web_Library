using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using New_Library.Data.Models.Forum;
using New_Web_Library.Service.Core.Interfaces;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.ViewModels.Forum;
using System.Security.Claims;

namespace New_Web_Library.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class TopicsController : Controller
    {
        private readonly ITopicService _topicService;
        private readonly ILogger<TopicsController> _logger;

        public TopicsController(ITopicService topicService ,ILogger<TopicsController> logger)
        {
            this._topicService = topicService;
            this._logger = logger;
            
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSubCategory(int Id)
        {
            if(Id <= 0)
            {
                return NotFound();
            }

            var result = await _topicService.CreateNewSubCategory(Id);

            if (!result.Success)
            {
                TempData["ErrorSubCategory"] = result.ErrorMessage;
                return RedirectToAction("Categories");
            }

            return View(result.Data);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubCategory(CreateSubCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _topicService.ConfirmCreationNewSubcategory(model, userId);

            if (!result.Success)
            {
                TempData["ErrorSubCategory"] = result.ErrorMessage;

                return RedirectToAction("Index", "Categories", new { area=""} );
            }

           

            TempData["SuccessSubCategory"] = "Successfully created a new subcategory";

            return RedirectToAction("SubCategory","Topics",new {id=result.Data.Id ,area = ""});




        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        
        public async Task<IActionResult> EditSubCategory(int Id)
        {
            if(Id <= 0)
            {
                return NotFound();
            }

            var result = await _topicService.EditSubCategory(Id);

            if (!result.Success)
            {
                TempData["ErrorSubCategory"] = result.ErrorMessage;

                return RedirectToAction("Index", "Categories" ,new {area=""} );
            }

            return View("CreateSubCategory", result.Data);

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> EditSubCategory(CreateSubCategoryViewModel model, [FromRoute] int Id)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _topicService.ConfirmEditSubCategory(model, Id,userId);

            if (!result.Success)
            {
                TempData["ErrorSubCategory"] = result.ErrorMessage;

                return RedirectToAction("Index", "Categories",new {area=""});
            }

            

            TempData["SuccessSubCategory"] = "Successfully edited the subcategory";

            return RedirectToAction("SubCategory", "Topics", new { id = Id ,area="" });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSubCategory(int Id)
        {
            if(Id <= 0)
            {
                return NotFound();
            }

            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _topicService.SoftDeleteSubCategory(Id,userId);

            if (!result.Success)
            {
                TempData["ErrorSubCategory"] = result.ErrorMessage;

                
            }
            else
            {
                TempData["SuccessSubCategory"] = "Successfully deleted the subcategory";
            }
            
            return RedirectToAction("Index", "Categories",new {area=""});

        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HardDelete(int Id)
        {
            if(Id <= 0)
            {
                return NotFound();
            }

            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _topicService.HardDeleteSubCategory(Id,userId);

            if (!result.Success)
            {
                TempData["ErrorSubCategory"] = result.ErrorMessage;

               
            }
            else
            {
                TempData["SuccessSubCategory"] = "Successfully deleted the subcategory";
            }

                return RedirectToAction("ForumSupportPreview", "Systems");

        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreSubCategory(int Id)
        {
            if(Id <= 0)
            {
                return NotFound();
            }

            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


            var result = await _topicService.RestoreSubCategory(Id,userId);

            if (!result.Success)
            {
                TempData["ErrorSubCategory"] = result.ErrorMessage;

               
            }

            TempData["SuccessSubCategory"] = "Successfully restored the subcategory";


            return RedirectToAction("ForumSupportPreview", "Systems");
        }






    }
}
