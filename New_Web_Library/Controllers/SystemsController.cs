using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using New_Web_Library.Service.Core.Interfaces;
using New_Web_Library.Services.Core.Interfaces;
using System.Security.Claims;

namespace New_Web_Library.Controllers
{
    public class SystemsController : Controller
    {
      
        private readonly ISystemService _systemsService;
        private readonly ITopicService _topicService;
        private readonly ILogger<SystemsController> _logger;

        public SystemsController( ISystemService systemsService ,ITopicService topicService,ILogger<SystemsController> logger)
        {
           
            this._systemsService = systemsService;
            this._topicService = topicService;
            this._logger = logger;
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReservation(Guid bookId)
        {

            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var reservation = await _systemsService.CreateNewReservationAsync(bookId, userId);



            if (!reservation.Success)
            {

                TempData["ErrorReservation"] = reservation.ErrorMessage;

               

                return RedirectToAction("Index", "Books");

            }



            TempData["SuccessReservation"] = "You have successfully reserved the book you selected.";


            return RedirectToAction("Details", "Books", new { Id = reservation.Data});


        }

    }
}
