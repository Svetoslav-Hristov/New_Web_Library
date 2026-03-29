using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using New_Web_Library.Service.Core.Interfaces;
using System.Security.Claims;

namespace New_Web_Library.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostsController : Controller
    {
        private readonly IPostService _postsService;
        private readonly ILogger<PostsController> _logger;

        public PostsController(IPostService postsService ,ILogger<PostsController> logger)
        {
            this._postsService = postsService;
            this._logger = logger;
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestorePost(int Id)
        {
            var result = await _postsService.RestoreDeletePost(Id);

            if (!result.Success)
            {
                TempData["ErrorRestorePost"] = result.ErrorMessage;

                return RedirectToAction("ForumSupportPreview", "Systems");
            }

            return RedirectToAction("ForumSupportPreview", "Systems");

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HardDeletePost(int Id)
        {
            var result = await _postsService.HardDeletePost(Id);

            if (!result.Success)
            {
                TempData["ErrorHardDeletePost"] = result.ErrorMessage;

                return RedirectToAction("ForumSupportPreview", "Systems");
            }

            return RedirectToAction("ForumSupportPreview", "Systems");

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserComplaint(int Id)
        {
            var post = await _postsService.GetUserComplaint(Id);

            if (!post.Success)
            {
                _logger.LogWarning(post.ErrorMessage);

                return RedirectToAction("ForumSupportPreview", "Systems");

            }


            return View(post.Data);


        }


    }
}
