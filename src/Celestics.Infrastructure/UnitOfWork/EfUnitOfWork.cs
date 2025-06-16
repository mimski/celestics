using Celestics.Application.Repositories;
using Celestics.Application.UnitOfWork;

namespace Celestics.Infrastructure.UnitOfWork;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly CelesticsDbContext _dbContext;

    public EfUnitOfWork(
        CelesticsDbContext dbContext,
        IPartnerRepository partners,
        IMerchantRepository merchants,
        ITransactionRepository transactions)
    {
        _dbContext = dbContext;
        Partners = partners;
        Merchants = merchants;
        Transactions = transactions;
    }
    
    public IPartnerRepository Partners { get; }

    public IMerchantRepository Merchants { get; }

    public ITransactionRepository Transactions { get; }

    public async Task<int> CommitAsync(CancellationToken ct = default)
    {
        return await _dbContext.SaveChangesAsync(ct);
    }
}
