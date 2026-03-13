using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using New_Web_Library.Data;
using New_Web_Library.Service.Core.Interfaces;
using New_Web_Library.ViewModels.Forum;
using System.Security.Claims;

namespace New_Web_Library.Controllers
{
    public class TopicsController : Controller
    {
        private readonly ITopicService _topicService;
        public TopicsController(ITopicService topicService)
        {
            this._topicService = topicService;
        }

       


        [HttpGet]
        public async Task<IActionResult> SubCategory(int Id)
        {
            var result = await _topicService.SubCategoryIndexPreview(Id);

            if (!result.Success)
            {
                TempData["WrongData"] = result.ErrorMessage;

                return RedirectToAction(nameof(Index));
            }


            return View(result.Data);

        }

       

        

       

    }
}
