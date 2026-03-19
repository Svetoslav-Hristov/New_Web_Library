using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using New_Library.Data.Models.Forum;
using New_Web_Library.Service.Core;
using New_Web_Library.Service.Core.Interfaces;
using New_Web_Library.ViewModels.Forum;
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
        [AllowAnonymous]
        public async Task<IActionResult> PostPreview(int Id)
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

                return RedirectToAction("Index", "Categories");
            }



            return View(result.Data);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreatePost(int Id)
        {
            var model = await _postsService.CreateNewPost(Id);



            if (!model.Success)
            {
                TempData["ErrorPost"] = model.ErrorMessage;

                return RedirectToAction("SubCategories", "Topics", new { Id });
            }

            return View(model.Data);


        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePost(CreateContentViewModel model, int Id)
        {
            if (!ModelState.IsValid)
            {

                return View(model);
            }

            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _postsService.ConfirmNewPost(model, userId, Id);

            if (!result.Success)
            {
                TempData["ErrorPost"] = result.ErrorMessage;

                return RedirectToAction("SubCategories", "Topics", new { Id });
            }


            TempData["SuccessPost"] = "You have successfully created a new post.";


            return RedirectToAction(nameof(PostPreview), new { id = result.Data });

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditPost(int Id)
        {


            var result = await _postsService.EditPost(Id);

            if (!result.Success)
            {
                TempData["ErrorEditPost"] = result.ErrorMessage;

                return RedirectToAction(nameof(PostPreview), new { id = Id });
            }

            return View("CreatePost", result.Data);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditPost(CreateContentViewModel model, [FromRoute] int Id, [FromRoute] int topicId)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _postsService.ConfirmEditPost(model, userId, Id);

            if (!result.Success)
            {
                TempData["ErrorEditPost"] = result.ErrorMessage;

                return RedirectToAction(nameof(PostPreview), new { id = topicId });

            }

            return RedirectToAction(nameof(PostPreview), new { id = Id });




        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeletePost(int Id)
        {


            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _postsService.SoftDeletePost(Id, userId);

            if (!result.Success)
            {
                TempData["ErrorDeletePost"] = result.ErrorMessage;

                return RedirectToAction(nameof(PostPreview), new { id = Id });
            }

            return RedirectToAction("SubCategory", "Topics", new { id = result.Data });


        }

        [HttpPost]
        [Authorize]
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
        [Authorize]
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


    }
}
