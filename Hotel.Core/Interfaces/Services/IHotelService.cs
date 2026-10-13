using Hotel.Core.DTOs.Hotels;

namespace Hotel.Core.Interfaces.Services;

public interface IHotelService
{
    Task<IEnumerable<HotelResponseDto>> GetByCityAsync(string city, string? name = null, CancellationToken cancellationToken = default);
    Task<HotelResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<HotelResponseDto> CreateAsync(CreateHotelDto dto, CancellationToken cancellationToken = default);
    Task<HotelResponseDto?> UpdateAsync(int id, UpdateHotelDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AddFavoriteAsync(int userId, int hotelId, CancellationToken cancellationToken = default);
}
