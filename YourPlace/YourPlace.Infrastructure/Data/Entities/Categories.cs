using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data.Enums;

namespace YourPlace.Infrastructure.Data.Entities
{
    public class Categories
    {
        [Key]
        public int CategoryID { get; set; }

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

        [ForeignKey("Hotel")]
        public int HotelID { get; set; }

        [Required]
        public Hotel Hotel { get; set; }
        public Categories()
        {
            
        }
        public Categories(Location location, Tourism tourism, Atmosphere atmosphere, Company company, Pricing pricing, int hotelID)
        {
            Location = location;
            Tourism = tourism;
            Atmosphere = atmosphere;
            Company = company;
            Pricing = pricing;
            HotelID = hotelID;
        }
    }
}
