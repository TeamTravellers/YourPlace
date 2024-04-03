using Microsoft.AspNetCore.Mvc;
using YourPlace.Core.Services;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Models.ManagerModels;
using YourPlace.Models.Managers_Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using YourPlace.Infrastructure.Data.Enums;
namespace YourPlace.Controllers
{
    public class ManagerMenuController : Controller
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly HotelsServices _hotelsServices;
        private readonly RoomServices _roomServices;
        private readonly HotelCategoriesServices _hotelCategoriesServices;
        public ManagerMenuController(IWebHostEnvironment webHost, HotelsServices hotelsServices, RoomServices roomServices, HotelCategoriesServices hotelCategoriesServices)
        {
            _webHost = webHost;
            _hotelsServices = hotelsServices;
            _roomServices = roomServices;
            _hotelCategoriesServices = hotelCategoriesServices;
        }

        private const string toManagerStartPage = "~/Views/Bulgarian/ManagerViews/StartPage.cshtml";
        private const string toAddHotelPage = "~/Views/Bulgarian/ManagerViews/AddHotel.cshtml";
        private const string toManagerHotels = "~/Views/Bulgarian/ManagerViews/ManagerHotels.cshtml";
        private const string toUploadImage = "~/Views/Bulgarian/ManagerViews/UploadImage.cshtml";

        public IActionResult Index(string managerID, string firstName, string lastName)
        {
            return View(toManagerStartPage, new HotelCreateModel { ManagerID = managerID, FirstName = firstName, LastName = lastName});
        }
        public IActionResult ReturnToStart([Bind("ManagerID, FirstName, LastName")] string managerID, string firstName, string lastName)
        {
            return View(toManagerStartPage, new HotelCreateModel { ManagerID = managerID, FirstName = firstName, LastName = lastName });
        }

        public IActionResult AddHotel([Bind("ManagerID", "FirstName", "LastName")] string managerID, string firstName, string lastName)
        {
            return View(toAddHotelPage, new HotelCreateModel { ManagerID = managerID, FirstName = firstName, LastName = lastName });
        }
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHotel([Bind("ManagerID, HotelName, Address, Town, Country, Rating, Details, RoomsInHotel, Location, Tourism, Atmosphere, Company, Pricing")] string managerID, string hotelName, string address, string town, string country, double rating, string details, List<Room> roomsInHotel, IFormFile imgfile, Location location, Tourism tourism, Atmosphere atmosphere, Company company, Pricing pricing)
        {
            
            var saveimg = Path.Combine(_webHost.WebRootPath, "Images/MainImages", imgfile.FileName);
            string imgext = Path.GetExtension(imgfile.FileName);
            string imageUrl = "";
            if (imgext == ".jpg" || imgext == ".png")
            {
                using (var uploadimg = new FileStream(saveimg, FileMode.Create))
                {
                    await imgfile.CopyToAsync(uploadimg);
                }

                imageUrl = imgfile.FileName; 
                Console.WriteLine(imageUrl);
            }
            else
            {
                ViewData["Message"] = "Само файлове с разширение .jpg & .png са позволени ...";
                
            }

            Hotel hotel = new Hotel(imageUrl, hotelName, address, town, country, rating, details, managerID);
            List<Hotel> hotels = new List<Hotel>();
            hotels.Add(hotel);
            await _hotelsServices.CreateAsync(hotel);
            
            foreach(var room in roomsInHotel)
            {
                if(room.CountInHotel != 0)
                {
                    await _roomServices.CreateAsync(room);
                    Console.WriteLine(room.Type + " " + room.CountInHotel + " " + room.Price);
                }
            }
            Categories categories = new Categories(location, tourism, atmosphere, company, pricing, hotel.HotelID);
            await _hotelCategoriesServices.CreateAsync(categories);

            Console.WriteLine(location.ToString());
            Console.WriteLine(tourism.ToString());
            Console.WriteLine(atmosphere.ToString());
            Console.WriteLine(company.ToString());
            Console.WriteLine(pricing.ToString());
       
            return View(toManagerHotels, new HotelCreateModel { ManagerHotels = hotels });
        }
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile imgfile)
        {
            var saveimg = Path.Combine(_webHost.WebRootPath, "Images/MainImages", imgfile.FileName);
            string imgext = Path.GetExtension(imgfile.FileName);
            if (imgext == ".jpg" || imgext == ".png")
            {
                using (var uploadimg = new FileStream(saveimg, FileMode.Create))
                {
                    await imgfile.CopyToAsync(uploadimg);
                    ViewData["Message"] = "The Selected File " + imgfile.FileName + " Is Saved Successfully ..!";
                }

                string imageUrl = imgfile.FileName; // Assuming Images folder is directly under wwwroot
                Console.WriteLine(imageUrl);
                return View(toAddHotelPage, new HotelCreateModel { MainImageURL = imageUrl });
            }
            else
            {
                ViewData["Message"] = "Only the image file .jpg & .png are allowed ...";
                return View(toAddHotelPage, new HotelCreateModel()); // Assuming you still want to return the view even if the file type is not supported.
            }
           
        }

    }
}
