using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data.Entities;

namespace YourPlace.Infrastructure.Data.Entities
{
    public class RoomSelection
    {
        public int RoomID { get; set; }
        public int ChosenCount { get; set; }

        public RoomSelection()
        {
            
        }
        public RoomSelection(int roomID, int count)
        {
            RoomID = roomID;
            ChosenCount = count;
        }
    }
}
