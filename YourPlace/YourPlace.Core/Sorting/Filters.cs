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
       
        public async Task<List<Tuple<Room, int>>> FindAllFreeRooms(DateOnly arrivalDate, DateOnly leavingDate)
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
        public async Task<List<Hotel>> HotelsWithFreeRooms(DateOnly arrivalDate, DateOnly leavingDate)
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
