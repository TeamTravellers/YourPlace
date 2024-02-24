using YourPlace.Infrastructure.Data.Entities;
namespace YourPlace.Models
{
    public class ReservationModel
    {
        public Dictionary<int, List<Room>> Rooms { get; set; }
        public int freeRoomsCount { get; set; }
    }
}
