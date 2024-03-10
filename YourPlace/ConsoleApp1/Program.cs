using System;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data.Enums;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Core.Services;
using YourPlace.Infrastructure.Data;
using YourPlace.Core.Sorting;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;

#region manage seeding layer
IdentityOptions options = new IdentityOptions();
options.Password.RequireDigit = false;
options.Password.RequireLowercase = false;
options.Password.RequireUppercase = false;
options.Password.RequireNonAlphanumeric = false;
options.Password.RequiredLength = 5;


DbContextOptionsBuilder<YourPlaceDbContext> builder = new DbContextOptionsBuilder<YourPlaceDbContext>();
builder.UseSqlServer(
    "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=YourPlaceDb;Integrated Security=False;Connect Timeout=30;Encrypt=False"
);

YourPlaceDbContext dbContext = new YourPlaceDbContext(builder.Options);

UserManager<User> userManager = new UserManager<User>(
    new UserStore<User>(dbContext), Options.Create(options),
    new PasswordHasher<User>(), new List<IUserValidator<User>>() { new UserValidator<User>() },
    new List<IPasswordValidator<User>>() { new PasswordValidator<User>() }, new UpperInvariantLookupNormalizer(),
    new IdentityErrorDescriber(), new ServiceCollection().BuildServiceProvider(),
    new Logger<UserManager<User>>(new LoggerFactory())
    );
#endregion

#region seeding
#region UserSeeding
UserServices userServices = new UserServices(dbContext, userManager);

//try
//{
//    Tuple<IdentityResult, User> result = await userServices.CreateAccountAsync("Peshka", "Peshova", "ppeshova", "ppeshova@gmail.com", "fdgdfgh", Roles.HotelManager);
//    User user = result.Item2;
//    Console.WriteLine($"{user.Id}");

//}
//catch (Exception ex)
//{
//    throw;
//}
//try
//{
//    await userServices.CreateRoleAsync(new IdentityRole(Roles.HotelManager.ToString()) { NormalizedName = "HOTELMANAGER" });
//    //await userServices.CreateRoleAsync(new IdentityRole(Roles.Traveller.ToString()) { NormalizedName = "TRAVELLER" });
//}
//catch (Exception ex)
//{
//    throw;
//}
//Tuple<IdentityResult, User> result = await userServices.LogInUserAsync("ppeshova@gmail.com", "fdgdfgh");
//Console.WriteLine(result.Item1);
#endregion

#region Hotels Seeding
HotelsServices hotelsServices = new HotelsServices(dbContext);


//Hotel hotel = new Hotel("RoyalLasVegas.jpg", "Las Vegas Royal", "Monte Carlo str.", "Las Vegas", "USA", 7.9, "Хотел Las Vegas Royal предлага всякакви по вид занимания от масажи до турнири по тенис и футбол. All-Inclisuve с включена храна и напитки");
//Console.WriteLine(hotel.ToString());

//await hotelsServices.CreateAsync(hotel);
//IEnumerable<Hotel> hotels = await hotelsServices.ReadAllAsync();
//foreach (var hotelche in hotels)
//{
//    Console.WriteLine(hotelche.ToString());
//}

//Image image = new Image("LaFleur1.jpg", 11);
//await hotelsServices.AddImages(10, "Therme4.jpg");
//List<Image> images = await hotelsServices.ShowHotelImages(8);
//Console.WriteLine(String.Join(",", images));
#endregion

#region CategoriesSeeding
HotelCategoriesServices hotelCategoriesServices = new HotelCategoriesServices(dbContext);

Categories category1 = new Categories(Location.Mountain, Tourism.Relax, Atmosphere.Calm, Company.Family, Pricing.Lux, 8);
await hotelCategoriesServices.CreateAsync(category1);

Categories category2 = new Categories(Location.Sea, Tourism.Adventure, Atmosphere.Party, Company.Group, Pricing.Modern, 9);
await hotelCategoriesServices.CreateAsync(category2);

Categories category3 = new Categories(Location.Village, Tourism.Relax, Atmosphere.Both, Company.Group, Pricing.Lux, 10);
await hotelCategoriesServices.CreateAsync(category3);

Categories category4 = new Categories(Location.LargeCity, Tourism.Shopping, Atmosphere.Neither, Company.OnePerson, Pricing.InTheMiddle, 11);
await hotelCategoriesServices.CreateAsync(category4);

Categories category5 = new Categories(Location.LargeCity, Tourism.Shopping, Atmosphere.Party, Company.Individual, Pricing.Cheap, 12);
await hotelCategoriesServices.CreateAsync(category5);
#endregion
#region Preferences
//PreferencesServices userQuestionsServices = new PreferencesServices(dbContext);
//Preferences preference = new Preferences(Location.Sea, Tourism.Relax, Atmosphere.Calm, Company.Group, Pricing.Lux);
//await userQuestionsServices.CreateAsync(preference);

