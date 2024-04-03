using Microsoft.AspNetCore.Mvc;
using YourPlace.Infrastructure.Data.Entities;
namespace YourPlace.Models
{
    public class ReservationModel
    {
        public Hotel HotelModel { get; set; }
        public List<Image> HotelImages {  get; set; }
        public int HotelID { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int PeopleCount { get; set; }
        public DateOnly ArrivalDate { get; set; }
        public DateOnly LeavingDate { get; set; }
        public decimal TotalPrice { get; set; }

        public Room ChosenRoom { get; set; }
        public int ChosenRoomID { get; set; }
        public int ChosenRoomCount { get; set; }
        public List<Tuple<Room, int>> AvailableRooms { get; set; }

        [BindProperty]
        public List<RoomSelection> ChosenRooms { get; set; }

        //public IActionResult OnPost()
        //{
        //    var list = ChosenRooms;
        //    TempData["ChosenRooms"] = list;
        //    return RedirectToActionResult(result);
        //}
    }
}
