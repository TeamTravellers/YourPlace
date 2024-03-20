using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Core.Contracts;
using YourPlace.Infrastructure.Data;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Infrastructure.Data.Enums;

namespace YourPlace.Core.Services
{
    public class RoomServices : IDbCRUD<Room, int>
    {
        private readonly YourPlaceDbContext _dbContext;
        private readonly HotelsServices _hotelsServices;
        public RoomServices(YourPlaceDbContext dbContext, HotelsServices hotelsServices)
        {
            _dbContext = dbContext;   
            _hotelsServices = hotelsServices;
        }
        #region CRUD For Rooms
        public async Task CreateAsync(Room room)
        {
            try
            {
                _dbContext.Rooms.Add(room);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Room> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Room> rooms = _dbContext.Rooms;
                if (useNavigationalProperties)
                {
                    rooms = rooms.Include(x => x.Hotel);
                }
                if (isReadOnly)
                {
                    rooms = rooms.AsNoTrackingWithIdentityResolution();
                }
                return await rooms.SingleOrDefaultAsync(x => x.RoomID == key);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Room>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Room> rooms = _dbContext.Rooms;
                if (isReadOnly)
                {
                    //hotels = hotels.AsNoTrackingWithIdentityResolution();
                }
                return await rooms.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(Room room)
        {
            try
            {
                _dbContext.Rooms.Update(room);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAsync(int key)
        {
            try
            {
                Room room = await ReadAsync(key, false, false);
                if (room is null)
                {
                    throw new ArgumentException(string.Format($"Room with id {key} does " +
                        $"not exist in the database!"));
                }
                _dbContext.Rooms.Remove(room);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public async Task<List<Room>> GetAllRoomsInHotel(int hotelID)
        {
            Hotel hotel = await _hotelsServices.ReadAsync(hotelID);
            List<Room> roomList = await ReadAllAsync();
            List<Room> roomsInHotel = new List<Room>();

            foreach(Room room in roomList)
            {
                if(room.HotelID == hotelID)
                {
                    roomsInHotel.Add(room);
                }
            }
            return roomsInHotel;
        }

    }
}
