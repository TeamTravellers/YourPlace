using System.ComponentModel;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Core.Services;
using YourPlace.Infrastructure.Data.Enums;

namespace YourPlace.Models
{
    public class AllHotelsModel
    {
        public decimal Price { get; set; }
        public string Country { get; set; }
        public int PeopleCount { get; set; }
        public DateOnly ArrivalDate { get; set; }
        public DateOnly LeavingDate { get; set; }
        public List<Hotel> Hotels { get; set; }
        public Hotel HotelModel { get; set; }
        public List<Image> HotelImages { get; set; } = new List<Image>();
        public Preferences Preferences { get; set; }

        public int PreferencesID { get; set; }
        public Location Location { get; set; }
        public Tourism Tourism { get; set; }
        public Atmosphere Atmosphere { get; set; }
        public Company Company { get; set; }
        public Pricing Pricing { get; set; }

    }
}
