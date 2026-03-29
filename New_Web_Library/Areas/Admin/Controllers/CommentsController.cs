using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using New_Web_Library.Service.Core.Interfaces;

namespace New_Web_Library.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentsService;
        private readonly ILogger<CommentsController> _logger;

        public CommentsController(ICommentService commentsService, ILogger<CommentsController> logger)
        {
            this._commentsService = commentsService;
            this._logger = logger;

        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreComment(int Id)
        {
            if (Id <= 0)
            {
                return NotFound();
            }

            var result = await _commentsService.RestoreDeleteComment(Id);

            if (!result.Success)
            {
                TempData["ErrorComment"] = result.ErrorMessage;


            }
            else
            {
                TempData["SuccessComment"] = "Successfully restored the deleted comment";

            }

            return RedirectToAction("ForumSupportPreview", "Systems");

        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HardDeleteComment(int Id)
        {
            if (Id <= 0)
            {
                return NotFound();
            }

            var result = await _commentsService.HardDeleteComment(Id);

            if (!result.Success)
            {
                TempData["ErrorComment"] = result.ErrorMessage;

            }
            else
            {
                TempData["SuccessComment"] = "Successfully permanently deleted the comment";

            }

            return RedirectToAction("ForumSupportPreview", "Systems");

        }


    }
}
