using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourPlace.Infrastructure.Data.Entities
{
    public class Reservation
    {
        [Key]
        public int ReservationID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public DateOnly ArrivalDate { get; set; }

        [Required]
        public DateOnly LeavingDate { get; set; }

        [Required]
        public int PeopleCount { get; set; }

        [Required]
        public decimal Price { get; set; }

        [ForeignKey("Hotel")]
        public int HotelID { get; set; }

        [Required]
        public Hotel Hotel { get; set; }

        public List<Room> ReservedRooms { get; set; }
        public List<Family> Families { get; set; }
        public Reservation()
        {

        }
        public Reservation(string firstName, string surname, DateOnly arrivalDate, DateOnly leavingDate, int peopleCount, decimal price, int hotelID, List<Room> reservedRooms, List<Family> families)
        {
            FirstName = firstName;
            Surname = surname;
            ArrivalDate = arrivalDate;
            LeavingDate = leavingDate;
            PeopleCount = peopleCount;
            Price = price;
            HotelID = hotelID;
            ReservedRooms = reservedRooms;
            Families = families;
        }

    }
}
