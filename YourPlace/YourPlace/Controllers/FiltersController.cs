using Microsoft.AspNetCore.Mvc;
using YourPlace.Core.Sorting;
using YourPlace.Infrastructure.Data.Entities;


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
        public async IActionResult FilterByCountry(string country)
        {
            List<Hotel> hotels = await _filters.FilterByCountry(country);
            return View(toCountryFilter, new )
        }
    }
}
