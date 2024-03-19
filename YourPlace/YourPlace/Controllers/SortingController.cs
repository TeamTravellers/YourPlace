using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Diagnostics.Metrics;
using YourPlace.Core.Services;
using YourPlace.Core.Sorting;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Infrastructure.Data.Enums;
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

        private const string toFilter = "~/Views/Bulgarian/SortingViews/Filters.cshtml";
        private const string toSubmitPage = "~/Views/Bulgarian/Submit-page.cshtml";
        private const string toPreferedHotels = "~/Views/Bulgarian/PreferedHotels.cshtml";
        private const string toMain = "~/Views/Bulgarian/MainPage.cshtml";

        public IActionResult Index()
        {
            return View(toSubmitPage);
        }

        //filters the hotels according the set filters in the filters menu
        public async Task<IActionResult> Filter(AllHotelsModel model)
        {
            List<Hotel> hotels = new List<Hotel>();

            if (model.Country != null)
            {
                List<Hotel> filteredByCountry = await _filters.FilterByCountry(model.Country);
                hotels.AddRange(filteredByCountry);
            }
            if (model.PeopleCount != 0)
            {
                List<Hotel> filteredByPeopleCount = await _filters.FilterByPeopleCount(model.PeopleCount);
                hotels.AddRange(filteredByPeopleCount);
            }
            if (model.Price != 0)
            {
                List<Hotel> filteredByPrice = await _filters.FilterByPrice(model.Price);
                hotels.AddRange(filteredByPrice);
            }
            if (model.ArrivalDate != null && model.LeavingDate != null)
            {
                List<Hotel> filteredByDates = await _filters.FilterByDates(model.ArrivalDate, model.LeavingDate);
                hotels.AddRange(filteredByDates);
            }
            
            hotels = hotels.Distinct().ToList();
            return View(toMain, new AllHotelsModel { Hotels = hotels });
        }
        //returns the submit page view
        public IActionResult ToSubmitPage()
        {
            return View(toSubmitPage);
        }
        public async Task<IActionResult> CreatePreferences([Bind("Location")] Location location, [Bind("Tourism")] Tourism tourism, [Bind("Atmosphere")] Atmosphere atmosphere, [Bind("Company")] Company company, [Bind("Pricing")] Pricing pricing)
        {
            Preferences preferences = new Preferences(location, tourism, atmosphere, company, pricing);
            List<Preferences> allPreferences = await _preferencesServices.ReadAllAsync();
            Preferences createdPreference = new Preferences();
            try
            {
                await _preferencesServices.CreateAsync(preferences);
                createdPreference = allPreferences.Where(allPreferences.Contains).FirstOrDefault();
                Console.WriteLine($"{preferences.Location.ToString()}, {preferences.Tourism.ToString()}, {preferences.Atmosphere.ToString()}, {preferences.Company.ToString()}, {preferences.Pricing.ToString()}");
            }
            catch
            {
                return StatusCode(404, "Operation was not successful!");
            }
            //RedirectToAction("PreferencesSorting");
            return View(toSubmitPage, new AllHotelsModel { Preferences = preferences });
        }
        public async Task<IActionResult> PreferencesSorting([Bind("PreferenceID")] int preferencesID)
        {
            Preferences preference = await _preferencesServices.ReadAsync(preferencesID);
            Console.WriteLine(preference.PreferencesID);
            List<Hotel> preferedHotels = await _preferencesSorting.GetPreferedHotels(preference);
            return View(toMain, new AllHotelsModel { Hotels = preferedHotels });
        }

    }
}
