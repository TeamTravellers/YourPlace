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
    public class RoomAvailability
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int HotelID { get; set; }

        public int Availability {  get; set; }

        [Required]
        public RoomTypes Type { get; set; }

        [NotMapped]
        public Room Room { get; set; }

        public RoomAvailability()
        {

        }
        public RoomAvailability(int hotelID, RoomTypes type, int availability)
        {
            HotelID = hotelID;
            Type = type;
            Availability = availability;
        }
    }
}
