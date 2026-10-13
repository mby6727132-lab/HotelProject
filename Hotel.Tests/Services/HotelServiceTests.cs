using AutoMapper;
using Hotel.Core.Entities;
using Hotel.Core.Interfaces.Repositories;
using Hotel.Service.Mapping;
using Hotel.Service.Services;
using Moq;
using HotelEntity = Hotel.Core.Entities.Hotel;

namespace Hotel.Tests.Services;

public class HotelServiceTests
{
    private readonly Mock<IHotelRepository> _hotelRepository = new();
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly HotelService _sut;

    public HotelServiceTests()
    {
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
        _sut = new HotelService(_hotelRepository.Object, _userRepository.Object, mapper);
    }

    [Fact]
    public async Task GetByCityAsync_ReturnsMappedHotels_ForGivenCity()
    {
        var hotels = new List<HotelEntity>
        {
            new() { Id = 1, Name = "Dan Tel Aviv", City = "Tel Aviv", Address = "99 HaYarkon St." }
        };
        _hotelRepository.Setup(r => r.GetByCityAsync("Tel Aviv", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(hotels);

        var result = (await _sut.GetByCityAsync("Tel Aviv", cancellationToken: CancellationToken.None)).ToList();

        Assert.Single(result);
        Assert.Equal("Dan Tel Aviv", result[0].Name);
        Assert.Equal("Tel Aviv", result[0].City);
    }

    [Fact]
    public async Task AddFavoriteAsync_ReturnsFalse_WhenHotelDoesNotExist()
    {
        _userRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User { Id = 1, Name = "x", Email = "x@x.com", PasswordHash = "h", Role = "Customer" });
        _hotelRepository.Setup(r => r.GetByIdWithFavoritesAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((HotelEntity?)null);

        var result = await _sut.AddFavoriteAsync(1, 99, CancellationToken.None);

        Assert.False(result);
        _hotelRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task AddFavoriteAsync_AddsRelationshipOnlyOnce_WhenCalledTwice()
    {
        var user = new User { Id = 1, Name = "x", Email = "x@x.com", PasswordHash = "h", Role = "Customer" };
        var hotel = new HotelEntity { Id = 2, Name = "King David", City = "Jerusalem", Address = "addr" };
        _userRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _hotelRepository.Setup(r => r.GetByIdWithFavoritesAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(hotel);
        _hotelRepository.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var first = await _sut.AddFavoriteAsync(1, 2, CancellationToken.None);
        var second = await _sut.AddFavoriteAsync(1, 2, CancellationToken.None);

        Assert.True(first);
        Assert.True(second);
        Assert.Single(hotel.FavoritedByUsers);
        // Second call finds the user already in the collection, so it must not save again.
        _hotelRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
