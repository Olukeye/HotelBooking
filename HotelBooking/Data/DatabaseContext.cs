using HotelBookings.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HotelBookings.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }


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
