﻿using System;
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

namespace YourPlace.Core.Services
{
    public class ReservationServices : IReservation, IDbCRUD<Reservation, int>
    {
        private readonly YourPlaceDbContext _dbContext;
        private readonly HotelsServices _hotelsServices;
        private readonly RoomAvailabiltyServices _roomAvailabiltyServices;
        private readonly Filters _filters;

        private readonly List<Family> CreatedFamilies = new List<Family>();
        public ReservationServices(YourPlaceDbContext dbContext, HotelsServices hotelsServices, RoomAvailabiltyServices roomAvailabiltyServices, Filters filters)
        {
            _dbContext = dbContext;
            _hotelsServices = hotelsServices;
            _roomAvailabiltyServices = roomAvailabiltyServices;
            _filters = filters;
        }
        #region CRUD For Reservations
        public async Task CreateAsync(Reservation item)
        {
            try
            {
                _dbContext.Add(item);
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


        #region old code
        public async Task<bool> CompareTotalCountWithFamilyMembersCount(List<Family> families, int totalCount) //something like js function
        {
            try
            {
                bool success = false;
                int totalCountInFamilies = 0;
                foreach (var family in families)
                {
                    totalCountInFamilies += family.TotalCount;
                }
                if (totalCountInFamilies != totalCount)
                {
                    throw new Exception("The total number of people does not match the number of all members in the families!");
                    success = false;
                }
                else
                {
                    success = true;
                }
                return success;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CheckForTotalRoomAvailability(int hotelID, int peopleCount) // == filters
        {
            try
            {
                bool result = false;
                Hotel hotel = await _hotelsServices.ReadAsync(hotelID);
                int maxCountOfPeopleInHotel = await _roomAvailabiltyServices.GetMaxCountOfPeopleInHotel(hotelID);
                if(maxCountOfPeopleInHotel >= peopleCount)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
                return result;
            }
            catch(Exception)
            {
                throw;
            }
        }
        public async Task<List<Room>> FreeRoomCheck(DateOnly arrivalDate, DateOnly leavingDate, int hotelID) // == filters
        {
            try
            {
                //List<Hotel> hotelsWithFreeRooms = await _filters.FilterByDates(arrivalDate, leavingDate);
                //Hotel hotel = await _hotelsServices.ReadAsync(hotelID);
                //List<Room> roomsInHotel = await _roomAvailabiltyServices.GetAllRoomsInHotel(hotelID);
                //List<Room> freeRoomsInHotel = new List<Room>();
                List<Room> freeRoomsInHotel = new List<Room>();
                List<Room> roomsInHotel = await _roomAvailabiltyServices.GetAllRoomsInHotel(hotelID);
                List<Reservation> reservationsForHotel = await FindReservationsForHotel(hotelID);
                List<Room> availableRooms = new List<Room>();
                List<int> reservedRoomsIDs = new List<int>();
                foreach (var reservation in reservationsForHotel)
                {
                    reservedRoomsIDs.Add(reservation.ReservationID);
                    if (leavingDate <= reservation.ArrivalDate && arrivalDate < leavingDate || arrivalDate >= reservation.LeavingDate && leavingDate > arrivalDate)
                    {
                        //reservedRoomsIDs.Add(reservation.RoomID);
                        //foreach(var room in roomsInHotel)
                        //{
                        freeRoomsInHotel = roomsInHotel.Where(x=>x.RoomID != reservation.RoomID).ToList();
                            
                        //}

                    }
                }
                availableRooms.AddRange(freeRoomsInHotel);


                //if (hotelsWithFreeRooms.Contains(hotel))
                //{
                //    foreach (var room in roomsInHotel)
                //    {
                //        freeRoomsInHotel.Add(room);
                //    }

                //}
                return availableRooms;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CompleteReservationAsync(string firstName, string surname, DateOnly arrivalDate, DateOnly leavingDate, int peopleCount, int hotelID, int roomID)
        {
            bool success;
            try
            {
                await CreateAsync(new Reservation(firstName, surname, arrivalDate, leavingDate, peopleCount, hotelID, roomID));
                success = true;

            }
            catch
            {
                success = false;
            }
            return success;
        }
        public async Task<List<Tuple<int, RoomTypes>>> FreeRoomsAccordingToTypeAsync(DateOnly arrivalDate, DateOnly leavingDate, List<Room> freeRooms)
        {
            try
            {
                //Hotel hotel = await _hotelsServices.ReadAsync(hotelID);
                //List<Room> freeRooms = await FreeRoomCheck(arrivalDate, leavingDate, hotelID);
                List<RoomTypes> roomTypesForHotel = new List<RoomTypes>();
                List<Room> roomsFromOneType = new List<Room>();
                Tuple<int, RoomTypes> countRoom;
                List<Tuple<int, RoomTypes>> result = new List<Tuple<int, RoomTypes>>();

                int count = 0;
                //List<Room> roomsInHotel = freeRooms.Where(x => x.HotelID == hotelID).ToList();
                foreach (Room room in freeRooms)
                {
                    roomTypesForHotel.Add(room.Type);
                    roomTypesForHotel = roomTypesForHotel.Distinct().ToList();
                }
                foreach (var type in roomTypesForHotel)
                {
                    count = 0;
                    roomsFromOneType = freeRooms.Where(x => x.Type == type).ToList();
                    count = roomsFromOneType.Count;
                    //countRooms.Add(count, type);

                    countRoom = Tuple.Create(count, type);
                    result.Add(countRoom);
                }

                return result;
                //if (count == 0)
                //{
                //    throw new Exception($"No free rooms for this number of people!");
                //}
                //else
                //{
                //    return count;
                //}
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Room>> GetRoomsByTypes(int hotelID, List<Tuple<int, RoomTypes>> countType)
        {
            List<Room> roomsInHotel = await _roomAvailabiltyServices.GetAllRoomsInHotel(hotelID);
            List<Room> roomsByType = new List<Room>();
            List<Room> result = new List<Room>();
            foreach(var tuple in countType)
            {
                  roomsByType = roomsInHotel.Where(x => x.Type == tuple.Item2).ToList();
                  result.AddRange(roomsByType);
            }
            return result;
        }


        public async Task<Family> CreateFamily(int totalCount)
        {
            try
            {
                Family newFamily = new Family(totalCount);
                CreatedFamilies.Add(newFamily);
                return newFamily;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<Room>> FreeRoomsAccordingToPeopleCount(DateOnly arrivalDate, DateOnly leavingDate, int peopleCount, int hotelID)
        {
            try
            {
                Hotel hotel = await _hotelsServices.ReadAsync(hotelID);
                RoomTypes roomType = RoomTypesHelper.GetRoomTypeForPeopleCount(peopleCount);
                List<Room> freeRooms = await FreeRoomCheck(arrivalDate, leavingDate, hotelID);
                List<Room> appropriateFreeRooms = freeRooms.Where(x => x.Type.ToString().ToLower() == roomType.ToString().ToLower()).ToList();
                return appropriateFreeRooms.ToList();
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        
        public async Task<int> CountOfFreeRoomsAccordingToType(int hotelID, Family family, DateOnly arrivalDate, DateOnly leavingDate)
        {
            try
            {
                Hotel hotel = await _hotelsServices.ReadAsync(hotelID);
                List<Room> freeRooms = await FreeRoomsAccordingToPeopleCount(arrivalDate, leavingDate, family.TotalCount, hotelID);
                //List<Room> roomsInHotel = freeRooms.Where(x => x.HotelID == hotelID).ToList();
                int count = freeRooms.Where(x => x.MaxPeopleCount == family.TotalCount).Count();
                
                if (count == 0)
                {
                    throw new Exception($"No free rooms for this number of people!");
                }
                else
                {
                    return count;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }    
        public async Task<Room> AccomodateFamily(int hotelID, Family family, DateOnly arrivalDate, DateOnly leavingDate)
        {
            Room room = new Room();
            List<Room> freeRooms = await FreeRoomsAccordingToPeopleCount(arrivalDate, leavingDate, family.TotalCount, hotelID);
            if (freeRooms is null)
            {
                throw new Exception("There are no free rooms for this family!");
            }
            else
            {
                int count = await CountOfFreeRoomsAccordingToType(hotelID, family, arrivalDate, leavingDate);
                List<int> roomsIDs = freeRooms.Select(x => x.RoomID).ToList();
                Random random = new Random();
                int randomIndex = random.Next(roomsIDs.Count);
                int finalRoomID = roomsIDs[randomIndex];
                room = await _dbContext.Rooms.FindAsync(finalRoomID);
            }
            return room;
        }
        /// <summary>
        /// //DO NOT FORGET TO CREATE FAMILY FIRST EVEN IF THE NUMBER OF PEOPLE IS SMALLER THAN OR EQUALS TO THE MAX CAPACITY OF THE BIGGEST ROOM IN A HOTEL
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="surname"></param>
        /// <param name="arrivalDate"></param>
        /// <param name="leavingDate"></param>
        /// <param name="peopleCount"></param>
        /// <param name="price"></param>
        /// <param name="hotelID"></param>
        /// <param name="familyCount"></param>
        /// <returns></returns>
        public async Task<decimal> CalculatePrices(List<Room> reservedRooms)
        {
            try
            {
                decimal totalPrice = 0;
                foreach (Room room in reservedRooms)
                {
                    totalPrice += room.Price;
                }
                return totalPrice;
            }
            catch (Exception)
            {
                throw;
            }
           
        }
        public async Task<bool> CompleteReservation(string firstName, string surname, DateOnly arrivalDate, DateOnly leavingDate, int peopleCount, decimal price, int hotelID, int familyCount)
        {
            bool success;
            try
            {
                CheckForTotalRoomAvailability(hotelID, peopleCount);
                CompareTotalCountWithFamilyMembersCount(CreatedFamilies, peopleCount); //more like js function
                List<Room> currentlyReservedRooms = new List<Room>();
                foreach (Family family in CreatedFamilies)
                {
                    //CreateFamily(peopleCount, family);
                    Room room = await AccomodateFamily(hotelID, family, arrivalDate, leavingDate);
                    currentlyReservedRooms.Add(room);
                }
                decimal totalPrice = await CalculatePrices(currentlyReservedRooms);
                //CreateAsync(new Reservation(firstName, surname, arrivalDate, leavingDate, peopleCount, totalPrice, hotelID, currentlyReservedRooms, CreatedFamilies));
                success = true;

            }
            catch
            {
                success = false;
            }
            return success;
        }
        #endregion

        #region new code in progress
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

        //GETALLROOMSINHOTEL();
        //public async Task<List<Room>> FreeRoomsCheck(int hotelID, DateOnly arrivalDate, DateOnly leavingDate)
        //{
        //    List<Reservation> reservationsForHotel = await FindReservationsForHotel(hotelID);
        //    foreach(Reservation reservation in reservationsForHotel)
        //    {
        //        if(leavingDate < reservation.ArrivalDate || arrivalDate > reservation.LeavingDate)
        //        {

        //        }
        //    }
        //}
        #endregion
    }
}

