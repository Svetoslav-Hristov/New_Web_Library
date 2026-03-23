using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Logging;
using New_Library.Data.Models.Forum;
using New_Web_Library.Data;
using New_Web_Library.Service.Core.Interfaces;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.ViewModels.Forum;
using System.Security.Claims;

namespace New_Web_Library.Controllers
{
    public class TopicsController : Controller
    {
        private readonly ITopicService _topicService;
        private readonly ILogger<TopicsController> _logger;
        public TopicsController(ITopicService topicService,ILogger<TopicsController> logger)
        {
            this._topicService = topicService;
            this._logger = logger;
        }




        [HttpGet]
        public async Task<IActionResult> SubCategory(int Id)
        {
            var result = await _topicService.SubCategoryIndexPreview(Id);

            if (!result.Success)
            {
               
                _logger.LogWarning("{0}: Id = {1} " ,result.ErrorMessage , Id);

                return NotFound();

            }


            return View(result.Data);

        }


        [HttpGet]
        public async Task<IActionResult> CreateSubCategory(int Id)
        {
            var result = await _topicService.CreateNewSubCategory(Id);

            if (!result.Success)
            {
                TempData["ErrorSubCategory"] = result.ErrorMessage;
                return RedirectToAction("Categories");
            }

            return View(result.Data);
        }


        [HttpPost]
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
                return View(model);
            }

            TempData["SuccessSubCategory"] = "Successfully created a new subcategory";

            return RedirectToAction("Index", "Categories");




        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditSubCategory(int Id)
        {
            var result = await _topicService.EditSubCategory(Id);

            if (!result.Success)
            {
                TempData["ErrorEditSubCategory"] = result.ErrorMessage;

                return RedirectToAction("Index", "Categories");
            }

            return View("CreateSubCategory", result.Data);

        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> EditSubCategory(CreateSubCategoryViewModel model, [FromRoute] int Id)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _topicService.ConfirmEditSubCategory(model,Id);

            if (!result.Success)
            {
                TempData["ErrorEditSubCategory"] = result.ErrorMessage;

                return RedirectToAction("Index", "Categories");
            }


            return RedirectToAction(nameof(SubCategory), new { id = Id });
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSubCategory(int Id)
        {
            var result = await _topicService.SoftDeleteSubCategory(Id);

            if (!result.Success)
            {
                TempData["ErrorDeleteSubCategory"] = result.ErrorMessage;

                return RedirectToAction("Index", "Categories");
            }

            return RedirectToAction("Index", "Categories");

        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> HardDelete (int Id)
        {
            var result = await _topicService.HardDeleteSubCategory(Id);

            if (!result.Success)
            {
                TempData["ErrorHardDeleteSubCategory"] = result.ErrorMessage;

                return RedirectToAction("ForumSupportPreview", "Systems");
            }

            return RedirectToAction("ForumSupportPreview", "Systems");

        }


        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> RestoreSubCategory (int Id)
        {
            var result = await _topicService.RestoreSubCategory(Id);

            if (!result.Success)
            {
                TempData["ErrorRestoreSubCategory"] = result.ErrorMessage;

                return RedirectToAction("ForumSupportPreview", "Systems");
            }

            return RedirectToAction("ForumSupportPreview", "Systems");
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateUserComplaint()
        {
            int subCategoryId = -1;

            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            
               ServiceResult<Topic> result = await _topicService.GetOrCreateSpecialSubCategory(userId);

                if (!result.Success)
                {

                    _logger.LogWarning(result.ErrorMessage);


                }
                else
                {
                    subCategoryId = result.Data.Id;
                }

            return RedirectToAction("CreatePost", "Posts", new { id= subCategoryId});

        }


    }


}

