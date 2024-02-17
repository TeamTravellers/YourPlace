using YourPlace.Infrastructure.Data.Entities;
namespace YourPlace.Models
{
    public class FilterModel
    {
        public decimal Price { get; set; }
        public string Country { get; set; }
        public int PeopleCount { get; set; }
        public DateOnly ArrivalDate { get; set; }
        public DateOnly LeavingDate { get; set; }
        public List<Hotel> Hotels { get; set; }
    }
}
