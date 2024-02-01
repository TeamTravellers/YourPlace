using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data.Enums;

namespace YourPlace.Infrastructure.Data.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        
        public string Surname { get; set; }

        
        public string Email { get; set; }

        
        public string Password { get; set; }

        
        public Roles Role { get; set; }

        public Preferences Preferences { get; set; }
        public User()
        { 

        }
        public User(string firstName, string surname, string email)
        {
            FirstName = firstName;
            Surname = surname;
            Email = email;
            NormalizedEmail = email.ToUpper();
            this.Preferences = Preferences;
            //Password = password;
            //Role = role;
        }
    }
}
