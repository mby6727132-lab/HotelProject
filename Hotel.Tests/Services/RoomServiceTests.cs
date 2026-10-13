using AutoMapper;
using Hotel.Core.DTOs.Rooms;
using Hotel.Core.Entities;
using Hotel.Core.Exceptions;
using Hotel.Core.Interfaces.Repositories;
using Hotel.Service.Mapping;
using Hotel.Service.Services;
using Moq;

namespace Hotel.Tests.Services;

public class RoomServiceTests
{
    private readonly Mock<IRoomRepository> _roomRepository = new();
    private readonly RoomService _sut;

    public RoomServiceTests()
    {
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
        _sut = new RoomService(_roomRepository.Object, mapper);
    }

    [Fact]
    public async Task GetAvailableAsync_ReturnsMappedRooms_WhenDatesValid()
    {
        var query = new AvailableRoomsQueryDto { CheckIn = new DateTime(2026, 12, 1), CheckOut = new DateTime(2026, 12, 5), Page = 1, PageSize = 10 };
        var rooms = new List<Room> { new() { Id = 1, RoomNumber = 101, Floor = 1, Price = 450m, HotelId = 1 } };
        _roomRepository
            .Setup(r => r.GetAvailableAsync(1, query.CheckIn, query.CheckOut, query.Guests, query.Page, query.PageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(rooms);

        var result = (await _sut.GetAvailableAsync(1, query, CancellationToken.None)).ToList();

        Assert.Single(result);
        Assert.Equal(101, result[0].RoomNumber);
        Assert.Equal(450m, result[0].Price);
    }

    [Theory]
    [InlineData("2026-12-05", "2026-12-01")] // checkIn after checkOut
    [InlineData("2026-12-01", "2026-12-01")] // checkIn == checkOut
    public async Task GetAvailableAsync_ThrowsInvalidDateRangeException_WhenRangeIsNotValid(string checkIn, string checkOut)
    {
        var query = new AvailableRoomsQueryDto { CheckIn = DateTime.Parse(checkIn), CheckOut = DateTime.Parse(checkOut) };

        await Assert.ThrowsAsync<InvalidDateRangeException>(() => _sut.GetAvailableAsync(1, query, CancellationToken.None));

        _roomRepository.Verify(
            r => r.GetAvailableAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GetAvailableAsync_PassesPageAndPageSizeThrough_ToTheRepository()
    {
        var query = new AvailableRoomsQueryDto { CheckIn = new DateTime(2026, 12, 1), CheckOut = new DateTime(2026, 12, 5), Page = 2, PageSize = 5 };
        _roomRepository
            .Setup(r => r.GetAvailableAsync(3, query.CheckIn, query.CheckOut, query.Guests, 2, 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Room>());

        await _sut.GetAvailableAsync(3, query, CancellationToken.None);

        _roomRepository.Verify(r => r.GetAvailableAsync(3, query.CheckIn, query.CheckOut, query.Guests, 2, 5, It.IsAny<CancellationToken>()), Times.Once);
    }
}
