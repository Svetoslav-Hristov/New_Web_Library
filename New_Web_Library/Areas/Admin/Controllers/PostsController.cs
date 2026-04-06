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
            if(Id <= 0)
            {
                return NotFound();
            }

            var result = await _postsService.RestoreDeletePost(Id);

            if (!result.Success)
            {
                TempData["ErrorRestorePost"] = result.ErrorMessage;

                
            }
            else
            {
                TempData["SuccessPost"]= "Successfully restored the deleted post";
            }

            return RedirectToAction("ForumSupportPreview", "Systems");

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HardDeletePost(int Id)
        {
            if(Id <= 0)
            {
                return NotFound();
            }

            var result = await _postsService.HardDeletePost(Id);

            if (!result.Success)
            {
                TempData["ErrorPost"] = result.ErrorMessage;

                
            }
            else
            {
                TempData["SuccessPost"] = "Post has been deleted permanently.";
            }

            return RedirectToAction("ForumSupportPreview", "Systems");

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserComplaint(int Id)
        {
            if(Id <= 0)
            {
                return NotFound();
            }

            var post = await _postsService.GetUserComplaint(Id);

            if (!post.Success)
            {
                TempData["ErrorComplaints"] = post.ErrorMessage;

                return RedirectToAction("UsersComplaints", "Systems");

            }


            return View(post.Data);


        }


    }
}
