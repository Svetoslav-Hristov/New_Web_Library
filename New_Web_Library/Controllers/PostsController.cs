using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using New_Web_Library.Service.Core.Interfaces;
using New_Web_Library.ViewModels.Forum;
using System.Security.Claims;
using static New_Web_Library.GCommon.EntityValidations.Topics;

namespace New_Web_Library.Controllers
{
    public class PostsController : Controller
    {

        private readonly IPostService _postsService;
        private readonly ILogger<PostsController> _logger;

        public PostsController(IPostService postsService, ILogger<PostsController> logger)
        {
            this._postsService = postsService;
            this._logger = logger;

        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> PostPreview(int Id, int pageNumber = 1, int pageSize = 4)
        {
            Guid? userId = null;

            if (User.Identity.IsAuthenticated && User.Identity.IsAuthenticated)
            {
                userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }


            var result = await _postsService.PostDetailModelsPreview(Id, userId, pageNumber, pageSize);

            if (!result.Success)
            {

                _logger.LogError("Post not found: Id = {PostId}", Id);

                return NotFound();
            }

            ViewBag.Page = pageNumber;

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

                _logger.LogWarning("{0} Id = {1}", model.ErrorMessage, Id);

                return NotFound();
            }

            return View(model.Data);


        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
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
                if (result.ErrorMessage.Contains("Unexpected"))
                {
                    _logger.LogError(result.ErrorMessage);

                    return StatusCode(500);
                }


                _logger.LogWarning(result.ErrorMessage);

                return NotFound();
            }

            if (result.Data.Topic.Title == TopicSpecialName)
            {
                TempData["SuccessMessage"] = "Your message has been successfully sent to the administrator.";

                return RedirectToAction("Index", "Categories");

            }



            TempData["SuccessPost"] = "You have successfully created a new post.";


            return RedirectToAction(nameof(PostPreview), new { id = result.Data.Id });

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditPost(int Id)
        {

            var result = await _postsService.EditPost(Id);

            if (!result.Success)
            {
                _logger.LogWarning("{0} Id = {1}", result.ErrorMessage, Id);

                return NotFound();


            }

            return View("CreatePost", result.Data);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
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
                if (result.ErrorMessage.Contains("not found!"))
                {
                    _logger.LogWarning(result.ErrorMessage);

                    return NotFound();
                }
                else if (result.ErrorMessage.Contains("Unexpected"))
                {
                    _logger.LogError(result.ErrorMessage);

                    return StatusCode(500);
                }


                TempData["ErrorEditPost"] = result.ErrorMessage;

                return RedirectToAction(nameof(PostPreview), new { id = topicId });

            }

            return RedirectToAction(nameof(PostPreview), new { id = Id });

        }


        [HttpPost]
        [Authorize(Roles ="Admin,User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int Id)
        {


            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);



            var result = await _postsService.SoftDeletePost(Id, userId);

            if (!result.Success)
            {

                if (result.ErrorMessage.Contains("not found!"))
                {
                    _logger.LogWarning(result.ErrorMessage);

                    return NotFound();

                }
                else if (result.ErrorMessage.Contains("Unexpected"))
                {
                    _logger.LogError(result.ErrorMessage);

                    return StatusCode(500);
                }

                TempData["ErrorDeletePost"] = result.ErrorMessage;

                return RedirectToAction(nameof(PostPreview), new { id = Id });

            }

            if (result.Data.Title == TopicSpecialName)
            {
                return RedirectToAction("UsersComplaints", "Systems");
            }



            return RedirectToAction("SubCategory", "Topics", new { id = result.Data.Id });


        }



    }
}
