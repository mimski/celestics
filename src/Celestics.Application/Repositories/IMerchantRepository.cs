using Celestics.Domain.Entities;

namespace Celestics.Application.Repositories;

public interface IMerchantRepository
{
    IQueryable<Merchant> Query();

    Task<Merchant?> GetByIdAsync(Guid id, bool includeTransactions = false, CancellationToken ct = default);
}
