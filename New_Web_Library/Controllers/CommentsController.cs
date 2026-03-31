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
        private readonly ICommentService _commentsService;
        private readonly ILogger<CommentsController> _logger;

        public CommentsController(ICommentService commentsService, ILogger<CommentsController> logger)
        {
            this._commentsService = commentsService;
            this._logger = logger;
        }




        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateComment(int Id)
        {
            if(Id <= 0)
            {
                return NotFound();
            }

            var result = await _commentsService.CreateNewComment(Id);

            if (!result.Success)
            {

                TempData["ErrorComment"] = result.ErrorMessage;

                return RedirectToAction("Index","Categories");

            }

            return View(result.Data);

        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment(CreateContentViewModel model, [FromRoute]int Id)
        {
            if(Id <= 0)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);



            var result = await _commentsService.ConfirmNewComment(model, Id, userId);

            if (!result.Success)
            {
                TempData["ErrorComment"] = result.ErrorMessage;

                

            }
            else
            {
                TempData["SuccessComment"] = "You have successfully commented on this post.";

            }



                return RedirectToAction("PostPreview", "Posts", new { id = Id });


        }


        [HttpGet]
        [Authorize]

        public async Task<IActionResult> EditComment(int Id)
        {

            if(Id <= 0)
            {
                return NotFound();
            }

            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _commentsService.EditComment(Id, userId);

            if (!result.Success)
            {
                
                TempData["ErrorComment"] = result.ErrorMessage;

                return RedirectToAction("Index","Categories");

            }

            return View(result.Data);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(CreateContentViewModel model, int Id)
        {
            if(Id <= 0)
            {
                return NotFound();
            }
            
            if (!ModelState.IsValid)
            {
                return View(model);

            }

            var result = await _commentsService.ConfirmEditComment(model, Id);

            if (!result.Success)
            {

                TempData["ErrorComment"] = result.ErrorMessage;


            }
            else
            {
                TempData["SuccessEditComment"] = "You have successfully edited your comment.";

            }
            
            return RedirectToAction("PostPreview", "Posts", new { id = result.Data.PostId });


        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int Id, int postId)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _commentsService.SoftDeleteComment(Id, postId, userId);

            if (!result.Success)
            {
               

                TempData["ErrorComment"] = result.ErrorMessage;

            }
            else
            {
                TempData["SuccessComment"] = "You have successfully deleted your comment.";
            }

                return RedirectToAction("PostPreview", "Posts", new { Id = postId });

        }

       

    }
}
