using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Infrastructure.Data.Enums;

namespace YourPlace.Core.Services
{
    public class RoomAvailabiltyServices
    {
        private readonly YourPlaceDbContext _dbContext;
        private readonly HotelsServices _hotelsServices;
        public RoomAvailabiltyServices(YourPlaceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Some CRUD methods
        public async Task CreateAsync(int hotelID, RoomTypes type, int count)
        {
            try
            {
                
                _dbContext.RoomsAvailability.Add(new RoomAvailability(hotelID, type, count)); 
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<RoomAvailability>> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                Hotel hotel = await _hotelsServices.ReadAsync(key);
                IQueryable<RoomAvailability> availability = _dbContext.RoomsAvailability;
                if (useNavigationalProperties)
                {
                    availability = availability.Include(x => x.Room);
                }
                if (isReadOnly)
                {
                    availability = availability.AsNoTrackingWithIdentityResolution();
                }
                List<RoomAvailability> roomAvailabilitiesInHotel = await availability.Where(x => x.HotelID == key).ToListAsync();
                return roomAvailabilitiesInHotel.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        public async Task FillAvailability(int hotelID)
        {
            try { 
            
                List<Room> roomsInHotel = _dbContext.Rooms.Where(x=>x.HotelID == hotelID).ToList();
                List<RoomTypes> roomTypes = new List<RoomTypes>();
                List<Room> roomsOfTheSameType = new List<Room>();
                int count = 0;
                foreach(Room room in roomsInHotel)
                {
                    roomTypes.Add(room.Type);
                    roomTypes.Distinct();

                }
                foreach(var type in roomTypes)
                {
                    roomsOfTheSameType = roomsInHotel.Where(x => x.Type.ToString().ToLower() == type.ToString().ToLower()).ToList();
                    count++;
                    CreateAsync(hotelID, type, count);
                }
                
            }
            catch(Exception)
            {
                throw;
            }
        }
        public async Task<int> GetTotalCountOfRoomsInHotel(int hotelID)
        {
            Hotel hotel = await _hotelsServices.ReadAsync(hotelID);
            List<RoomAvailability> roomsTypeAndAvailabilityInHotel = await ReadAsync(hotelID);
            int total = 0;
            foreach(var item in roomsTypeAndAvailabilityInHotel)
            {
                total += item.Availability;
            }
            return total;

        }
        public async Task<List<Room>> GetAllRoomsInHotel(int hotelID)
        {
            try
            {
                Hotel hotel = await _hotelsServices.ReadAsync(hotelID);
                IQueryable<Room> rooms = _dbContext.Rooms;
                List<Room> resultList = new List<Room>();
                foreach (var room in rooms)
                {
                    if (room.HotelID == hotelID)
                    {
                        resultList.Add(room);
                    }
                }
                return resultList;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task<int> GetMaxCountOfPeopleInHotel(int hotelID)
        {
            try
            {
                List<Room> roomsInHotel = await GetAllRoomsInHotel(hotelID);
                int maxCount = 0;
                foreach (var room in roomsInHotel)
                {
                    maxCount += room.MaxPeopleCount;
                }
                return maxCount;
            }
            catch
            {
                throw;
            }
        }
    }
}

