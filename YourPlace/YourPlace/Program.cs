using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YourPlace.Infrastructure.Data;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using YourPlace.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity.UI.Services;
using YourPlace.Core.Services;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<YourPlaceDbContext>(options => options.UseSqlServer(connectionString));

        
        builder.Services.AddScoped<UserServices, UserServices>();
        //builder.Services.AddScoped<IEmailSender, EmailSender>();
        builder.Services.AddScoped<HotelsServices>();



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

