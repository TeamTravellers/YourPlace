using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Core.Contracts;
using YourPlace.Core.Services;
using YourPlace.Infrastructure.Data;
using YourPlace.Infrastructure.Data.Entities;

namespace YourPlace.Core.Sorting
{
    public class Filters : IFilter
    {
        private readonly YourPlaceDbContext _dbContext;
        private readonly HotelsServices _hotelsServices;
        private readonly RoomAvailabiltyServices _roomAvailabiltyServices;
        private readonly ReservationServices _reservationsServices;
        private readonly RoomServices _roomServices;
        public Filters(YourPlaceDbContext dbContext, HotelsServices hotelsServices, RoomAvailabiltyServices roomAvailabiltyServices, ReservationServices reservationsServices, RoomServices roomServices)
        {
            _dbContext = dbContext;
            _hotelsServices = hotelsServices;
            _roomAvailabiltyServices = roomAvailabiltyServices;
            _reservationsServices = reservationsServices;
            _roomServices = roomServices;
        }

        public async Task<List<Hotel>> ApplyFilters(string country, int peopleCount, decimal price, DateOnly arrivalDate, DateOnly leavingDate)
        {
            List<Hotel> filteredByCountry = new List<Hotel>();
            List<Hotel> filteredByPeopleCount = new List<Hotel>();
            List<Hotel> filteredByPrice = new List<Hotel>();
            List<Hotel> filteredByDates = new List<Hotel>();

            List<Hotel> hotels = new List<Hotel>();
            List<List<Hotel>> allFiltered = new List<List<Hotel>>(); //ще добавяме всички филтрирани списъци от хотели, които не са празни
            List<Hotel> resultList = new List<Hotel>();

            if (country != null)
            {
                filteredByCountry = await FilterByCountry(country);
                foreach (Hotel hotel in filteredByCountry)
                {
                    if (!hotels.Any(x => x.HotelID == hotel.HotelID))
                    {
                        hotels.Add(hotel);
                    }
                }
                allFiltered.Add(filteredByCountry);
            }
            if (peopleCount != 0)
            {
                filteredByPeopleCount = await FilterByPeopleCount(peopleCount);
                foreach (Hotel hotel in filteredByPeopleCount)
                {
                    if (!hotels.Any(x => x.HotelID == hotel.HotelID))
                    {
                        hotels.Add(hotel);
                    }
                }
                allFiltered.Add(filteredByPeopleCount);
            }
            if (price != 0)
            {
                filteredByPrice = await FilterByPrice(price);
                foreach (Hotel hotel in filteredByPrice)
                {
                    if (!hotels.Any(x => x.HotelID == hotel.HotelID))
                    {
                        hotels.Add(hotel);
                    }
                }
                allFiltered.Add(filteredByPrice);
            }
            if (arrivalDate != null && leavingDate != null)
            {
                filteredByDates = await FilterByDates(arrivalDate, leavingDate);
                foreach (Hotel hotel in filteredByDates)
                {
                    if (!hotels.Any(x => x.HotelID == hotel.HotelID))
                    {
                        hotels.Add(hotel);
                    }
                }
                allFiltered.Add(filteredByDates);
            }

            hotels = hotels.Distinct().ToList(); //това представлява обединение от резултатите, дадени от всеки един филтър
            resultList = await HotelIntersection(hotels, allFiltered);
            return resultList;
            
        }
        /// <summary>
        /// методът Intersect не би работил за сложни обекти като Hotel;
        /// създаваме наш метод за намиране на съединението от филтрираните хотели
        /// </summary>
        /// <param name="union"></param>
        /// <returns></returns>
        public async Task<List<Hotel>> HotelIntersection(List<Hotel> union, List<List<Hotel>> allFiltered) 
        {
            List<Hotel> intersection = new List<Hotel>();

            foreach(Hotel hotel in union)
            {
                int count = 0; //като стойност на тази променлива ще проверяваме колко пъти се среща хотелът във всеки един от филтрираните списъци
                foreach (var list in allFiltered)
                {
                    if(list.Any(x=>x.HotelID == hotel.HotelID))
                    {
                        count++;
                    }
                }
                //ако хотелът се среща във всички филтрирани списъци, които не са празни, то тогава той ще бъде добавен към крайният списък,
                //защото ще отговаря на всички условия, зададени от потребителя

                if (count == allFiltered.Count) 
                {
                    intersection.Add(hotel);
                }
            }
            return intersection;
        }
        public async Task<List<Hotel>> FilterByCountry(string country)
        {
            List<Hotel> hotels = await _hotelsServices.ReadAllAsync();
            var resultList = hotels.Where(x => x.Country == country).ToList();
            return resultList;
        }
        public async Task<List<Hotel>> FilterByPeopleCount(int count)
        {
            Console.WriteLine(count);
            var hotels = await _hotelsServices.ReadAllAsync();
            var filteredHotels = new List<Hotel>();
            foreach (var hotel in hotels)
            {
                int maxCount = await _roomAvailabiltyServices.GetMaxCountOfPeopleInHotel(hotel.HotelID);
                if (maxCount >= count)
                {
                    filteredHotels.Add(hotel);
                }
            }
            return filteredHotels;
        }

