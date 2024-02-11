using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Infrastructure.Data.Enums;
using YourPlace.Core.Contracts;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using YourPlace.Core.Services;
using System.Collections;
using Microsoft.Identity.Client;

namespace YourPlace.Core.Sorting
{
    public class PreferencesSorting
    {
        private readonly PreferencesServices _userQuestionsServices;
        private readonly HotelCategoriesServices _hotelCategoriesServices;
        private readonly HotelsServices _hotelsServices;
        private readonly UserManager<User> _userManager;

        
        public PreferencesSorting(PreferencesServices userQuestionsServices, HotelCategoriesServices hotelCategoriesServices, HotelsServices hotelServices,UserManager<User> userManager)
        {
            _userQuestionsServices = userQuestionsServices;
            _hotelCategoriesServices = hotelCategoriesServices;
            _hotelsServices = hotelServices;
            _userManager = userManager;
        }
        public async Task<Dictionary<int, int>> GetPreferencesCount(Preferences preference)
        {
            try
            {
                var hotelsCategories = await _hotelCategoriesServices.ReadAllAsync();
                int commonPreferencesCount = 0;
                Tuple<Location, Tourism, Atmosphere, Company, Pricing, int> categories;
                List<Tuple<Location, Tourism, Atmosphere, Company, Pricing, int>> categoriesForHotels = new List<Tuple<Location, Tourism, Atmosphere, Company, Pricing, int>>();
                foreach (var category in hotelsCategories)
                {
                    categories = Tuple.Create
                    (
                        category.Location,
                        category.Tourism,
                        category.Atmosphere,
                        category.Company,
                        category.Pricing,
                        category.HotelID
                    );
                    categoriesForHotels.Add(categories);
                }
                Dictionary<int, int> matchingPreferences = new Dictionary<int, int>();

                foreach (var categoryTuple in categoriesForHotels)
                {
                    commonPreferencesCount = 0;
                    if (categoryTuple.Item1 == preference.Location)
                    {
                        commonPreferencesCount++;
                    }
                    if (categoryTuple.Item2 == preference.Tourism)
                    {
                        commonPreferencesCount++;
                    }
                    if (categoryTuple.Item3 == preference.Atmosphere)
                    {
                        commonPreferencesCount++;
                    }
                    if (categoryTuple.Item4 == preference.Company)
                    {
                        commonPreferencesCount++;
                    }
                    if (categoryTuple.Item5 == preference.Pricing)
                    {
                        commonPreferencesCount++;
                    }
                    
                    matchingPreferences.Add(categoryTuple.Item6, commonPreferencesCount);

                }
                return matchingPreferences.ToDictionary();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<List<Hotel>> GetPreferedHotels(Preferences preference)
        {
            List<Hotel> preferedHotels = new List<Hotel>();
            Dictionary<int, int> matchingPreferences = await GetPreferencesCount(preference);
            //matchingPreferences = matchingPreferences.OrderByDescending(x => x.Value).ToDictionary();
            Dictionary<Hotel, int> preferencesSorting = new Dictionary<Hotel, int>();
            foreach(var pair in matchingPreferences)
            {
                Hotel hotel = await _hotelsServices.ReadAsync(pair.Key);
                preferencesSorting.Add(hotel, pair.Value);
            }
            preferencesSorting = preferencesSorting.OrderByDescending(x=>x.Value).ThenBy(x=>x.Key.HotelName).ToDictionary();
            foreach (var hotel in preferencesSorting.Keys)
            {
                preferedHotels.Add(hotel);
            }
            
            return preferedHotels;
        }

        #region Preferences For Logged User
        public async Task<Dictionary<Hotel, int>> GetUserPreferencesCount(string userID)
        {
            try
            {
                User user = await _userManager.FindByIdAsync(userID);
                var userPreferences = await _userQuestionsServices.ReadAsync(userID);
                //var hotels = await _hotelsServices.ReadAllAsync();
                //var hotels = new List<Hotel>();
                var hotelsCategories = await _hotelCategoriesServices.ReadAllAsync();
                int commonPreferencesCount = 0;
                //var preferedHotels = new List<Hotel>();

                Tuple<Location, Tourism, Atmosphere, Company, Pricing> preferences = new Tuple<Location, Tourism, Atmosphere, Company, Pricing>
                (
                    userPreferences.Location,
                    userPreferences.Tourism,
                    userPreferences.Atmosphere,
                    userPreferences.Company,
                    userPreferences.Pricing
                );

                Tuple<Location, Tourism, Atmosphere, Company, Pricing, Hotel> categories;
                List<Tuple<Location, Tourism, Atmosphere, Company, Pricing, Hotel>> categoriesForHotels = new List<Tuple<Location, Tourism, Atmosphere, Company, Pricing, Hotel>>();
                foreach (var category in hotelsCategories)
                {
                    categories = Tuple.Create
                    (
                        category.Location,
                        category.Tourism,
                        category.Atmosphere,
                        category.Company,
                        category.Pricing,
                        category.Hotel
                    );
                    //hotels.Add(category.Hotel);
                    categoriesForHotels.Add(categories);
                }
                Dictionary<Hotel, int> matchingPreferences = new Dictionary<Hotel, int>();

                foreach (var categoryTuple in categoriesForHotels)
                {
                    if (categoryTuple.Item1 == preferences.Item1)
                    {
                        commonPreferencesCount++;
                    }
                    if (categoryTuple.Item2 == preferences.Item2)
                    {
                        commonPreferencesCount++;
                    }
                    if (categoryTuple.Item3 == preferences.Item3)
                    {
                        commonPreferencesCount++;
                    }
                    if (categoryTuple.Item4 == preferences.Item4)
                    {
                        commonPreferencesCount++;
                    }
                    if (categoryTuple.Item5 == preferences.Item5)
                    {
                        commonPreferencesCount++;
                    }
                    matchingPreferences.Add(categoryTuple.Item6, commonPreferencesCount);

                }
                return matchingPreferences.ToDictionary();
            }
            catch (Exception)
            {
                throw;
            }
           
        }
        public async Task<List<Hotel>> GetPreferedHotel(string userID)
        {
            Dictionary<Hotel, int> matchingPreferences = await GetUserPreferencesCount(userID);
            matchingPreferences = matchingPreferences.OrderByDescending(x => x.Value).ToDictionary();

            List<Hotel> preferedHotels = matchingPreferences.Keys.ToList();
            return preferedHotels;
        }

        #endregion
    }
}
