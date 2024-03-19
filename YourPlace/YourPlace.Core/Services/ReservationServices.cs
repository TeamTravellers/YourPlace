using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Core.Contracts;
using System.ComponentModel;
using YourPlace.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Diagnostics;
using Microsoft.Identity.Client.Extensibility;
using YourPlace.Core.Sorting;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Mvc;
using NuGet.Versioning;

namespace YourPlace.Core.Services
{
    public class ReservationServices : IReservation, IDbCRUD<Reservation, int>
    {
        private readonly YourPlaceDbContext _dbContext;
        private readonly HotelsServices _hotelsServices;
        private readonly RoomServices _roomServices;
        private readonly RoomAvailabiltyServices _roomAvailabiltyServices;
        private readonly Filters _filters;

        public ReservationServices(YourPlaceDbContext dbContext, HotelsServices hotelsServices, RoomServices roomServices, RoomAvailabiltyServices roomAvailabiltyServices, Filters filters)
        {
            _dbContext = dbContext;
            _hotelsServices = hotelsServices;
            _roomServices = roomServices;
            _roomAvailabiltyServices = roomAvailabiltyServices;
            _filters = filters;
        }
        #region CRUD For Reservations
        public async Task CreateAsync(Reservation reservation)
        {
            try
            {
                _dbContext.Reservations.Add(reservation);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Reservation> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Reservation> reservations = _dbContext.Reservations;
                if (useNavigationalProperties)
                {
                    reservations = reservations.Include(x => x.Hotel);
                }
                if (isReadOnly)
                {
                    reservations.AsNoTrackingWithIdentityResolution();
                }
                return await reservations.SingleOrDefaultAsync(x => x.ReservationID == key);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Reservation>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Reservation> reservations = _dbContext.Reservations;
                if (isReadOnly)
                {
                    reservations.AsNoTrackingWithIdentityResolution();
                }
                return await reservations.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public async Task UpdateAsync(Reservation item)
        {
            try
            {
                _dbContext.Update(item);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAsync(int key)
        {
            try
            {
                Reservation reservation = await ReadAsync(key);
                if (reservation == null)
                {
                    throw new ArgumentException(string.Format($"Computer with id {key} does " +
                        $"not exist in the database!"));
                }
                _dbContext.Remove(reservation);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region CRUD For Reserved Rooms
        public async Task CreateReservedRoomAsync(ReservedRoom reserved)
        {
            try
            {
                _dbContext.ReservedRooms.Add(reserved);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ReservedRoom> ReadReservedRoomAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<ReservedRoom> reservedRooms = _dbContext.ReservedRooms;
                if (useNavigationalProperties)
                {
                    reservedRooms = reservedRooms.Include(x => x.Reservation);
                    reservedRooms = reservedRooms.Include(x => x.Room);
                }
                if (isReadOnly)
                {
                    reservedRooms.AsNoTrackingWithIdentityResolution();
                }
                return await reservedRooms.SingleOrDefaultAsync(x => x.ID == key);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ReservedRoom>> ReadAllReservedRoomsAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<ReservedRoom> reservedRooms = _dbContext.ReservedRooms;
                if (isReadOnly)
                {
                    reservedRooms.AsNoTrackingWithIdentityResolution();
                }
                return await reservedRooms.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateReservedRoomAsync(ReservedRoom reserved)
        {
            try
            {
                _dbContext.Update(reserved);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteReservedRoomAsync(int key)
        {
            try
            {
                ReservedRoom reservedRoom = await ReadReservedRoomAsync(key);
                if (reservedRoom == null)
                {
                    throw new ArgumentException(string.Format($"Reservation with id {reservedRoom.ReservationID} does " +
                        $"not exist in the database!"));
                }
                _dbContext.Remove(reservedRoom);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Logic for reservations
        public async Task<List<Reservation>> FindReservationsForHotel(int hotelID)
        {
            try
            {
                List<Reservation> reservations = await ReadAllAsync();
                List<Reservation> reservationsForHotel = reservations.Where(x => x.HotelID == hotelID).ToList();
                return reservationsForHotel;
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<ReservedRoom>> FindReservedRoomsForHotel(int hotelID)
        {
            try
            {
                List<ReservedRoom> reservedRooms = await ReadAllReservedRoomsAsync();
                List<ReservedRoom> reservedRoomsInHotel = reservedRooms.Where(x => x.HotelID == hotelID).ToList();
                return reservedRoomsInHotel;
            }
            catch
            {
                throw;
            }
        }
        //GETALLROOMSINHOTEL();
        //THE LIST RESERVED ROOMS IN RESERVATION to be filled when creating a reservation
        public async Task<List<Tuple<Room,int>>> FindFreeRooms(int hotelID, DateOnly arrivalDate, DateOnly leavingDate)
        {
            Hotel hotel = await _hotelsServices.ReadAsync(hotelID);
            List<Reservation> reservationsForHotel = await FindReservationsForHotel(hotelID);
            //List<RoomAvailability> roomsAvailability = await _roomAvailabiltyServices.ReadAsync(hotelID);
            List<ReservedRoom> reservedRooms = await FindReservedRoomsForHotel(hotelID);
            List<Room> busyRooms = new List<Room>();

            List<Tuple<Room, int>> resultList = new List<Tuple<Room, int>>();
            int count = 0;
            
            foreach(var reservedRoom in reservedRooms)
            {
                Room room = await _roomServices.ReadAsync(reservedRoom.RoomID);
                busyRooms.Add(room);
            }
            foreach(var reservedRoom in reservedRooms)
            {
                var currentRoomType = busyRooms.Where(x => x.RoomID == reservedRoom.RoomID);
                count += reservedRoom.Count;
            }
            foreach (var reservedRoom in reservedRooms)
            {
                Room room = await _roomServices.ReadAsync(reservedRoom.RoomID);
                
                Reservation reservation = await ReadAsync(reservedRoom.ReservationID);
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
                if(leavingDate >= reservation.ArrivalDate && leavingDate <= reservation.LeavingDate)
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
                if(reservation.ArrivalDate >= arrivalDate && reservation.LeavingDate <= leavingDate || arrivalDate >= reservation.ArrivalDate && arrivalDate <= reservation.LeavingDate)
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
            var finalRoomList = await CheckCount(resultList);

            return finalRoomList;
        }
        public async Task<List<Tuple<Room, int>>> CheckCount(List<Tuple<Room,int>> resultList)
        {
            List<Tuple<Room, int>> finalFreeRoomList = new List<Tuple<Room, int>>();
            foreach (var tuple in resultList)
            {
                var roomsFromOneType = resultList.Where(x => x.Item1.RoomID == tuple.Item1.RoomID).ToList();
                int freeRoomsCount = roomsFromOneType.Min(x => x.Item2);
                Tuple<Room, int> roomCount = Tuple.Create(tuple.Item1, freeRoomsCount);
                if (!finalFreeRoomList.Any(x => x.Item1.RoomID == roomCount.Item1.RoomID && x.Item2 == roomCount.Item2))
                {
                    finalFreeRoomList.Add(roomCount);
                }

            }
            return finalFreeRoomList;

        }
        public async Task<decimal> CalculatePrice(DateOnly arrivalDate, DateOnly leavingDate, List<RoomSelection> selectedRooms)
        {
            decimal total = 0;
            int daysDifference = leavingDate.DayNumber - arrivalDate.DayNumber; 
            //dayNumber is great; calculates the number of the day since the first year; it gives exact results!!!

            foreach (var roomSelection in selectedRooms)
            {
                Room selectedRoom = await _roomServices.ReadAsync(roomSelection.RoomID);
                total += daysDifference * selectedRoom.Price * roomSelection.ChosenCount;
            }
            return total;
        }
        public async Task Reserve(string firstName, string surname, int peopleCount, DateOnly arrivalDate, DateOnly leavingDate, decimal totalPrice, int hotelID, List<RoomSelection> chosenRooms)
        {
            Reservation reservation = new Reservation(firstName, surname, arrivalDate, leavingDate, totalPrice, peopleCount, hotelID, chosenRooms);
            await CreateAsync(reservation);
            List<Reservation> reservations = new List<Reservation>();
            int currentReservationID = reservations.FirstOrDefault(reservation).ReservationID;
            foreach (var selectedRoom in chosenRooms)
            {
                if(selectedRoom.ChosenCount > 0)
                {
                    ReservedRoom reservedRoom = new ReservedRoom(currentReservationID, selectedRoom.RoomID, selectedRoom.ChosenCount, hotelID);
                    Console.WriteLine($"RESERVATIONID: {reservedRoom.ReservationID} | ROOM ID: {reservedRoom.RoomID} | CHOSENCOUNT: {reservedRoom.Count} | HOTELID: {reservedRoom.HotelID}");
                    await CreateReservedRoomAsync(reservedRoom);
                }
            }

        }
        #endregion
    }
}

