using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YourPlace.Infrastructure.Data;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using YourPlace.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity.UI.Services;
using YourPlace.Core.Services;
using YourPlace.Core.Sorting;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<YourPlaceDbContext>(options => options.UseSqlServer(connectionString));


        builder.Services.AddAuthentication()
          .AddGoogle(options =>
          {
              options.ClientId = "657954048976-psj504rnils7e6isbss9jr6up525ssdl.apps.googleusercontent.com";
              options.ClientSecret = "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";
              options.CallbackPath = "/signin-google";
          })
          .AddFacebook(options =>
           {
            options.AppId = "1619903765518790";
            options.AppSecret = "ce36c91361dae351b528482a95f906ad";
            
          });


        builder.Services.AddScoped<UserServices, UserServices>();
        //builder.Services.AddScoped<IEmailSender, EmailSender>();
        builder.Services.AddScoped<HotelsServices>();
        builder.Services.AddScoped<RoomServices>();
        builder.Services.AddScoped<HotelCategoriesServices>();
        builder.Services.AddScoped<PreferencesServices>();
        builder.Services.AddScoped<ReservationServices>();
        builder.Services.AddScoped<RoomAvailabiltyServices>();
        builder.Services.AddScoped<Filters>();
        builder.Services.AddScoped<PreferencesSorting>();
        



        builder.Services.AddIdentity<User, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            options.Lockout = new LockoutOptions { AllowedForNewUsers = true, DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10), MaxFailedAccessAttempts = 5 };
        })
                .AddEntityFrameworkStores<YourPlaceDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();
        //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<YourPlaceDbContext>();
        builder.Services.AddRazorPages();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 5;
        });

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

            options.LoginPath = "/Identity/Account/Login";
            options.AccessDeniedPath = "/Idenity/Account/AccessDenied";
            options.SlidingExpiration = true;
        });

        
        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<YourPlaceDbContext>();
            dbContext.Database.Migrate();
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        
        //IMPORTANT!!!
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            // Your custom endpoints before MapRazorPages

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Map Razor Pages (for Identity)
            endpoints.MapRazorPages();

            // Your custom endpoints after MapRazorPages
        });

        //app.MapControllerRoute(
        //    name: "default",
        //    pattern: "{controller=Home}/{action=Index}/{id?}");
        
        app.Run();
    }

}

