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
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Surname { get; set; }

        public User()
        { 

        }
        public User(string username, string email, string firstName, string surname)
        {
            this.UserName = username;
            this.NormalizedUserName = username.ToUpper();
            this.Email = email;
            this.NormalizedEmail = email.ToUpper();
            this.FirstName = firstName;
            this.Surname = surname;
        }
    }
}
