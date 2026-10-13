using Hotel.Core.Interfaces.Repositories;
using Hotel.Data.Context;
using Microsoft.EntityFrameworkCore;
using HotelEntity = Hotel.Core.Entities.Hotel;

namespace Hotel.Data.Repositories;

public class HotelRepository : IHotelRepository
{
    private readonly AppDbContext _context;

    public HotelRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<HotelEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Hotels.AsNoTracking().Include(h => h.Images).ToListAsync(cancellationToken);

    public async Task<IEnumerable<HotelEntity>> GetByCityAsync(string city, string? name = null, CancellationToken cancellationToken = default)
        => await _context.Hotels
            .AsNoTracking()
            .Include(h => h.Images)
            .Where(h => h.City == city)
            .Where(h => string.IsNullOrEmpty(name) || h.Name.Contains(name))
            .ToListAsync(cancellationToken);

    public async Task<HotelEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Hotels.Include(h => h.Images).FirstOrDefaultAsync(h => h.Id == id, cancellationToken);

    public async Task<HotelEntity?> GetByIdWithFavoritesAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Hotels
            .Include(h => h.FavoritedByUsers)
            .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);

    public async Task AddAsync(HotelEntity entity, CancellationToken cancellationToken = default)
        => await _context.Hotels.AddAsync(entity, cancellationToken);

    public void Update(HotelEntity entity)
        => _context.Hotels.Update(entity);

    public void Delete(HotelEntity entity)
        => _context.Hotels.Remove(entity);

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken) > 0;
}
