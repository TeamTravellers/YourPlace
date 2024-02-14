using Microsoft.AspNetCore.Mvc;
using YourPlace.Core.Services;

namespace YourPlace.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ReservationServices _reservationServices;

        public ReservationsController(ReservationServices reservationServices)
        {
            _reservationServices = reservationServices;
        }
        private const string toReservation = "~/Views/Bulgarian/Hotels/Reservation.cshtml";
        public IActionResult Index()
        {

            return View(toReservation);
        }
    }
}
