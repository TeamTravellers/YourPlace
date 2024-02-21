﻿using System;
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
    public class PreferencesServices : IDbCRUD<Preferences, int>
    {
        private readonly YourPlaceDbContext _dbContext;
        //private readonly UserManager<User> _userManager;
        public PreferencesServices(YourPlaceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region CRUD
        public async Task CreateAsync(Preferences preferences)
        {
            try
            {
                _dbContext.Preferences.Add(preferences);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Preferences> ReadAsync(int preferenceID, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Preferences> preferences = _dbContext.Preferences;
                if (useNavigationalProperties)
                {
                    //preferences = preferences.Include(x => x.User);
                }
                if (isReadOnly)
                {
                    preferences = preferences.AsNoTrackingWithIdentityResolution();
                }
                return await preferences.SingleOrDefaultAsync(x => x.PreferencesID == preferenceID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Preferences> ReadByUserAsync(string userID, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Preferences> preferences = _dbContext.Preferences;
                if (useNavigationalProperties)
                {
                    //preferences = preferences.Include(x => x.User);
                }
                if (isReadOnly)
                {
                    preferences = preferences.AsNoTrackingWithIdentityResolution();
                }
                return await preferences.SingleOrDefaultAsync(x => x.UserId == userID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<Preferences>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Preferences> preferences = _dbContext.Preferences;
                if (useNavigationalProperties)
                {
                    //preferences = preferences.Include(x => x.User);
                }
                if (isReadOnly)
                {
                    preferences.AsNoTrackingWithIdentityResolution();
                }
                return await preferences.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task UpdateAsync(Preferences suggestion)
        {

            try
            {
                _dbContext.Preferences.Update(suggestion);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAsync(int preferenceID)
        {
            try
            {
                Preferences preferences = await ReadAsync(preferenceID, false, false);
                if (preferences is null)
                {
                    throw new ArgumentException(string.Format($"Preferences with ID {preferenceID} does " +
                        $"not exist in the database!"));
                }
                _dbContext.Preferences.Remove(preferences);
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
