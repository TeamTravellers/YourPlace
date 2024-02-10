using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Diagnostics.Metrics;
using YourPlace.Core.Sorting;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Models;

namespace YourPlace.Controllers
{
    public class FiltersController : Controller
    {
        private readonly Filters _filters;
        public FiltersController(Filters filters)
        {
            _filters = filters;
        }

        private const string toCountryFilter = "Views/Bulgarian/Hotels/CountryFilter";
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Filter(string country, int count, decimal price, DateOnly arrivingDate, DateOnly leavingDate)
        {
            List<Hotel> hotels = new List<Hotel>();
            if (country != null)
            {
                hotels = await _filters.FilterByCountry(country);
            }
            if (count != 0)
            {
                hotels = await _filters.FilterByPeopleCount(count);
            }
            if (price != 0)
            {
                hotels = await _filters.FilterByPrice(price);
            }
            if (arrivingDate != null && leavingDate != null)
            {
                hotels = await _filters.FilterByDates(arrivingDate, leavingDate);
            }
            return View(toCountryFilter, new AllHotelsModel { Hotels = hotels });
        }
        

    }
}
