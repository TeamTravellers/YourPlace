using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using YourPlace.Models;

namespace YourPlace.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private const string homePage = "Index";
        private const string english = "~/Views/English/Index.cshtml";
        private const string bulgarian = "~/Views/Bulgarian/Index.cshtml";
        private const string ImagesPath = "/Images/ProductImages";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult English()
        {
            return View(english);
        }

        public IActionResult Bulgarian()
        {
            return View(bulgarian);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}