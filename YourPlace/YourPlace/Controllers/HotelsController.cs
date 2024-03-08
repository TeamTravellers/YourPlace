using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YourPlace.Core.Services;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Models;

namespace YourPlace.Controllers
{
    public class HotelsController : Controller
    {
        private readonly HotelsServices _hotelServices;
        public HotelsController(HotelsServices hotelServices)
        {
            _hotelServices = hotelServices;
        }

        private const string toOffer = "~/Views/Bulgarian/Hotels/Offer.cshtml";

        // GET: HotelController
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: HotelController/Offer/5
        //binds offer id with the hotel so it can display the correct hotel data 
        public async Task<IActionResult> Offer([Bind("Id")] int hotelID) 
        {
            Hotel hotel = await _hotelServices.ReadAsync(hotelID);
            List<Image> images = await _hotelServices.ShowHotelImages(hotelID);
            return View(toOffer, new AllHotelsModel { HotelModel = hotel, HotelImages = images});
        }

        // GET: HotelController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HotelController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HotelController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HotelController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HotelController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HotelController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
