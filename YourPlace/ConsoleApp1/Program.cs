using System;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data.Enums;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Core.Services;
using YourPlace.Infrastructure.Data;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;

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
Console.WriteLine("imageURL");
string imageURL = Console.ReadLine();
Console.WriteLine("name");
string name = Console.ReadLine();
Console.WriteLine("address");
string address = Console.ReadLine();
Console.WriteLine("town");
string town = Console.ReadLine();
Console.WriteLine("country");
string country = Console.ReadLine();
Console.WriteLine("rating");
double rating = double.Parse(Console.ReadLine());
Console.WriteLine("details");
string details = Console.ReadLine();

Hotel hotel = new Hotel(imageURL, name, address, town, country, rating, details);
Console.WriteLine(hotel.ToString());

await hotelsServices.CreateAsync(hotel);
IEnumerable<Hotel> hotels = await hotelsServices.ReadAllAsync();
foreach (var hotelche in hotels)
{
    Console.WriteLine(hotelche.ToString());
}