using System.ComponentModel.DataAnnotations;
using YourPlace.Infrastructure.Data.Entities;

namespace YourPlace.Models
{
    public class HotelMainViewModel
    {
        public Hotel HotelModel { get; set; }
        public List<Image> HotelImages { get; set; } = new List<Image>();
    }
}
