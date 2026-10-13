using Hotel.Core.Entities;

namespace Hotel.Core.Interfaces.Repositories;

public interface IInquiryRepository
{
    Task<Inquiry?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Inquiry>> GetByUserAsync(int userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Inquiry>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Inquiry entity, CancellationToken cancellationToken = default);
    void Update(Inquiry entity);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}
