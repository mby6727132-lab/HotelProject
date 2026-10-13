using AutoMapper;
using Hotel.Core.DTOs.Hotels;
using Hotel.Core.Interfaces.Repositories;
using Hotel.Core.Interfaces.Services;
using HotelEntity = Hotel.Core.Entities.Hotel;

namespace Hotel.Service.Services;

public class HotelService : IHotelService
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public HotelService(IHotelRepository hotelRepository, IUserRepository userRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<HotelResponseDto>> GetByCityAsync(string city, string? name = null, CancellationToken cancellationToken = default)
    {
        var hotels = await _hotelRepository.GetByCityAsync(city, name, cancellationToken);
        return _mapper.Map<IEnumerable<HotelResponseDto>>(hotels);
    }

    public async Task<HotelResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var hotel = await _hotelRepository.GetByIdAsync(id, cancellationToken);
        return hotel is null ? null : _mapper.Map<HotelResponseDto>(hotel);
    }

    public async Task<HotelResponseDto> CreateAsync(CreateHotelDto dto, CancellationToken cancellationToken = default)
    {
        var hotel = _mapper.Map<HotelEntity>(dto);

        await _hotelRepository.AddAsync(hotel, cancellationToken);
        await _hotelRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<HotelResponseDto>(hotel);
    }

    public async Task<HotelResponseDto?> UpdateAsync(int id, UpdateHotelDto dto, CancellationToken cancellationToken = default)
    {
        var hotel = await _hotelRepository.GetByIdAsync(id, cancellationToken);
        if (hotel is null)
            return null;

        _mapper.Map(dto, hotel);
        _hotelRepository.Update(hotel);
        await _hotelRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<HotelResponseDto>(hotel);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var hotel = await _hotelRepository.GetByIdAsync(id, cancellationToken);
        if (hotel is null)
            return false;

        _hotelRepository.Delete(hotel);
        await _hotelRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> AddFavoriteAsync(int userId, int hotelId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return false;

        var hotel = await _hotelRepository.GetByIdWithFavoritesAsync(hotelId, cancellationToken);
        if (hotel is null)
            return false;

        if (hotel.FavoritedByUsers.All(u => u.Id != userId))
        {
            hotel.FavoritedByUsers.Add(user);
            await _hotelRepository.SaveChangesAsync(cancellationToken);
        }

        return true;
    }
}
