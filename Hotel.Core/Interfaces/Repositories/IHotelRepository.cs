using HotelEntity = Hotel.Core.Entities.Hotel;

namespace Hotel.Core.Interfaces.Repositories;

public interface IHotelRepository
{
    Task<IEnumerable<HotelEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<HotelEntity>> GetByCityAsync(string city, string? name = null, CancellationToken cancellationToken = default);
    Task<HotelEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<HotelEntity?> GetByIdWithFavoritesAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(HotelEntity entity, CancellationToken cancellationToken = default);
    void Update(HotelEntity entity);
    void Delete(HotelEntity entity);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}
