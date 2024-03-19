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

        [NotMapped]
        public List<RoomAvailability> roomsAvailability { get; set; }

        [NotMapped]
        public List<RoomSelection> ReservedRooms { get; set; }

        [NotMapped]
        public List<Family> Families { get; set; }
        public Reservation()
        {

        }
        public Reservation(string firstName, string surname, DateOnly arrivalDate, DateOnly leavingDate, decimal price, int peopleCount, int hotelID, List<RoomSelection> reservedRooms)
        {
            FirstName = firstName;
            Surname = surname;
            ArrivalDate = arrivalDate;
            LeavingDate = leavingDate;
            Price = price;
            PeopleCount = peopleCount;
            HotelID = hotelID;
            ReservedRooms = reservedRooms;
        }
        //public Reservation(string firstName, string surname, DateOnly arrivalDate, DateOnly leavingDate, int peopleCount, decimal price, int hotelID, int roomID)
        //{
        //    FirstName = firstName;
        //    Surname = surname;
        //    ArrivalDate = arrivalDate;
        //    LeavingDate = leavingDate;
        //    PeopleCount = peopleCount;
        //    Price = price;
        //    HotelID = hotelID;
        //    RoomID = roomID;
        //}
        //public Reservation(string firstName, string surname, DateOnly arrivalDate, DateOnly leavingDate, int peopleCount, decimal price, int hotelID, List<Room> reservedRooms, List<Family> families)
        //{
        //    FirstName = firstName;
        //    Surname = surname;
        //    ArrivalDate = arrivalDate;
        //    LeavingDate = leavingDate;
        //    PeopleCount = peopleCount;
        //    Price = price;
        //    HotelID = hotelID;
        //    ReservedRooms = reservedRooms;
        //    Families = families;
        //}

    }
}
