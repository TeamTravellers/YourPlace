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
        public IActionResult Index()
        {

            return View();
        }
    }
}
