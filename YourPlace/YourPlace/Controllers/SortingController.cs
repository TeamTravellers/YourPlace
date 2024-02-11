using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Diagnostics.Metrics;
using YourPlace.Core.Services;
using YourPlace.Core.Sorting;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Models;

namespace YourPlace.Controllers
{
    public class SortingController : Controller
    {
        private readonly ILogger<SortingController> _logger;
        private readonly Filters _filters;
        private readonly PreferencesSorting _preferencesSorting;
        private readonly PreferencesServices _preferencesServices;
        public SortingController(ILogger<SortingController> logger, Filters filters, PreferencesSorting preferencesSorting, PreferencesServices preferencesServices)
        {
            _logger = logger;
            _filters = filters;
            _preferencesSorting = preferencesSorting;
            _preferencesServices = preferencesServices;
        }

        private const string toCountryFilter = "~/Views/Bulgarian/Hotels/CountryFilter.cshtml";
        private const string toSubmitPage = "~/Views/Bulgarian/Submit-page.cshtml";
        private const string toMainBgWithPreferences = "~/Views/Bulgarian/MainPage.cshtml";
        
        public IActionResult Index()
        {
            return View(toSubmitPage);
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
        public IActionResult ToSubmitPage()
        {
            return View(toSubmitPage);
        }
        public async Task<IActionResult> CreatePreferences(Preferences preference)
        {
            try
            {
                await _preferencesServices.CreateAsync(preference);
            }
            catch
            {
                return StatusCode(404, "Operation was not successful!");
            }
            RedirectToAction("PreferencesSorting");
            return View();
        }
        public async Task<IActionResult> PreferencesSorting(Preferences preference)
        {
            List<Hotel> preferedHotels = await _preferencesSorting.GetPreferedHotels(preference);
            return View(toMainBgWithPreferences, new AllHotelsModel { Hotels = preferedHotels});
        }

    }
}
