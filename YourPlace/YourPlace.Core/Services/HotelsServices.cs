using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace YourPlace.Core.Services
{
    public class HotelsServices :IDbCRUD<Hotel, int>
    {
        private readonly YourPlaceDbContext _dbContext;
        private readonly RoomAvailabiltyServices _roomAvailabiltyServices;

        public HotelsServices(YourPlaceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region CRUD For Hotels
        public async Task CreateAsync(Hotel hotel)
        {
            try
            {
                _dbContext.Hotels.Add(hotel);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Hotel> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Hotel> hotels = _dbContext.Hotels;
                
                if (isReadOnly)
                {
                    hotels = hotels.AsNoTrackingWithIdentityResolution();
                }
                return await hotels.SingleOrDefaultAsync(x => x.HotelID == key);
            }
            catch (Exception)
            {
                throw;
            }
        }
 
        public async Task<List<Hotel>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Hotel> hotels = _dbContext.Hotels;
                if (isReadOnly)
                {
                    //hotels = hotels.AsNoTrackingWithIdentityResolution();
                }
                return await hotels.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(Hotel item)
        {
            try
            {
                _dbContext.Hotels.Update(item);
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
                Hotel hotel = await ReadAsync(key, false, false);
                if (hotel is null)
                {
                    throw new ArgumentException(string.Format($"Hotel with id {key} does " +
                        $"not exist in the database!"));
                }
                _dbContext.Hotels.Remove(hotel);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region CRUD  Images
        public async Task AddImages(int hotelID, string imagePath)
        {
            try
            {
                Image image = new Image(imagePath, hotelID);
                Hotel hotel = await ReadAsync(hotelID);
                //hotel.Images.Add(image);
                _dbContext.Images.Add(image);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<Image>> ShowHotelImages(int hotelID, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            Hotel hotel = await ReadAsync(hotelID);
            IQueryable<Image> images = _dbContext.Images.Where(x => x.HotelID == hotelID); // may have a problem
            if (useNavigationalProperties)
            {
                images = images.Include(x => x.Hotel);
            }
            if (isReadOnly)
            {
                images = images.AsNoTrackingWithIdentityResolution();
            }
            return await images.ToListAsync();
        }
        public async Task DeleteImage(int hotelID, int imageID)
        {
            
            Hotel hotel = await ReadAsync(hotelID, false, false);
            if (hotel is null)
            {
                throw new ArgumentException(string.Format($"Hotel with id {hotelID} does " +
                    $"not exist in the database!"));
            }
            IEnumerable<Image> imagesInHotel = await ShowHotelImages(hotelID);
            foreach (Image image in imagesInHotel)
            {
                if(image.ImageID == imageID)
                {
                    _dbContext.Images.Remove(image);
                    hotel.Images.Remove(image);
                }
            }
            
            await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}