        public async Task<List<Hotel>> FilterByPrice(decimal price)
        {
            var rooms = await _dbContext.Rooms.Where(x => x.Price <= price).ToListAsync();
            var resultList = new List<Hotel>();

            foreach (var room in rooms)
            {
                Hotel hotel =  _dbContext.Hotels.FirstOrDefault(x => x.HotelID == room.HotelID);
                if (!resultList.Any(x => x.HotelID == hotel.HotelID))
                {
                    resultList.Add(hotel);
                }
            }
            return resultList;
        }
        public async Task<List<Hotel>> FilterByDates(DateOnly arrivalDate, DateOnly leavingDate)
        {
         
           List<Hotel> filteredHotels = await HotelsWithFreeRooms(arrivalDate, leavingDate);
           return filteredHotels;
        }
       
        public async Task<List<Tuple<Room, int>>> FindAllFreeRooms(DateOnly arrivalDate, DateOnly leavingDate) //намираме всички свободни стаи
        {
            List<ReservedRoom> reservedRooms = await _reservationsServices.ReadAllReservedRoomsAsync();

            List<Tuple<Room, int>> resultList = new List<Tuple<Room, int>>();

            foreach (var reservedRoom in reservedRooms)
            {
                Room room = await _roomServices.ReadAsync(reservedRoom.RoomID);

                Reservation reservation = await _reservationsServices.ReadAsync(reservedRoom.ReservationID);
                int roomCountInHotel = room.CountInHotel;

                if (arrivalDate > reservation.LeavingDate || leavingDate < reservation.ArrivalDate)
                {
                    Tuple<Room, int> roomCount = Tuple.Create(room, room.CountInHotel);
                    if (!resultList.Contains(roomCount))
                    {
                        resultList.Add(roomCount);
                    }
                }
                else
                if (leavingDate >= reservation.ArrivalDate && leavingDate <= reservation.LeavingDate)
                {
                    int freeRoomsCount = room.CountInHotel - reservedRoom.Count;
                    if (freeRoomsCount > 0)
                    {
                        Tuple<Room, int> roomCount = Tuple.Create(room, freeRoomsCount);
                        if (!resultList.Contains(roomCount))
                        {
                            resultList.Add(roomCount);
                        }
                    }
                }
                else
                if (reservation.ArrivalDate >= arrivalDate && reservation.LeavingDate <= leavingDate || arrivalDate >= reservation.ArrivalDate && arrivalDate <= reservation.LeavingDate)
                {
                    int freeRoomsCount = room.CountInHotel - reservedRoom.Count;
                    if (freeRoomsCount > 0)
                    {
                        Tuple<Room, int> roomCount = Tuple.Create(room, freeRoomsCount);
                        if (!resultList.Contains(roomCount))
                        {
                            resultList.Add(roomCount);
                        }
                    }
                }
            }
            //resultList = resultList.Distinct().ToList();
            var finalRoomList = await _reservationsServices.CheckCount(resultList);

            return finalRoomList;
        }
        public async Task<List<Hotel>> HotelsWithFreeRooms(DateOnly arrivalDate, DateOnly leavingDate) //намираме хотелите към всички свободни стаи
        {
            List<Tuple<Room, int>> roomList = await FindAllFreeRooms(arrivalDate, leavingDate);
            List<Hotel> filteredHotels = new List<Hotel>();
            foreach(var room in roomList)
            {
                Hotel hotel = await _hotelsServices.ReadAsync(room.Item1.HotelID);
                if (!filteredHotels.Any(x => x.HotelID == hotel.HotelID))
                {
                    filteredHotels.Add(hotel);
                }
            }
            return filteredHotels;
        }
    }
}
