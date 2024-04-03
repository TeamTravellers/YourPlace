using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourPlace.Infrastructure.Data.Entities
{
    public class Hotel
    {
        [Key]
        public int HotelID { get; set; }

        [Required]
        public string MainImageURL { get; set; }

        [Required] 
        public string HotelName { get; set; }

        [Required]
        public string Address { get; set;}

        [Required]
        public string Town { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public double Rating { get; set; }

        [Required]
        public string Details { get; set; }

        [AllowNull]
        public string? ManagerID { get; set; }
        public List<Image> Images { get; set; }
        public Hotel()
        {

        }
        public Hotel(string mainImageURL, string hotelName, string address, string town, string country, double rating, string details)
        {
            MainImageURL = mainImageURL;
            HotelName = hotelName;
            Address = address;
            Town = town;
            Country = country;
            Rating = rating;
            Details = details;
        }
        public Hotel(string mainImageURL, string hotelName, string address, string town, string country, double rating, string details, string managerID)
        {
            MainImageURL = mainImageURL;
            HotelName = hotelName;
            Address = address;
            Town = town;
            Country = country;
            Rating = rating;
            Details = details;
            ManagerID = managerID;
        }
        public override string ToString()
        {
            return $"{this.MainImageURL} {this.HotelName} {this.Address} {this.Town} {this.Country} {this.Rating} {this.Details}";
        }
    }   
   
}
