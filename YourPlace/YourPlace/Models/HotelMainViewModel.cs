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

    }
}
