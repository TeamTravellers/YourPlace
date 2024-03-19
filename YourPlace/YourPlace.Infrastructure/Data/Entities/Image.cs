﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourPlace.Infrastructure.Data.Entities
{
    public class Image
    {
        [Key]
        public int ImageID { get; set; }

        [Required]
        public string ImageURL {  get; set; }

        [ForeignKey("HotelID")]
        public int HotelID { get; set; }

        [NotMapped]
        public Hotel Hotel { get; set; }

        public Image()
        {

        }
        public Image(string imageURL, int hotelID)
        {
            ImageURL = imageURL;
            HotelID = hotelID;
        }
    }
}
