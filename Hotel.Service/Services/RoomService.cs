using AutoMapper;
using Hotel.Core.DTOs.Rooms;
using Hotel.Core.Entities;
using Hotel.Core.Exceptions;
using Hotel.Core.Interfaces.Repositories;
using Hotel.Core.Interfaces.Services;

namespace Hotel.Service.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public RoomService(IRoomRepository roomRepository, IMapper mapper)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoomResponseDto>> GetAvailableAsync(int hotelId, AvailableRoomsQueryDto query, CancellationToken cancellationToken = default)
    {
        if (query.CheckIn >= query.CheckOut)
            throw new InvalidDateRangeException("CheckIn must be earlier than CheckOut.");

        var rooms = await _roomRepository.GetAvailableAsync(
            hotelId, query.CheckIn, query.CheckOut, query.Guests, query.Page, query.PageSize, cancellationToken);

        return _mapper.Map<IEnumerable<RoomResponseDto>>(rooms);
    }

    public async Task<RoomResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var room = await _roomRepository.GetByIdAsync(id, cancellationToken);
        return room is null ? null : _mapper.Map<RoomResponseDto>(room);
    }

    public async Task<RoomResponseDto> CreateAsync(CreateRoomDto dto, CancellationToken cancellationToken = default)
    {
        var room = _mapper.Map<Room>(dto);

        await _roomRepository.AddAsync(room, cancellationToken);
        await _roomRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<RoomResponseDto>(room);
    }

    public async Task<RoomResponseDto?> UpdateAsync(int id, UpdateRoomDto dto, CancellationToken cancellationToken = default)
    {
        var room = await _roomRepository.GetByIdAsync(id, cancellationToken);
        if (room is null)
            return null;

        _mapper.Map(dto, room);
        _roomRepository.Update(room);
        await _roomRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<RoomResponseDto>(room);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var room = await _roomRepository.GetByIdAsync(id, cancellationToken);
        if (room is null)
            return false;

        _roomRepository.Delete(room);
        await _roomRepository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
