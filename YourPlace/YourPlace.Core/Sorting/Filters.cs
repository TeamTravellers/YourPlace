using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Core.Contracts;
using YourPlace.Core.Services;
using YourPlace.Infrastructure.Data;
using YourPlace.Infrastructure.Data.Entities;

namespace YourPlace.Core.Sorting
{
    public class Filters : IFilter
    {
        private readonly YourPlaceDbContext _dbContext;
        private readonly HotelsServices _hotelsServices;
        private readonly RoomAvailabiltyServices _roomAvailabiltyServices;

        public Filters(YourPlaceDbContext dbContext, HotelsServices hotelsServices, RoomAvailabiltyServices roomAvailabiltyServices)
        {
            _dbContext = dbContext;
            _hotelsServices = hotelsServices;
            _roomAvailabiltyServices = roomAvailabiltyServices;
        }
        public async Task<List<Hotel>> FilterByCountry(string country)
        {
            return await _dbContext.Hotels.Where(x => x.Country == country).ToListAsync();
        }
        public async Task<List<Hotel>> FilterByPeopleCount(int count)
        {
            var hotels = await _hotelsServices.ReadAllAsync();
            var filteredHotels = new List<Hotel>();
            foreach (var hotel in hotels)
            {
                int maxCount = await _roomAvailabiltyServices.GetMaxCountOfPeopleInHotel(hotel.HotelID);
                if (maxCount >= count)
                {
                    filteredHotels.Add(hotel);
                }
            }
            return filteredHotels;
        }

        public async Task<List<Hotel>> FilterByPrice(decimal price)
        {
            var rooms = await _dbContext.Rooms.Where(x => x.Price <= price).ToListAsync();
            var filteredHotels = new List<Hotel>();
            foreach (var room in rooms)
            {
                filteredHotels = await _dbContext.Hotels.Where(x => x.HotelID == room.HotelID).Distinct().ToListAsync();
            }
            return filteredHotels;
        }
        public async Task<List<Hotel>> FilterByDates(DateOnly arrivingDate, DateOnly leavingDate)
        {
            var reservations = await _dbContext.Reservations.ToListAsync();
            var freeRooms = new List<Room>();
            var filteredHotels = new List<Hotel>();

            foreach (var reservation in reservations)
            {
                if (leavingDate < reservation.ArrivalDate && arrivingDate < leavingDate || arrivingDate > reservation.LeavingDate && leavingDate > arrivingDate)
                {
                    filteredHotels = await _dbContext.Hotels.Where(x => x.HotelID == reservation.HotelID).ToListAsync();
                }
            }
            //foreach(var room in freeRooms)
            //{
            //    filteredHotels = await _dbContext.Hotels.Where(x => x.HotelID == room.HotelID).Distinct().ToListAsync();
            //}
            return filteredHotels;
        }
        //public async Task<List<Hotel>> FilterByOpportunityForPets(int hotelID)
        //{

        //}

    }
}
