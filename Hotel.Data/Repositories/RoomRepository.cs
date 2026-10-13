using Hotel.Core.Entities;
using Hotel.Core.Interfaces.Repositories;
using Hotel.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Data.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly AppDbContext _context;

    public RoomRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Room>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Rooms.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<Room?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Rooms.Include(r => r.Images).FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

    public async Task<IEnumerable<Room>> GetAvailableAsync(
        int hotelId,
        DateTime checkIn,
        DateTime checkOut,
        int? guests,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
        => await _context.Rooms
            .AsNoTracking()
            .Include(r => r.Images)
            .Where(r => r.HotelId == hotelId)
            .Where(r => guests == null || r.Capacity >= guests)
            .Where(r => !_context.Payments.Any(p =>
                p.RoomId == r.Id &&
                p.CheckIn < checkOut &&
                p.CheckOut > checkIn))
            .OrderBy(r => r.RoomNumber)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Room entity, CancellationToken cancellationToken = default)
        => await _context.Rooms.AddAsync(entity, cancellationToken);

    public void Update(Room entity)
        => _context.Rooms.Update(entity);

    public void Delete(Room entity)
        => _context.Rooms.Remove(entity);

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken) > 0;
}
