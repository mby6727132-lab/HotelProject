using AutoMapper;
using Hotel.Core.DTOs.Payments;
using Hotel.Core.Entities;
using Hotel.Core.Exceptions;
using Hotel.Core.Interfaces.Repositories;
using Hotel.Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Service.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public PaymentService(IPaymentRepository paymentRepository, IRoomRepository roomRepository, IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<PaymentResponseDto> CreateAsync(int userId, CreatePaymentDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.CheckIn >= dto.CheckOut)
            throw new InvalidDateRangeException("CheckIn must be earlier than CheckOut.");

        // Loaded tracked, since the booking flow updates Room.LastBookingAt below
        var room = await _roomRepository.GetByIdAsync(dto.RoomId, cancellationToken);
        if (room is null)
            throw new KeyNotFoundException($"Room {dto.RoomId} was not found.");

        var hasOverlap = await _paymentRepository.ExistsOverlapAsync(dto.RoomId, dto.CheckIn, dto.CheckOut, cancellationToken);
        if (hasOverlap)
            throw new RoomUnavailableException("The room is already booked for the requested dates.");

        var nights = (dto.CheckOut - dto.CheckIn).Days;

        var payment = _mapper.Map<Payment>(dto);
        payment.UserId = userId;
        payment.Amount = nights * room.Price;
        payment.IsPaid = false;

        room.LastBookingAt = DateTime.UtcNow;
        _roomRepository.Update(room);

        await _paymentRepository.AddAsync(payment, cancellationToken);

        try
        {
            // Single SaveChanges call: Room.LastBookingAt update and the new Payment
            // are persisted together in the same unit of work. The UPDATE statement's
            // WHERE clause includes Room's original RowVersion (concurrency token), so
            // if another request already booked this room since it was loaded, zero rows
            // match and EF Core throws DbUpdateConcurrencyException below.
            await _paymentRepository.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            // Someone else booked this room between our read and our save.
            // 409 is the correct status here, not 400 and not 500.
            throw new ConcurrencyConflictException(
                "The room was booked by another request in the meantime. Please try again.");
        }

        return _mapper.Map<PaymentResponseDto>(payment);
    }

    public async Task<IEnumerable<PaymentResponseDto>> GetMyPaymentsAsync(int userId, CancellationToken cancellationToken = default)
    {
        var payments = await _paymentRepository.GetByUserAsync(userId, cancellationToken);
        return _mapper.Map<IEnumerable<PaymentResponseDto>>(payments);
    }
}
