using Microsoft.AspNetCore.Mvc;
using New_Web_Library.Service.Core.Interfaces;
using System.Security.Claims;

namespace New_Web_Library.Controllers
{
    public class PostsController : Controller
    {

        private readonly IPostsService _postsService;

        public PostsController(IPostsService postsService)
        {
            this._postsService = postsService;
        }


        [HttpGet]
        public async Task<IActionResult> Post(int Id)
        {
            Guid? userId = null;

            if (User.Identity.IsAuthenticated && User.Identity.IsAuthenticated)
            {
                userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }


            var result = await _postsService.PostDetailModelsPreview(Id, userId);

            if (!result.Success)
            {
                TempData["ErrorPost"] = result.ErrorMessage;

                return RedirectToAction(nameof(TopicsController), new { Id });
            }



            return View(result.Data);
        }


    }
}
