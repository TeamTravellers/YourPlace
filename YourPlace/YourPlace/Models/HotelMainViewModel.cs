using System.ComponentModel.DataAnnotations;
using YourPlace.Infrastructure.Data.Entities;

namespace YourPlace.Models
{
    public class HotelMainViewModel
    {
        public string Name { get; set; }

        public string MainImage { get; set; }
        public string Town { get; set; }
        public string Country { get; set; }
        public double Rating { get; set; }

        public string Details { get; set; }

        public List<Image> Images { get; set; }

        public HotelMainViewModel(string name, string mainImage, string town, string country, double rating, string details)
        {
            this.Name = name;
            this.MainImage = mainImage;
            this.Town = town;
            this.Country = country;
            this.Rating = rating;
            this.Details = details;
        }

        //public Hotel Hotel { get; set; }
    }
}
