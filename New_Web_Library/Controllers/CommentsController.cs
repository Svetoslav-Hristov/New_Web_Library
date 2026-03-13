using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public CommentsController(ICommentsService commentsService)
        {
            this._commentsService = commentsService;
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


            return RedirectToAction(nameof(Post), new { Id });


        }


        [HttpGet]
        [Authorize]

        public async Task<IActionResult> EditComment(int Id)
        {

            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _commentsService.EditComment(Id, userId);

            if (!result.Success)
            {
                TempData["ErrorEdit"] = result.ErrorMessage;

                return RedirectToAction(nameof(Index));

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
                TempData["ErrorEdit"] = result.ErrorMessage;
                return RedirectToAction(nameof(Post), new { Id });
            }

            TempData["SuccessEditComment"] = "You have successfully edited your comment.";


            return RedirectToAction(nameof(Post), new { Id = result.Data.PostId });


        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int Id,int postId)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _commentsService.SoftDeleteComment(Id,postId ,userId);

            if (!result.Success)
            {

                return RedirectToAction(nameof(Index));


            }

            return RedirectToAction(nameof(Post), new { result.Data.Id });

        }
    }
}
