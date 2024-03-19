using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YourPlace.Infrastructure.Data.Entities;
using Microsoft.SqlServer;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using YourPlace.Infrastructure.Data.Enums;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Identity;


namespace YourPlace.Infrastructure.Data
{
    public class YourPlaceDbContext : IdentityDbContext<User>
    {
        public YourPlaceDbContext(DbContextOptions<YourPlaceDbContext> options) : base(options) { }
        
        //public DbSet<User> Users { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Preferences> Preferences { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Categories> Categories { get; set; }
        public DbSet<RoomAvailability> RoomsAvailability { get; set;}
        
        public DbSet<ReservedRoom> ReservedRooms { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=YourPlaceDb;Integrated Security=False;Connect Timeout=30;Encrypt=False;");
            }

            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Conversions For Enums
            modelBuilder.Entity<Room>().Property(r => r.Type).HasConversion<string>();
            modelBuilder.Entity<RoomAvailability>().Property(r=>r.Type).HasConversion<string>();
            modelBuilder.Entity<Categories>().Property(r => r.Location).HasConversion<string>();
            modelBuilder.Entity<Categories>().Property(r => r.Tourism).HasConversion<string>();
            modelBuilder.Entity<Categories>().Property(r => r.Atmosphere).HasConversion<string>();
            modelBuilder.Entity<Categories>().Property(r => r.Company).HasConversion<string>();
            modelBuilder.Entity<Categories>().Property(r => r.Pricing).HasConversion<string>();

            modelBuilder.Entity<Preferences>().Property(r => r.Location).HasConversion<string>();
            modelBuilder.Entity<Preferences>().Property(r => r.Tourism).HasConversion<string>();
            modelBuilder.Entity<Preferences>().Property(r => r.Atmosphere).HasConversion<string>();
            modelBuilder.Entity<Preferences>().Property(r => r.Company).HasConversion<string>();
            modelBuilder.Entity<Preferences>().Property(r => r.Pricing).HasConversion<string>();
            #endregion

            #region Relationships
            //modelBuilder.Entity<Preferences>().HasOne(h => h.User).WithMany().HasForeignKey(p => p.UserId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Categories>().HasOne(c => c.Hotel).WithMany().HasForeignKey(c => c.HotelID).IsRequired().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Reservation>().HasOne(r => r.Hotel).WithMany().HasForeignKey(r => r.HotelID).IsRequired().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RoomAvailability>().HasOne(r => r.Hotel).WithMany().HasForeignKey(r => r.HotelID).IsRequired().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ReservedRoom>().HasOne(r => r.Reservation).WithMany().HasForeignKey(r => r.ReservationID).IsRequired().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ReservedRoom>().HasOne(r => r.Room).WithMany().HasForeignKey(r => r.RoomID).IsRequired().OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Seeding
            modelBuilder.Entity<IdentityRole>().HasData(
             new IdentityRole { Id = "1", Name = "Traveller", NormalizedName = "Traveller" },
             new IdentityRole { Id = "2", Name = "Hotel Manager", NormalizedName = "HotelManager" },
             new IdentityRole { Id = "3", Name = "Admin", NormalizedName = "Admin" }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = "1",
                    UserName = "Admin",
                    NormalizedUserName = "Admin",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    Surname = "User",
                    PasswordHash = new PasswordHasher<User>().HashPassword(null, "Password123"),
                    SecurityStamp = string.Empty
                }
            );


            modelBuilder.Entity<Hotel>().HasData(
               new() { HotelID = 1, MainImageURL = "Arte.jpg", HotelName = "Arte Spa Hotel", Address = "ул. Юндола 20", Town = "Велинград", Country = "България", Rating = 9.7, Details = "Хотелът е с чудесен изглед към гората. Има неограничен безплатен Wi-Fi и удобен паркинг. Хотелът разполага с три вътрешни басейна и един външен." },
               new() { HotelID = 2, MainImageURL = "RoseGarden.jpg", HotelName = "Rose Garden", Address = "ул. Ропотамо 12", Town = "Поморие", Country = "България", Rating = 8.5, Details = "Апартаменти Роуз Гардънс се намират на 50 метра от плажа. Включват сезонен външен басейн и сезонен ресторант, безплатен Wi-Fi и сезонен спа център." },
               new() { HotelID = 3, MainImageURL = "Therme.jpg", HotelName = "Therme", Address = "ул. Горна Баня", Town = "Баня", Country = "България", Rating = 9.1, Details = "Хотелът предлага безплатен високоскоростен WI-FI. Има спа център и 3 вътрешни басейна, както и 2 външни - един за деца, един за възрастни." },
               new() { HotelID = 4, MainImageURL = "LaFleur.jpg", HotelName = "La Fleur", Address = "Flower str.", Town = "Paris", Country = "Франция", Rating = 9.5, Details = "Прекрасна гледка към Айфеловата кула. Храната е високо качество, а стаите са прекрасни." },
               new() { HotelID = 5, MainImageURL = "RoyalLasVegas.jpg", HotelName = "Las Vegas Royal", Address = "Monte Carlo str.", Town = "Las Vegas", Country = "САЩ", Rating = 7.9, Details = "Хотел Las Vegas Royal предлага всякакви по вид занимания - от масажи до турнири по тенис и футбол. All-Inclisuve с включена храна и напитки" }
            );

