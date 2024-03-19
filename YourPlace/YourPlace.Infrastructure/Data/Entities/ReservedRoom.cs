using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourPlace.Infrastructure.Data.Entities
{
    public class ReservedRoom
    {
        /// <summary>
        /// обект, който свързва резервацията и всяка една резервирана стая; улеснява работата на базата данни и логиката
        /// </summary>
        [Key]
        public int ID { get; set; }
        [Required]
        public int ReservationID { get; set; }
        [Required]
        public int RoomID { get; set; }

        public int Count { get; set; }
        [Required]
        public int HotelID { get; set; }

        [NotMapped]
        public Reservation Reservation { get; set; }

        [NotMapped]
        public Room Room { get; set; }

        public ReservedRoom()
        {
            
        }
        public ReservedRoom(int reservationID, int roomID, int count, int hotelID)
        {
            ReservationID = reservationID;
            RoomID = roomID;
            Count = count;
            HotelID = hotelID;
        }
    }
}
