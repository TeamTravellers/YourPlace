using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data;
using YourPlace.Core.Contracts;
using YourPlace.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace YourPlace.Core.Services
{
    public class HotelCategoriesServices : IDbCRUD<Categories, int>
    {
        private readonly YourPlaceDbContext _dbContext;
        public HotelCategoriesServices(YourPlaceDbContext dbContext)
        {
            dbContext = _dbContext;
        }
        #region CRUD
        public async Task CreateAsync(Categories category)
        {
            try
            {
                _dbContext.Categories.Add(category);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Categories> ReadAsync(int hotelID, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Categories> categories = _dbContext.Categories;
                
                if (useNavigationalProperties)
                {
                    categories = categories.Include(x => x.Hotel);
                }
                if (isReadOnly)
                {
                    categories = categories.AsNoTrackingWithIdentityResolution();
                }
                return await categories.SingleOrDefaultAsync(x => x.HotelID == hotelID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<Categories>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Categories> categories = _dbContext.Categories;
                if (useNavigationalProperties)
                {
                    categories = categories.Include(x => x.Hotel);
                }
                if (isReadOnly)
                {
                    categories = categories.AsNoTrackingWithIdentityResolution();
                }
                return await categories.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task UpdateAsync(Categories category)
        {
            try
            {
                _dbContext.Categories.Update(category);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task DeleteAsync(int hotelID)
        {
            try
            {
                Categories categories = await ReadAsync(hotelID, false, false);
                if (categories is null)
                {
                    throw new ArgumentException(string.Format($"Category with hotelID {hotelID} does " +
                        $"not exist in the database!"));
                }
                _dbContext.Categories.Remove(categories);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
