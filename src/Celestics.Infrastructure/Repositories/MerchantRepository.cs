using Celestics.Application.Repositories;
using Celestics.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Celestics.Infrastructure.Repositories;

public class MerchantRepository : IMerchantRepository
{
    private readonly CelesticsDbContext _dbContext;

    public MerchantRepository(CelesticsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Merchant> Query()
    {
        return _dbContext.Merchants.AsNoTracking();
    }

    public async Task<Merchant?> GetByIdAsync(Guid id, bool includeTransactions = false, CancellationToken ct = default)
    {
        IQueryable<Merchant> query = _dbContext.Merchants;

        if (includeTransactions)
        {
            query = query.Include(merchant => merchant.Transactions);
        }

        return await query.AsNoTracking().FirstOrDefaultAsync(merchant => merchant.Id == id, ct);
    }
}
