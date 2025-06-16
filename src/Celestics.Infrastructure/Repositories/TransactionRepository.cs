using Celestics.Application.Repositories;
using Celestics.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Celestics.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly CelesticsDbContext _dbContext;

    public TransactionRepository(CelesticsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Transaction> Query()
    {
        return _dbContext.Transactions.AsNoTracking();
    }

    public async Task<Transaction?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbContext.Transactions.AsNoTracking()
            .FirstOrDefaultAsync(transaction => transaction.Id == id, ct);
    }

    public async Task<bool> ExistsByExternalIdAsync(string externalId, CancellationToken ct = default)
    {
        return await _dbContext.Transactions.AsNoTracking()
            .AnyAsync(transaction => transaction.ExternalId == externalId, ct);
    }

    public async Task AddRangeAsync(IEnumerable<Transaction> transactions, CancellationToken ct = default)
    {
        await _dbContext.Transactions.AddRangeAsync(transactions, ct);
    }
}
