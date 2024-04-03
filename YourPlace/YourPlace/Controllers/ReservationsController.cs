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
        private const string toDatesEntering = "~/Views/Bulgarian/Hotels/DatesEntering.cshtml";
        private const string toAvailability = "~/Views/Bulgarian/Hotels/Availability.cshtml";
        private const string toReservation = "~/Views/Bulgarian/Hotels/Reservation.cshtml";
        private const string toReservationResult = "~/Views/Bulgarian/Hotels/ReservationResult.cshtml";
        public async Task<IActionResult> Index([Bind("Id")] int hotelID)
        {
            Hotel hotel = await _hotelsServices.ReadAsync(hotelID);
            List<Image> images = await _hotelsServices.ShowHotelImages(hotelID);
            Console.WriteLine(hotelID);
            //RedirectToAction("FreeRoomsCheck");
            return View(toDatesEntering,new ReservationModel { HotelModel = hotel, HotelImages = images});
        }

#region old stuff
        //public async Task<IActionResult> Availability([Bind("hotelID")] int hotelID, [Bind("peopleCount")] int peopleCount, [Bind("ArrivalDate")] DateOnly arrivalDate, [Bind("LeavingDate")] DateOnly leavingDate)
        //{
        //    //bool result = await _reservationServices.CheckForTotalRoomAvailability(hotelID, peopleCount);
        //    List<Room> availableRooms = new List<Room>();
        //    List<Tuple<int, RoomTypes>> countRooms = new List<Tuple<int, RoomTypes>>();
        //    List<Room> rooms = new List<Room>();

        //    //if (result == false)
        //    //{
        //    //    ViewBag.Message = "There are no places for so many people in this hotel!";
        //    //    Console.WriteLine("There are no places for so many people!");

        //    //}
        //    //else
        //    //{

        //        availableRooms = await _reservationServices.FreeRoomCheck(arrivalDate, leavingDate, hotelID);
        //        countRooms = await _reservationServices.FreeRoomsAccordingToTypeAsync(arrivalDate, leavingDate, availableRooms);
        //        rooms = await _reservationServices.GetRoomsByTypes(hotelID, countRooms);
        //        //return View(); // Return your view here
        //    //}
        //    return View(toAvailability, new AllHotelsModel { Rooms = rooms, ArrivalDate = arrivalDate, LeavingDate = leavingDate });
        //}

        ////checks if a room is available
        //public async Task<IActionResult> CheckAvailability([Bind("FirstName")] string firstName, [Bind("Surname")] string surname, [Bind("ArrivalDate")] DateOnly arrivalDate, [Bind("LeavingDate")] DateOnly leavingDate, [Bind("peopleCount")] int peopleCount, [Bind("hotelID")] int hotelID)
        //{
        //    //bool result = await _reservationServices.CheckForTotalRoomAvailability(hotelID, peopleCount);
        //    //List<Room> availableRooms = new List<Room>();
        //    //List<Tuple<int, RoomTypes>> countRooms = new List<Tuple<int, RoomTypes>>();
        //    //List<Room> rooms = new List<Room>();
        //    ////int roomID = 0;
        //    //RoomTypes type = RoomTypesHelper.GetRoomTypeForPeopleCount(peopleCount);
        //    //List<Room> roomsInHotel = await _roomAvailabiltyServices.GetAllRoomsInHotel(hotelID);
        //    ////availableRooms = await _reservationServices.FreeRoomCheck(arrivalDate, leavingDate, hotelID);
        //    //Room room = roomsInHotel.Where(x => x.Type.ToString() == type.ToString()).FirstOrDefault();
        //    //int roomID = room.RoomID;
        //    Console.WriteLine(hotelID);
        //    Reservation reservation = new Reservation(firstName, surname, arrivalDate, leavingDate, peopleCount, hotelID);

        //    //STATUS CODES 
        //    await _reservationServices.CreateAsync(reservation);

        //    //countRooms = await _reservationServices.FreeRoomsAccordingToTypeAsync(arrivalDate, leavingDate, availableRooms);
        //    //rooms = await _reservationServices.GetRoomsByTypes(hotelID, countRooms);
        //    //return View(); // Return your view here
        //    //}
        //    //return View(toAvailability, new AllHotelsModel { Rooms = availableRooms, ArrivalDate = arrivalDate, LeavingDate = leavingDate });
        //    //return View(toAvailability);
        //    return View(toAvailability, new AllHotelsModel { FirstName = firstName, Surname = surname, ArrivalDate = arrivalDate, LeavingDate = leavingDate, PeopleCount = peopleCount, HotelID = hotelID});
        //}
