using Hotel.Core.Entities;
using Hotel.Data.Context;
using Microsoft.EntityFrameworkCore;
using HotelEntity = Hotel.Core.Entities.Hotel;

namespace Hotel.Tests.Concurrency;

/// <summary>
/// Integration test proving the Optimistic Concurrency mechanism on Room.RowVersion:
/// two separate DbContext instances read the same Room, both try to save a conflicting
/// update, and only one is allowed to succeed. Requires a real SQL Server LocalDB instance
/// (rowversion is a SQL Server-specific computed column; this cannot be faked with a mock).
/// </summary>
public class ConcurrencyTests
{
    private const string ConnectionString =
        "Server=(localdb)\\mssqllocaldb;Database=HotelProjectDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

    private static DbContextOptions<AppDbContext> Options => new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlServer(ConnectionString)
        .Options;

    [Fact]
    public async Task TwoDbContexts_ConflictingUpdate_OnlyOneSaveSucceeds()
    {
        int roomId;
        await using (var setupContext = new AppDbContext(Options))
        {
            var hotel = new HotelEntity { Name = "__ConcurrencyTestHotel__", City = "TestCity", Address = "Test Address" };
            var room = new Room { RoomNumber = 999, Floor = 9, Price = 100m, Hotel = hotel };
            setupContext.Hotels.Add(hotel);
            setupContext.Rooms.Add(room);
            await setupContext.SaveChangesAsync();
            roomId = room.Id;
        }

        try
        {
            await using var contextA = new AppDbContext(Options);
            await using var contextB = new AppDbContext(Options);

            var roomA = await contextA.Rooms.FirstAsync(r => r.Id == roomId);
            var roomB = await contextB.Rooms.FirstAsync(r => r.Id == roomId);

            // Both contexts read the exact same RowVersion before either writes.
            Assert.True(roomA.RowVersion.SequenceEqual(roomB.RowVersion));

            roomA.LastBookingAt = DateTime.UtcNow;
            roomB.LastBookingAt = DateTime.UtcNow.AddSeconds(1);

            // First save: succeeds, and bumps the row's RowVersion in the database.
            await contextA.SaveChangesAsync();

            // Second save: its UPDATE's WHERE clause still carries the now-stale original
            // RowVersion, matches zero rows, and EF Core must surface that as a conflict.
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => contextB.SaveChangesAsync());
        }
        finally
        {
            await using var cleanupContext = new AppDbContext(Options);
            var room = await cleanupContext.Rooms.FirstAsync(r => r.Id == roomId);
            var hotel = await cleanupContext.Hotels.FirstAsync(h => h.Id == room.HotelId);
            cleanupContext.Rooms.Remove(room);
            cleanupContext.Hotels.Remove(hotel);
            await cleanupContext.SaveChangesAsync();
        }
    }
}
