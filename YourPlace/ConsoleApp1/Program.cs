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
HotelsServices hotelsServices = new HotelsServices(dbContext);
//Console.WriteLine("imageURL");
//string imageURL = Console.ReadLine();
//Console.WriteLine("name");
//string name = Console.ReadLine();
//Console.WriteLine("address");
//string address = Console.ReadLine();
//Console.WriteLine("town");
//string town = Console.ReadLine();
//Console.WriteLine("country");
//string country = Console.ReadLine();
//Console.WriteLine("rating");
//double rating = double.Parse(Console.ReadLine());
//Console.WriteLine("details");
//string details = Console.ReadLine();

//Hotel hotel = new Hotel("RoseGarden.jpg", "Rose Garden", "ул. Ропотамо 12", "Поморие", "България", 8.5, "Апартаменти Роуз Гардънс се намират на 50 метра от плажа. Включват сезонен външен басейн и сезонен ресторант, безплатен Wi-Fi и сезонен спа център.");
//Console.WriteLine(hotel.ToString());

//await hotelsServices.CreateAsync(hotel);
//IEnumerable<Hotel> hotels = await hotelsServices.ReadAllAsync();
//foreach (var hotelche in hotels)
//{
//    Console.WriteLine(hotelche.ToString());
//}

//Image image = new Image("Arte1.jpg", 8);
//await hotelsServices.AddImages(9, "RoseGarden4.jpg");
//List<Image> images =  await hotelsServices.ShowHotelImages(8);
//Console.WriteLine(String.Join(",", images));

HotelCategoriesServices hotelCategoriesServices = new HotelCategoriesServices(dbContext);

//Categories category1 = new Categories(Location.LargeCity, Tourism.Culture, Atmosphere.Neither, Company.OnePerson, Pricing.Modern, 1);
//await hotelCategoriesServices.CreateAsync(category1);

//Categories category2 = new Categories(Location.Village, Tourism.Adventure, Atmosphere.Calm, Company.Group, Pricing.Modern, 4);
//await hotelCategoriesServices.CreateAsync(category2);

//Categories category3 = new Categories(Location.Sea, Tourism.Relax, Atmosphere.Party, Company.Group, Pricing.Lux, 5);
//await hotelCategoriesServices.CreateAsync(category3);

//Categories category4 = new Categories(Location.Sea, Tourism.Relax, Atmosphere.Calm, Company.Group, Pricing.InTheMiddle, 6);
//await hotelCategoriesServices.CreateAsync(category4);

//Categories category5 = new Categories(Location.Mountain, Tourism.Relax, Atmosphere.Calm, Company.Family, Pricing.Lux, 7);
//await hotelCategoriesServices.CreateAsync(category5);

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
//RoomServices roomServices = new RoomServices(dbContext);
//Room room = new Room(RoomTypes.maisonette, (decimal)200, 6, 3, 9);
//await roomServices.CreateAsync(room);
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
List<Hotel> hotels = await filters.FilterByCountry("France");
foreach(var hotel in hotels)
{
    Console.WriteLine(hotel.ToString());
}
