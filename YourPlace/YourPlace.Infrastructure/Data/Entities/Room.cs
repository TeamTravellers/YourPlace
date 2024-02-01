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

        [ForeignKey("Hotel")]
        public int HotelID { get; set; }
    }
}