            modelBuilder.Entity<Image>().HasData(
                new Image { ImageID = 1, ImageURL = "Arte1.jpg", HotelID = 1 },
                new Image { ImageID = 2, ImageURL = "Arte2.jpg", HotelID = 1 },
                new Image { ImageID = 3, ImageURL = "Arte3.jpg", HotelID = 1 },
                new Image { ImageID = 4, ImageURL = "RoseGarden1.jpg", HotelID = 2 },
                new Image { ImageID = 5, ImageURL = "RoseGarden2.jpg", HotelID = 2 },
                new Image { ImageID = 6, ImageURL = "RoseGarden3.jpg", HotelID = 2 },
                new Image { ImageID = 7, ImageURL = "RoseGarden4.jpg", HotelID = 2 },
                new Image { ImageID = 8, ImageURL = "Therme1.jpg", HotelID = 3 },
                new Image { ImageID = 9, ImageURL = "Therme2.jpg", HotelID = 3 },
                new Image { ImageID = 10, ImageURL = "Therme3.jpg", HotelID = 3 },
                new Image { ImageID = 11, ImageURL = "LaFleur1.jpg", HotelID = 4 },
                new Image { ImageID = 12, ImageURL = "LaFleur2.jpg", HotelID = 4 },
                new Image { ImageID = 13, ImageURL = "LaFleur3.jpg", HotelID = 4 },
                new Image { ImageID = 14, ImageURL = "LasVegasRoyal1.jpg", HotelID = 5 },
                new Image { ImageID = 15, ImageURL = "LasVegasRoyal2.jpg", HotelID = 5 }
            );

            modelBuilder.Entity<Room>().HasData(
                  // Hotel 1 rooms
                  new Room { RoomID = 1, Type = RoomTypes.studio, Price = 100.00m, MaxPeopleCount = 2, CountInHotel = 5, HotelID = 1 },
                  new Room { RoomID = 2, Type = RoomTypes.doubleRoom, Price = 150.00m, MaxPeopleCount = 2, CountInHotel = 10, HotelID = 1 },

                  // Hotel 2 rooms
                  new Room { RoomID = 3, Type = RoomTypes.tripleRoom, Price = 200.00m, MaxPeopleCount = 3, CountInHotel = 8, HotelID = 2 },
                  new Room { RoomID = 4, Type = RoomTypes.deluxeRoom, Price = 300.00m, MaxPeopleCount = 4, CountInHotel = 3, HotelID = 2 },

                  // Hotel 3 rooms
                  new Room { RoomID = 5, Type = RoomTypes.maisonette, Price = 400.00m, MaxPeopleCount = 6, CountInHotel = 2, HotelID = 3 },
                  new Room { RoomID = 6, Type = RoomTypes.studio, Price = 120.00m, MaxPeopleCount = 2, CountInHotel = 5, HotelID = 3 },

                  // Hotel 4 rooms
                  new Room { RoomID = 7, Type = RoomTypes.studio, Price = 90.00m, MaxPeopleCount = 2, CountInHotel = 3, HotelID = 4 },
                  new Room { RoomID = 8, Type = RoomTypes.doubleRoom, Price = 120.00m, MaxPeopleCount = 2, CountInHotel = 5, HotelID = 4 },

                  // Hotel 5 rooms
                  new Room { RoomID = 9, Type = RoomTypes.tripleRoom, Price = 180.00m, MaxPeopleCount = 3, CountInHotel = 4, HotelID = 5 },
                  new Room { RoomID = 10, Type = RoomTypes.deluxeRoom, Price = 250.00m, MaxPeopleCount = 4, CountInHotel = 2, HotelID = 5 }
              );

