using HotelEntity = Hotel.Core.Entities.Hotel;
using Microsoft.EntityFrameworkCore;
using Hotel.Core.Entities;

namespace Hotel.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<HotelEntity> Hotels => Set<HotelEntity>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<HotelImage> HotelImages => Set<HotelImage>();
    public DbSet<RoomImage> RoomImages => Set<RoomImage>();
    public DbSet<Inquiry> Inquiries => Set<Inquiry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Applies HotelConfiguration, RoomConfiguration, UserConfiguration, PaymentConfiguration
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
