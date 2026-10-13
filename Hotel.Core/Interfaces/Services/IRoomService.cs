using Hotel.Core.DTOs.Rooms;

namespace Hotel.Core.Interfaces.Services;

public interface IRoomService
{
    Task<IEnumerable<RoomResponseDto>> GetAvailableAsync(int hotelId, AvailableRoomsQueryDto query, CancellationToken cancellationToken = default);
    Task<RoomResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<RoomResponseDto> CreateAsync(CreateRoomDto dto, CancellationToken cancellationToken = default);
    Task<RoomResponseDto?> UpdateAsync(int id, UpdateRoomDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
