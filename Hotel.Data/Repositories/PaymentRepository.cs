using Hotel.Core.Entities;
using Hotel.Core.Interfaces.Repositories;
using Hotel.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Data.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Payment entity, CancellationToken cancellationToken = default)
        => await _context.Payments.AddAsync(entity, cancellationToken);

    public async Task<IEnumerable<Payment>> GetByUserAsync(int userId, CancellationToken cancellationToken = default)
        => await _context.Payments
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .ToListAsync(cancellationToken);

    public async Task<bool> ExistsOverlapAsync(
        int roomId,
        DateTime checkIn,
        DateTime checkOut,
        CancellationToken cancellationToken = default)
        => await _context.Payments
            .AsNoTracking()
            .AnyAsync(p =>
                p.RoomId == roomId &&
                p.CheckIn < checkOut &&
                p.CheckOut > checkIn,
                cancellationToken);

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken) > 0;
}
