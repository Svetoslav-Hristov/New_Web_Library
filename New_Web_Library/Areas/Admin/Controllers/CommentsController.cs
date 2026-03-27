using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using New_Web_Library.Service.Core.Interfaces;

namespace New_Web_Library.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommentsController : Controller
    {
        private readonly ICommentsService _commentsService;
        private readonly ILogger<CommentsController> _logger;

        public CommentsController(ICommentsService commentsService ,ILogger<CommentsController> logger)
        {
            this._commentsService = commentsService;
            this._logger = logger;
            
        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
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
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
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
