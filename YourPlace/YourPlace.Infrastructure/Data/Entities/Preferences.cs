using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data.Enums;

namespace YourPlace.Infrastructure.Data.Entities
{
    public class Preferences
    {
        [Key]
        public int SuggestionID { get; set; }

        [Required]
        public Location Location { get; set; }

        [Required]
        public Tourism Tourism { get; set; }

        [Required]
        public Atmosphere Atmosphere { get; set; }

        [Required]
        public Company Company { get; set; }

        [Required]
        public Pricing Pricing { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }

        [Required]
        public User User { get; set; }
        public Preferences()
        {

        }
        public Preferences(Location location, Tourism tourism, Atmosphere atmosphere, Company company, Pricing pricing, string userID)
        {
            Location = location;
            Tourism = tourism;
            Atmosphere = atmosphere;
            Company = company;
            Pricing = pricing;
            UserID = userID;
        }
    }
}
