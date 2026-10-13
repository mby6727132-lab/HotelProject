using Hotel.Core.Entities;

namespace Hotel.Core.Interfaces.Repositories;

public interface IRoomRepository
{
    Task<IEnumerable<Room>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Room?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Room>> GetAvailableAsync(
        int hotelId,
        DateTime checkIn,
        DateTime checkOut,
        int? guests,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task AddAsync(Room entity, CancellationToken cancellationToken = default);
    void Update(Room entity);
    void Delete(Room entity);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}
