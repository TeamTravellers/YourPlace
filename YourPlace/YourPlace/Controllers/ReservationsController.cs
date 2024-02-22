using Microsoft.AspNetCore.Mvc;
using YourPlace.Core.Services;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Models;

namespace YourPlace.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ReservationServices _reservationServices;
        private readonly HotelsServices _hotelsServices;

        public ReservationsController(ReservationServices reservationServices, HotelsServices hotelsServices)
        {
            _reservationServices = reservationServices;
            _hotelsServices = hotelsServices;

        }
        private const string toReservation = "~/Views/Bulgarian/Hotels/Reservation.cshtml";
        public async Task<IActionResult> IndexAsync([Bind("Id")] int hotelID)
        {
            Hotel hotel = await _hotelsServices.ReadAsync(hotelID);
            List<Image> images = await _hotelsServices.ShowHotelImages(hotelID);
            return View(toReservation, new AllHotelsModel { HotelModel = hotel, HotelImages = images });
        }

    }
}
