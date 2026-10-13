using AutoMapper;
using Hotel.Core.DTOs.Payments;
using Hotel.Core.Entities;
using Hotel.Core.Exceptions;
using Hotel.Core.Interfaces.Repositories;
using Hotel.Service.Mapping;
using Hotel.Service.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Hotel.Tests.Services;

public class PaymentServiceTests
{
    private readonly Mock<IPaymentRepository> _paymentRepository = new();
    private readonly Mock<IRoomRepository> _roomRepository = new();
    private readonly PaymentService _sut;

    public PaymentServiceTests()
    {
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
        _sut = new PaymentService(_paymentRepository.Object, _roomRepository.Object, mapper);
    }

    private static CreatePaymentDto ValidDto(int roomId = 1) => new()
    {
        RoomId = roomId,
        CheckIn = new DateTime(2026, 12, 1),
        CheckOut = new DateTime(2026, 12, 4) // 3 nights
    };

    [Fact]
    public async Task CreateAsync_ThrowsInvalidDateRangeException_WhenCheckInNotBeforeCheckOut()
    {
        var dto = new CreatePaymentDto { RoomId = 1, CheckIn = new DateTime(2026, 12, 4), CheckOut = new DateTime(2026, 12, 1) };

        await Assert.ThrowsAsync<InvalidDateRangeException>(() => _sut.CreateAsync(1, dto, CancellationToken.None));

        _roomRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ThrowsKeyNotFoundException_WhenRoomDoesNotExist()
    {
        _roomRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Room?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.CreateAsync(1, ValidDto(), CancellationToken.None));
    }

    [Fact]
    public async Task CreateAsync_ThrowsRoomUnavailableException_WhenOverlapExists()
    {
        var room = new Room { Id = 1, RoomNumber = 101, Floor = 1, Price = 100m, HotelId = 1 };
        _roomRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(room);
        _paymentRepository
            .Setup(r => r.ExistsOverlapAsync(1, It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<RoomUnavailableException>(() => _sut.CreateAsync(1, ValidDto(), CancellationToken.None));

        _paymentRepository.Verify(r => r.AddAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_CalculatesAmountServerSide_FromNightsTimesRoomPrice()
    {
        // CreatePaymentDto carries no Amount field at all - the server is the only source of truth for price.
        var room = new Room { Id = 1, RoomNumber = 101, Floor = 1, Price = 200m, HotelId = 1 };
        _roomRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(room);
        _paymentRepository
            .Setup(r => r.ExistsOverlapAsync(1, It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        Payment? captured = null;
        _paymentRepository.Setup(r => r.AddAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()))
            .Callback<Payment, CancellationToken>((p, _) => captured = p)
            .Returns(Task.CompletedTask);
        _paymentRepository.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var result = await _sut.CreateAsync(42, ValidDto(), CancellationToken.None); // 3 nights * 200 = 600

        Assert.Equal(600m, result.Amount);
        Assert.NotNull(captured);
        Assert.Equal(600m, captured!.Amount);
        Assert.Equal(42, captured.UserId);
        Assert.False(captured.IsPaid);
        Assert.NotNull(room.LastBookingAt);
    }

    [Fact]
    public async Task CreateAsync_ThrowsConcurrencyConflictException_WhenSaveChangesThrowsDbUpdateConcurrencyException()
    {
        var room = new Room { Id = 1, RoomNumber = 101, Floor = 1, Price = 100m, HotelId = 1 };
        _roomRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(room);
        _paymentRepository
            .Setup(r => r.ExistsOverlapAsync(1, It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _paymentRepository.Setup(r => r.AddAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _paymentRepository.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DbUpdateConcurrencyException("Simulated conflict"));

        await Assert.ThrowsAsync<ConcurrencyConflictException>(() => _sut.CreateAsync(1, ValidDto(), CancellationToken.None));
    }
}
