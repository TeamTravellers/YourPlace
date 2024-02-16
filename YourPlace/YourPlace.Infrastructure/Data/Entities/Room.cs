using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data.Enums;

namespace YourPlace.Infrastructure.Data.Entities
{
    public class Room
    {
        [Key]
        public int RoomID { get; set; }

        [Required]
        public RoomTypes Type { get; set; } //студио, апартамент...

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int MaxPeopleCount { get; set; }

        [Required]
        public int CountInHotel { get; set; }

        [ForeignKey("Hotel")]
        public int HotelID { get; set; }

        [NotMapped]
        public Hotel Hotel { get; set; }
        public Room()
        {
            
        }
        public Room(RoomTypes type, decimal Price, int maxPeopleCount, int countinHotel, int hotelID)
        {
            this.Type = type;
            this.Price = Price;
            this.MaxPeopleCount = maxPeopleCount;
            this.HotelID = hotelID;
            this.CountInHotel = countinHotel;
        }
        public override string ToString()
        {
            return $"{this.Type} | {this.Price} | {this.MaxPeopleCount}";
        }
    }
}
