using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Infrastructure.Data.Enums;
using YourPlace.Core.Contracts;
using Microsoft.AspNetCore.Identity;
using YourPlace.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;
namespace YourPlace.Core.Services
{
    public class UserServices 
    {
        private readonly UserManager<User> _userManager;
        private readonly YourPlaceDbContext _dbContext;
        public UserServices(YourPlaceDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        #region SIGN UP
        public async Task<User> CreateAccountAsync(string firstName, string surname, string email, string password, Roles role)
        {
            try
            {
                User user = new User(firstName, surname, email);
                IdentityResult result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    throw new ArgumentException(result.Errors.First().Description);
                }
                if (role == Roles.HotelManager)
                {
                    await _userManager.AddToRoleAsync(user, Roles.HotelManager.ToString());
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, Roles.Traveller.ToString());
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region LOG IN
        public async Task<User> LogInUserAsync(string username, string password)
        {
            try
            {
                User user = await _userManager.FindByNameAsync(username);

                if (user == null)
                {
                    // create IdentityError => "Username not found!"
                    CreateAccountAsync(user.FirstName, user.Surname, user.Email, user.Password, user.Role);
                }

                IdentityResult result = await _userManager.PasswordValidators[0].ValidateAsync(_userManager, user, password);

                if (result.Succeeded)
                {
                    return user;
                    //return await context.Users.FindAsync(user.Id);
                }
                else
                {
                    // create IdentityError => "Password is not correct!"
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        public async Task<User> ReadUserAsync(string key)
        {
            try
            {
                return await _userManager.FindByIdAsync(key);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<User>> ReadAllUsersAsync()
        {
            try
            {
                return await _dbContext.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region Account Features
        public async Task UpdateAccountAsync(string firstName, string surname, string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    User userToBeEdited = await _userManager.FindByEmailAsync(email);
                    userToBeEdited.FirstName = firstName;
                    userToBeEdited.Surname = surname;
                    userToBeEdited.Email = email;
                    await _userManager.UpdateAsync(userToBeEdited);
                }
                
            }
            catch(Exception)
            {
                throw;
            }
            
            //userToBeEdited.Password = editedUser.Password;
            //userToBeEdited.Role = editedUser.Role;
            
        }
        public async Task ResetPasswordAsync(string id, string newPassword)
        {
            User user = await _userManager.FindByIdAsync(id);
            user.Password = newPassword;
            await _userManager.UpdateAsync(user);
        }
        public async Task DeleteAccountAsync(string id)
        {
            try
            {
                User user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    throw new InvalidOperationException("User not found for deletion");
                }
                await _userManager.DeleteAsync(user);
            }
            catch(Exception)
            {
                throw;
            }
        }

        #endregion
        #region CRUD for Roles

        public async Task CreateRoleAsync(IdentityRole role)
        {
            try
            {
                _dbContext.Roles.Add(role);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new ArgumentException("The role already exists");
            }
        }
        #endregion

    }
}
