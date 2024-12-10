using Microsoft.EntityFrameworkCore;
using RentACar.Api.Entities;

namespace RentACar.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } 
        public DbSet<Car> Cars { get; set; } 
        public DbSet<Reservation> Reservations { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>()
                .Property(r => r.StartDate)
                .HasColumnType("timestamp with time zone");

            modelBuilder.Entity<Reservation>()
                .Property(r => r.EndDate)
                .HasColumnType("timestamp with time zone");
        }

    }

}
