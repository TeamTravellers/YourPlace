using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Diagnostics;
using YourPlace.Core.Services;
using YourPlace.Models;

namespace YourPlace.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HotelsServices _hotelsServices;

        private const string homePage = "Index";
        private const string english = "~/Views/English/Index.cshtml";
        private const string bulgarian = "~/Views/Bulgarian/Index.cshtml";
        private const string toMainBg = "~/Views/Bulgarian/MainPage.cshtml";
        private const string bgTest = "~/Views/Bulgarian/BgTest.cshtml"; //bg test, figure out how to make it for both languages
        private const string ImagesPath = "/Images/ProductImages";

        public HomeController(ILogger<HomeController> logger, HotelsServices hotelsServices)
        {
            _logger = logger;
            _hotelsServices = hotelsServices;
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

        public async Task<IActionResult> ToMainBgAsync()
        {
            try
            {
                var hotels = await _hotelsServices.ReadAllAsync();
                return View(toMainBg, new AllHotelsModel { Hotels = hotels });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return View("Error");
            }
        }

        public IActionResult Test()
        {
            return View(bgTest);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}