using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using New_Web_Library.Data;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels;
using New_Web_Library.ViewModels.Book;

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
    }
}
