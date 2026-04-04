using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels;
using New_Web_Library.ViewModels.Book;
using System.Diagnostics;

namespace New_Web_Library.Controllers
{
    public class WelcomeController : Controller
    {
        private readonly ILogger<WelcomeController> _logger;
        private readonly IWelcomeService _welcomeService;


        public WelcomeController(ILogger<WelcomeController> logger, IWelcomeService welcomeService)

        {
            _logger = logger;
            this._welcomeService = welcomeService;

        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Contacts()
        {
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> EnterPreview()
        {
            IEnumerable<PreviewBookModel> bookCollection = await _welcomeService.GetLatestTitlesPreviewAsync();

            return View(bookCollection);

        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Error404()
        {
            Response.StatusCode = 404;

            return View();
        }

        public IActionResult Error500()
        {
            Response.StatusCode = 500;

            return View();
        }

    }
}
