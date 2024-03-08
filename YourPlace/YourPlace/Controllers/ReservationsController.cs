using Microsoft.AspNetCore.Mvc;
using YourPlace.Core.Services;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Infrastructure.Data.Enums;
using YourPlace.Models;

namespace YourPlace.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ReservationServices _reservationServices;
        private readonly HotelsServices _hotelsServices;
        private readonly RoomAvailabiltyServices _roomAvailabiltyServices;

        //handles the reservations
        public ReservationsController(ReservationServices reservationServices, HotelsServices hotelsServices, RoomAvailabiltyServices roomAvailabiltyServices)
        {
            _reservationServices = reservationServices;
            _hotelsServices = hotelsServices;
            _roomAvailabiltyServices = roomAvailabiltyServices;

        }
        private const string toReservation = "~/Views/Bulgarian/Hotels/Reservation.cshtml";
        private const string toAvailability = "~/Views/Bulgarian/Hotels/Availability.cshtml";
        public async Task<IActionResult> Index([Bind("Id")] int hotelID)
        {
            Hotel hotel = await _hotelsServices.ReadAsync(hotelID);
            //List<Image> images = await _hotelsServices.ShowHotelImages(hotelID);
            return View(toReservation, new AllHotelsModel { HotelModel = hotel});
        }
        public async Task<IActionResult> Availability([Bind("hotelID")] int hotelID, [Bind("peopleCount")] int peopleCount, [Bind("ArrivalDate")] DateOnly arrivalDate, [Bind("LeavingDate")] DateOnly leavingDate)
        {
            //bool result = await _reservationServices.CheckForTotalRoomAvailability(hotelID, peopleCount);
            List<Room> availableRooms = new List<Room>();
            List<Tuple<int, RoomTypes>> countRooms = new List<Tuple<int, RoomTypes>>();
            List<Room> rooms = new List<Room>();

            //if (result == false)
            //{
            //    ViewBag.Message = "There are no places for so many people in this hotel!";
            //    Console.WriteLine("There are no places for so many people!");
                
            //}
            //else
            //{
            
                availableRooms = await _reservationServices.FreeRoomCheck(arrivalDate, leavingDate, hotelID);
                countRooms = await _reservationServices.FreeRoomsAccordingToTypeAsync(arrivalDate, leavingDate, availableRooms);
                rooms = await _reservationServices.GetRoomsByTypes(hotelID, countRooms);
                //return View(); // Return your view here
            //}
            return View(toAvailability, new AllHotelsModel { Rooms = rooms, ArrivalDate = arrivalDate, LeavingDate = leavingDate });
        }

        //checks if a room is available
        public async Task<IActionResult> CheckAvailability([Bind("FirstName")] string firstName, [Bind("Surname")] string surname, [Bind("ArrivalDate")] DateOnly arrivalDate, [Bind("LeavingDate")] DateOnly leavingDate, [Bind("peopleCount")] int peopleCount, [Bind("hotelID")] int hotelID)
        {
            //bool result = await _reservationServices.CheckForTotalRoomAvailability(hotelID, peopleCount);
            //List<Room> availableRooms = new List<Room>();
            //List<Tuple<int, RoomTypes>> countRooms = new List<Tuple<int, RoomTypes>>();
            //List<Room> rooms = new List<Room>();
            ////int roomID = 0;
            //RoomTypes type = RoomTypesHelper.GetRoomTypeForPeopleCount(peopleCount);
            //List<Room> roomsInHotel = await _roomAvailabiltyServices.GetAllRoomsInHotel(hotelID);
            ////availableRooms = await _reservationServices.FreeRoomCheck(arrivalDate, leavingDate, hotelID);
            //Room room = roomsInHotel.Where(x => x.Type.ToString() == type.ToString()).FirstOrDefault();
            //int roomID = room.RoomID;
            Reservation reservation = new Reservation(firstName, surname, arrivalDate, leavingDate, peopleCount, hotelID, 1);
            
            //await _reservationServices.CreateAsync(reservation);
            //countRooms = await _reservationServices.FreeRoomsAccordingToTypeAsync(arrivalDate, leavingDate, availableRooms);
            //rooms = await _reservationServices.GetRoomsByTypes(hotelID, countRooms);
            //return View(); // Return your view here
            //}
            //return View(toAvailability, new AllHotelsModel { Rooms = availableRooms, ArrivalDate = arrivalDate, LeavingDate = leavingDate });
            //return View(toAvailability);
            return View(toAvailability, new AllHotelsModel { FirstName = firstName, Surname = surname, ArrivalDate = arrivalDate, LeavingDate = leavingDate, PeopleCount = peopleCount});
        }
    }
}
