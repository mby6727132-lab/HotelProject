using Hotel.Core.Entities;
using Hotel.Core.Interfaces.Repositories;
using Hotel.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Data.Repositories;

public class InquiryRepository : IInquiryRepository
{
    private readonly AppDbContext _context;

    public InquiryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Inquiry?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Inquiries
            .Include(i => i.Hotel)
            .Include(i => i.User)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

    public async Task<IEnumerable<Inquiry>> GetByUserAsync(int userId, CancellationToken cancellationToken = default)
        => await _context.Inquiries
            .AsNoTracking()
            .Include(i => i.Hotel)
            .Where(i => i.UserId == userId)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Inquiry>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Inquiries
            .AsNoTracking()
            .Include(i => i.Hotel)
            .Include(i => i.User)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Inquiry entity, CancellationToken cancellationToken = default)
        => await _context.Inquiries.AddAsync(entity, cancellationToken);

    public void Update(Inquiry entity)
        => _context.Inquiries.Update(entity);

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken) > 0;
}
