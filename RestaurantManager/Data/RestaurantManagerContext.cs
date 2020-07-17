using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class RestaurantManagerContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Waiter> Waiters { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<PastReservation> PastReservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Waiter>()
                .HasMany(waiter => waiter.Reservations)
                .WithOne(reservation => reservation.ServiceWaiter)
                .HasForeignKey(reservation => reservation.WaiterId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany(user => user.Reservations)
                .WithOne(reservation => reservation.ReservationHolder)
                .HasForeignKey(reservation => reservation.ReservationHolderId)
                .OnDelete(DeleteBehavior.Cascade);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-9F6IC4T\\SQLEXPRESS;Database=RestaurantManagerDB;Trusted_Connection=True;");
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
        }
    }
}
