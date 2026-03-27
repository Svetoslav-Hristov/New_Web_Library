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
        public TopicsController(ITopicService topicService, ILogger<TopicsController> logger)
        {
            this._topicService = topicService;
            this._logger = logger;
        }




        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SubCategory(int Id)
        {
            var result = await _topicService.SubCategoryIndexPreview(Id);

            if (!result.Success)
            {

                _logger.LogWarning("{0}: Id = {1} ", result.ErrorMessage, Id);

                return NotFound();

            }


            return View(result.Data);

        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateUserComplaint(int Id)
        {
            int subCategoryId = -1;

            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);


            ServiceResult<Topic> result = await _topicService.GetOrCreateSpecialSubCategory(userId);

            if (!result.Success)
            {

                _logger.LogWarning(result.ErrorMessage);

                TempData["Unexpected"] = result.ErrorMessage;

                return RedirectToAction("PostPreview", "Posts", new { id = Id });

            }
           
                subCategoryId = result.Data.Id;
            

            return RedirectToAction("CreatePost", "Posts", new { id = subCategoryId });

        }


    }


}

