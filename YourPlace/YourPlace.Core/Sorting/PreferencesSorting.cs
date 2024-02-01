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
        private readonly UserQuestionsServices _userQuestionsServices;
        private readonly HotelCategoriesServices _hotelCategoriesServices;
        //private readonly HotelsServices _hotelsServices;
        private readonly UserManager<User> _userManager;
        
        public PreferencesSorting(UserQuestionsServices userQuestionsServices, HotelCategoriesServices hotelCategoriesServices, UserManager<User> userManager)
        {
            _userQuestionsServices = userQuestionsServices;
            _hotelCategoriesServices = hotelCategoriesServices;
            _userManager = userManager;
        }

        public async Task<Dictionary<Hotel, int>> GetUserPreferencesCount(string userID)
        {
            try
            {
                User user = await _userManager.FindByIdAsync(userID);
                user.Preferences = await _userQuestionsServices.ReadAsync(userID);
                //var hotels = await _hotelsServices.ReadAllAsync();
                //var hotels = new List<Hotel>();
                var hotelsCategories = await _hotelCategoriesServices.ReadAllAsync();
                int commonPreferencesCount = 0;
                //var preferedHotels = new List<Hotel>();

                Tuple<Location, Tourism, Atmosphere, Company, Pricing> preferences = new Tuple<Location, Tourism, Atmosphere, Company, Pricing>
                (
                    user.Preferences.Location,
                    user.Preferences.Tourism,
                    user.Preferences.Atmosphere,
                    user.Preferences.Company,
                    user.Preferences.Pricing
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
        
    }
}