            modelBuilder.Entity<Categories>().HasData(
                 new Categories { CategoryID = 1, Location = Location.Sea, Tourism = Tourism.Culture, Atmosphere = Atmosphere.Calm, Company = Company.Family, Pricing = Pricing.Lux, HotelID = 1 },
                 new Categories { CategoryID = 2, Location = Location.Mountain, Tourism = Tourism.Adventure, Atmosphere = Atmosphere.Party, Company = Company.Group, Pricing = Pricing.InTheMiddle, HotelID = 2 },
                 new Categories { CategoryID = 3, Location = Location.LargeCity, Tourism = Tourism.Shopping, Atmosphere = Atmosphere.Both, Company = Company.OnePerson, Pricing = Pricing.Cheap, HotelID = 3 },
                 new Categories { CategoryID = 4, Location = Location.Village, Tourism = Tourism.Relax, Atmosphere = Atmosphere.Neither, Company = Company.Individual, Pricing = Pricing.Modern, HotelID = 4 },
                 new Categories { CategoryID = 5, Location = Location.Sea, Tourism = Tourism.Adventure, Atmosphere = Atmosphere.Party, Company = Company.Family, Pricing = Pricing.InTheMiddle, HotelID = 5 }
            );


            modelBuilder.Entity<Reservation>().HasData(
                 new Reservation { ReservationID = 1, FirstName = "Иван", Surname = "Петров", ArrivalDate = new DateOnly(2024, 3, 20), LeavingDate = new DateOnly(2024, 3, 25), PeopleCount = 2, Price = 500.00m, HotelID = 1,
                     ReservedRooms = new List<RoomSelection>
                     {
                        new RoomSelection { RoomID = 1, ChosenCount = 1 },
                        new RoomSelection { RoomID = 2, ChosenCount = 1}
                     }
                 },
                 new Reservation { ReservationID = 2, FirstName = "Мария", Surname = "Иванова", ArrivalDate = new DateOnly(2024, 4, 10), LeavingDate = new DateOnly(2024, 4, 15), PeopleCount = 1, Price = 300.00m, HotelID = 2,
                     ReservedRooms = new List<RoomSelection>
                     {
                        new RoomSelection { RoomID = 3, ChosenCount = 1}
                     }
                 },
                 new Reservation { ReservationID = 3, FirstName = "Петър", Surname = "Иванов", ArrivalDate = new DateOnly(2024, 5, 10), LeavingDate = new DateOnly(2024, 5, 15), PeopleCount = 3, Price = 750.00m, HotelID = 1,
                     ReservedRooms = new List<RoomSelection>
                     {
                        new RoomSelection { RoomID = 1, ChosenCount = 2}
                     }
                 },
                 new Reservation { ReservationID = 4, FirstName = "Гергана", Surname = "Петрова", ArrivalDate = new DateOnly(2024, 6, 20), LeavingDate = new DateOnly(2024, 6, 25), PeopleCount = 2, Price = 600.00m, HotelID = 2,
                     ReservedRooms = new List<RoomSelection>
                     {
                        new RoomSelection { RoomID = 4, ChosenCount = 1}
                     }
                 },
                 new Reservation { ReservationID = 5, FirstName = "Стефан", Surname = "Георгиев", ArrivalDate = new DateOnly(2024, 7, 1), LeavingDate = new DateOnly(2024, 7, 5), PeopleCount = 1, Price = 200.00m, HotelID = 3,
                     ReservedRooms = new List<RoomSelection>
                     {
                        new RoomSelection { RoomID = 6, ChosenCount = 1},
                        new RoomSelection { RoomID = 7, ChosenCount = 1}
                     }
                 }
            );

            modelBuilder.Entity<ReservedRoom>().HasData(
                // Reservation 1
                new ReservedRoom { ID = 1, ReservationID = 1, RoomID = 1, Count = 1, HotelID = 1 },
                new ReservedRoom { ID = 2, ReservationID = 1, RoomID = 2, Count = 1, HotelID = 1 },

                // Reservation 2
                new ReservedRoom { ID = 3, ReservationID = 2, RoomID = 3, Count = 1, HotelID = 2 },

                // Reservation 3
                new ReservedRoom { ID = 4, ReservationID = 3, RoomID = 1, Count = 2, HotelID = 1 },

                // Reservation 4
                new ReservedRoom { ID = 5, ReservationID = 4, RoomID = 4, Count = 1, HotelID = 2 },

                // Reservation 5
                new ReservedRoom { ID = 6, ReservationID = 5, RoomID = 6, Count = 1, HotelID = 3 },
                new ReservedRoom { ID = 7, ReservationID = 5, RoomID = 7, Count = 1, HotelID = 3 }
            );


            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