#endregion
        public async Task<IActionResult> FreeRoomsCheck([Bind("ArrivalDate")] DateOnly arrivalDate, [Bind("LeavingDate")] DateOnly leavingDate, [Bind("hotelID")] int hotelID)
        {
            Hotel hotel = await _hotelsServices.ReadAsync(hotelID);
            List<Tuple<Room,int>> availableRooms = await _reservationServices.FindFreeRooms(hotelID, arrivalDate, leavingDate);
            Console.WriteLine(hotelID);
            Console.WriteLine(arrivalDate.ToString());
            Console.WriteLine(leavingDate.ToString());
            foreach (Tuple<Room, int> item in availableRooms)
            {
                Console.WriteLine(item.Item1.ToString());
                Console.WriteLine(item.Item2);
                Console.WriteLine();
            }
            List<RoomSelection> chosenRooms = new List<RoomSelection>();
            return View(toAvailability, new ReservationModel { AvailableRooms = availableRooms, ArrivalDate = arrivalDate, LeavingDate = leavingDate, HotelID = hotelID, ChosenRooms = chosenRooms });
        }
        public async Task<IActionResult> GoToReservation([Bind("ArrivalDate")] DateOnly arrivalDate, [Bind("LeavingDate")] DateOnly leavingDate, [Bind("HotelID")] int hotelID, [Bind("ChosenRooms")] List<RoomSelection> chosenRooms)
        {
            Hotel hotel = await _hotelsServices.ReadAsync(hotelID);
            decimal totalPrice = await _reservationServices.CalculatePrice(arrivalDate, leavingDate, chosenRooms);

            Console.WriteLine(totalPrice);
            Console.WriteLine($"Hotel ID: {hotelID}");
            Console.WriteLine($"Arrival Date: {arrivalDate}");
            Console.WriteLine($"Leaving Date: {leavingDate}");

            if (chosenRooms == null || chosenRooms.Count == 0)
            {
                Console.WriteLine("No rooms selected.");
            }
            else
            {
                Console.WriteLine("Selected rooms:");
                foreach (var room in chosenRooms)
                {
                    Console.WriteLine($"Room ID: {room.RoomID}, Chosen Count: {room.ChosenCount}");
                }
            }
            return View(toReservation, new ReservationModel { HotelID = hotelID, ArrivalDate = arrivalDate, LeavingDate = leavingDate, TotalPrice = totalPrice, ChosenRooms = chosenRooms});
        }
        public async Task<IActionResult> CreateReservation([Bind("FirstName")] string firstName, [Bind("Surname")] string surname, [Bind("ArrivalDate")] DateOnly arrivalDate, [Bind("LeavingDate")] DateOnly leavingDate, [Bind("peopleCount")] int peopleCount, [Bind("PhoneNumber")] string phoneNumber, [Bind("Email")] string email, [Bind("TotalPrice")] decimal totalPrice, [Bind("hotelID")] int hotelID, [Bind("ChosenRooms")] List<RoomSelection> chosenRooms)
        {
            try
            { 
                await _reservationServices.Reserve(firstName, surname, peopleCount, arrivalDate, leavingDate, totalPrice, hotelID, chosenRooms);
            }
            catch
            {
                StatusCode(404, $"Съжаляваме, но резервацията Ви не е успешна. Опитайте отново по-късно! \n Sorry, but your reservations was not successful. Please, try again later!");
            }
            return View(toReservationResult, new ReservationModel { FirstName = firstName, Surname = surname, PeopleCount = peopleCount, PhoneNumber = phoneNumber, Email = email, ArrivalDate = arrivalDate, LeavingDate = leavingDate, TotalPrice = totalPrice});
        }
    }
}
