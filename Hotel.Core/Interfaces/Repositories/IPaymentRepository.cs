using Hotel.Core.Entities;

namespace Hotel.Core.Interfaces.Repositories;

public interface IPaymentRepository
{
    Task AddAsync(Payment entity, CancellationToken cancellationToken = default);
    Task<IEnumerable<Payment>> GetByUserAsync(int userId, CancellationToken cancellationToken = default);
    Task<bool> ExistsOverlapAsync(
        int roomId,
        DateTime checkIn,
        DateTime checkOut,
        CancellationToken cancellationToken = default);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}
