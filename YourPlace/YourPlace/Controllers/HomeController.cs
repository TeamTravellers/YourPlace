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
        private const string bgTest = "~/Views/Bulgarian/BgTest.cshtml"; 
        private const string ImagesPath = "/Images/ProductImages";

        public HomeController(ILogger<HomeController> logger, HotelsServices hotelsServices)
        {
            _logger = logger;
            _hotelsServices = hotelsServices;
        }

        //returns main page view
        public IActionResult Index()
        {
            return View();
        }

        //returns privacy view
        public IActionResult Privacy()
        {
            return View();
        }

        //returns english view - main page
        public IActionResult English()
        {
            return View(english);
        }

        //returns bulgarian view - main page
        public IActionResult Bulgarian()
        {
            return View(bulgarian);
        }

        //returns bulgarian index page - all offers
        public async Task<IActionResult> ToMainBg()
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
        
        //bg test view
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