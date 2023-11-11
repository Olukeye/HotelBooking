using HotelBooking.Configuration.Entities;
using HotelBooking.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data
{
    public class DatabaseContext : IdentityDbContext<ApiUser>
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }
        public  DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RolesConfig());

        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Country>().HasData(
        //        new Country
        //        {
        //            Id = 1,
        //            Name = "",
        //            ShortName = ""
        //        });

        //    modelBuilder.Entity<Hotel>().HasData(
        //        new Hotel
        //        {
        //            Id = 1,
        //            Name = "",
        //            Address = "",
        //            CheapestRate = "",
        //             City = ""
        //        });
        //}
    }
}
