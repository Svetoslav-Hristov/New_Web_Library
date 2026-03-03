using Microsoft.AspNetCore.Mvc;

namespace New_Web_Library.Controllers
{
    public class ForumController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
