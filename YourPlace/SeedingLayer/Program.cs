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

try
{
    Tuple<IdentityResult, User> result = await userServices.CreateAccountAsync("Maria", "Metrova", "mpetrova", "mimip@gmail.com", "554gdghd", Roles.Traveller);
    User user = result.Item2;
    Console.WriteLine($"{user.Id}");

}
catch (Exception ex)
{
    throw;
}
//try
//{
//    await userServices.CreateRoleAsync(new IdentityRole(Roles.HotelManager.ToString()) { NormalizedName = "HOTEL MANAGER" });
//    await userServices.CreateRoleAsync(new IdentityRole(Roles.Traveller.ToString()) { NormalizedName = "TRAVELLER" });
//}
//catch(Exception ex)
//{
//    throw;
//}






