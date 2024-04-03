using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Infrastructure.Data.Enums;

namespace YourPlace.Models.ManagerModels
{
    public class HotelCreateModel
    {
        public int HotelID { get; set; }
        public string MainImageURL { get; set; }
        public string HotelName { get; set; }
        public string Address { get; set; }
        public string Town { get; set; }
        public string Country { get; set; }
        public double Rating { get; set; }
        public string Details { get; set; }
        public string ManagerID { get; set; }
        public List<Image> HotelImages { get; set; }

        public List<Room> RoomsInHotel { get; set; }

        public Location Location { get; set; }

        public Tourism Tourism { get; set; }
        public Atmosphere Atmosphere { get; set; }
        public Company Company { get; set; }    
        public Pricing Pricing { get; set; }
        

        public List<Hotel> ManagerHotels { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