//PreferencesSorting preferencesSorting = new PreferencesSorting(userQuestionsServices, hotelCategoriesServices, hotelsServices, userManager);

//await preferencesSorting.GetPreferencesCount(preference);
//List<Hotel> result = await preferencesSorting.GetPreferedHotels(preference);
//foreach(var hotelche in result)
//{
//    Console.WriteLine(hotelche.ToString());
//}
#endregion
RoomAvailabiltyServices roomAvailabilityServices = new RoomAvailabiltyServices(dbContext, hotelsServices);
Filters filters = new Filters(dbContext, hotelsServices, roomAvailabilityServices);
ReservationServices reservationServices = new ReservationServices(dbContext, hotelsServices, roomAvailabilityServices, filters);


#region CompareTotalCountWithFamilyMembersCount

//Console.WriteLine("People total count:");
//int peopleCount = int.Parse(Console.ReadLine());
//Console.WriteLine("Family count:");
//int familyCount = int.Parse(Console.ReadLine());

//List<Family> familiesForReservation = new List<Family>();

//for (int i = 0; i < familyCount; i++)
//{
//    Console.WriteLine("Enter count of family members: ");
//    int familyMembersCount = int.Parse(Console.ReadLine());
//    Family family = new Family(familyMembersCount);
//    familiesForReservation.Add(family);
//}


//    bool result;
//    result = await reservationServices.CompareTotalCountWithFamilyMembersCount(familiesForReservation, peopleCount);
//    Console.WriteLine(result);
#endregion
//FIRST METHOD TESTED: WORKS
#region Rooms seeding
RoomServices roomServices = new RoomServices(dbContext, hotelsServices);
//decimal price = 250;
//int count = 4;
//int hotelId = 8;
//int count1 = 9;
//Room room = new Room(RoomTypes.maisonette, price, count, hotelId, count1);
//await roomServices.CreateAsync(room);
//await roomAvailabilityServices.FillAvailability(9);
#endregion

#region CheckForTotalRoomAvailability
//RoomAvailabiltyServices roomAvailabiltyServices = new RoomAvailabiltyServices(dbContext);
//await roomAvailabiltyServices.FillAvailability(9);


//bool result1;
//result1 = await reservationServices.CheckForTotalRoomAvailability(9, peopleCount);

//bool result1;
//result1 = await reservationServices.CheckForTotalRoomAvailability(9, 17);
//Console.WriteLine(result1);

//WORKS
#endregion
//SECOND METHOD TESTED: WORKS

//Family family = new Family(4);
//family = await reservationServices.CreateFamily(4);

//Reservation reservation = new Reservation("Bistra", "Taneva", new DateOnly(2024, 4, 20), new DateOnly(2024, 4, 25), 2, 350, 9);
//await reservationServices.CreateAsync(reservation);

//РЕГИСТРИРАНЕ С ИМЕЙЛ

//List<Room> freeRooms = await reservationServices.FreeRoomCheck(new DateOnly(2024, 6, 20), new DateOnly(2024, 6, 25), 9);
//foreach(var room in freeRooms)
//{
//    Console.WriteLine(room.ToString());
//}
//List<Hotel> hotels = await filters.FilterByCountry("France");
//foreach(var hotel in hotels)
//{
//    Console.WriteLine(hotel.ToString());
//}


//List<Room> availableRooms = new List<Room>();
//List<Tuple<int, RoomTypes>> countRooms = new List<Tuple<int, RoomTypes>>();
//List<Room> rooms = new List<Room>();

//if (result == false)
//{
//    ViewBag.Message = "There are no places for so many people in this hotel!";
//    Console.WriteLine("There are no places for so many people!");

//}
//else
//{

//Reservation reservation = new Reservation("Olimpiicho", "Olimpiev", new DateOnly(2024, 4, 20), new DateOnly(2024, 4, 25), 3, 400, 9, 5);
//await reservationServices.CreateAsync(reservation);

//DateOnly arrivalDate = new DateOnly(2024, 2, 23);
//DateOnly leavingDate = new DateOnly(2024, 2, 24);
//availableRooms = await reservationServices.FreeRoomCheck(arrivalDate, leavingDate, 9);
////countRooms = await reservationServices.FreeRoomsAccordingToTypeAsync(arrivalDate, leavingDate, availableRooms);

////rooms = await reservationServices.GetRoomsByTypes(9, countRooms);

//foreach (var room in availableRooms)
//{
//    Console.WriteLine(room.ToString());
//}
#endregion

#region testing
List<Room> roomList = await roomServices.GetAllRoomsInHotel(9);
foreach(Room room1 in roomList)
{
    Console.WriteLine(room1.ToString());
}
#endregion