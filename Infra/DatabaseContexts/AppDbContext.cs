namespace CinemaApp.Infra.DatabaseContexts;

using Microsoft.EntityFrameworkCore;
using Domain.Entities;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<CinemaRoom> CinemaRooms { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<MovieSchedule> MovieSchedules { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //     .OwnsMany(ms => ms.BookedSeats);
        //
        
        modelBuilder.Entity<Booking>(builder =>
        {
            // Map the Seat value object to columns in the Booking table
            builder.OwnsOne(b => b.Seat, seat =>
            {
                seat.Property(s => s.Row).HasColumnName("SeatRow"); // Map to SeatRow column
                seat.Property(s => s.Number).HasColumnName("SeatNumber"); // Map to SeatNumber column
                seat.Property(s => s.IsAvailable).HasColumnName("SeatIsAvailable"); // Map to SeatIsAvailable column
            });

            // Configure other properties of Booking
            builder.HasKey(b => b.Id); // Assuming BaseEntity has an Id property
            builder.Property(b => b.MovieScheduleId).IsRequired();
        });
    }
}
