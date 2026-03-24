using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using New_Library.Data.Models.Forum;
using New_Web_Library.Service.Core;
using New_Web_Library.Service.Core.Interfaces;
using New_Web_Library.ViewModels.Forum;
using System.Security.Claims;

namespace New_Web_Library.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentsService _commentsService;
        private readonly ILogger<CommentsController> _logger;

        public CommentsController(ICommentsService commentsService, ILogger<CommentsController> logger)
        {
            this._commentsService = commentsService;
            this._logger = logger;
        }




        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateComment(int Id)
        {
            var result = await _commentsService.CreateNewComment(Id);

            if (!result.Success)
            {

                TempData["ErrorComment"] = result.ErrorMessage;

                return RedirectToAction(nameof(Post), new { Id });

            }

            return View(result.Data);

        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateComment(CreateContentViewModel model, int Id)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);



            var result = await _commentsService.ConfirmNewComment(model, Id, userId);

            if (!result.Success)
            {
                TempData["ErrorComment"] = result.ErrorMessage;

                return RedirectToAction(nameof(Post), new { Id });
            }


            return RedirectToAction("PostPreview", "Posts", new { id = Id });


        }


        [HttpGet]
        [Authorize]

        public async Task<IActionResult> EditComment(int Id)
        {

            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _commentsService.EditComment(Id, userId);

            if (!result.Success)
            {
                _logger.LogWarning(result.ErrorMessage);

                TempData["ErrorEdit"] = result.ErrorMessage;

                return RedirectToAction("Index","Categories");

            }

            return View(result.Data);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditComment(CreateContentViewModel model, int Id)
        {
            if (!ModelState.IsValid)
            {
                return View(model);

            }

            var result = await _commentsService.ConfirmEditComment(model, Id);

            if (!result.Success)
            {
                _logger.LogWarning(result.ErrorMessage);

                TempData["ErrorEdit"] = result.ErrorMessage;

                return RedirectToAction("PostPreview", "Posts", new { id = result.Data.PostId });
            }

            TempData["SuccessEditComment"] = "You have successfully edited your comment.";


            return RedirectToAction("PostPreview", "Posts", new { id = result.Data.PostId });


        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int Id, int postId)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _commentsService.SoftDeleteComment(Id, postId, userId);

            if (!result.Success)
            {
                _logger.LogWarning(result.ErrorMessage);

                TempData["ErrorDeleteComment"] = result.ErrorMessage;

                return RedirectToAction("PostPreview", "Posts", new { Id = postId });


            }

            return RedirectToAction("PostPreview", "Posts", new { Id = postId });

        }

        [HttpPost]
        public async Task<IActionResult> RestoreComment(int Id)
        {
            var result = await _commentsService.RestoreDeleteComment(Id);

            if (!result.Success)
            {
                TempData["ErrorRestoreComment"] = result.ErrorMessage;

                return RedirectToAction("ForumSupportPreview", "Systems");
            }

            return RedirectToAction("ForumSupportPreview", "Systems");

        }

        [HttpPost]
        public async Task<IActionResult> HardDeleteComment(int Id)
        {
            var result = await _commentsService.HardDeleteComment(Id);

            if (!result.Success)
            {
                TempData["ErrorHardDeleteComment"] = result.ErrorMessage;

                return RedirectToAction("ForumSupportPreview", "Systems");
            }



            return RedirectToAction("ForumSupportPreview", "Systems");

        }


    }
}
