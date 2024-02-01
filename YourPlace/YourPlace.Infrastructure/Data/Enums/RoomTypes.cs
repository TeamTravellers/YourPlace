using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourPlace.Infrastructure.Data.Enums
{
    public enum RoomTypes : int
    {
        none,
        studio = 1,
        doubleRoom = 2,
        tripleRoom = 3,
        deluxeRoom = 4,
        maisonette, //= 5
    }
    public static class RoomTypesHelper
    {
        public static RoomTypes GetRoomTypeForPeopleCount(int peopleCount)
        {
            if (peopleCount == 1)
            {
                return RoomTypes.studio;
            }
            if (peopleCount == 2)
            {
                return RoomTypes.doubleRoom;
            }
            if (peopleCount == 3)
            {
                return RoomTypes.tripleRoom;
            }
            if (peopleCount == 4)
            {
                return RoomTypes.deluxeRoom;
            }
            if (peopleCount > 4 && peopleCount <= 6)
            {
                return RoomTypes.maisonette;
            }
            return RoomTypes.none;

        }

    }
}
