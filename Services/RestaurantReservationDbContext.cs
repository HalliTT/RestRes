using Microsoft.EntityFrameworkCore;
using RestRes.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace RestRes.Services
{
    public class RestaurantReservationDbContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; init; }


        public DbSet<Reservation> Reservations { get; init; }


        public RestaurantReservationDbContext(DbContextOptions options)
        : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Restaurant>();
            modelBuilder.Entity<Reservation>();
        }
    }
}