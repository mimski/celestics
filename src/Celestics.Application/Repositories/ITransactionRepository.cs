using Celestics.Domain.Entities;

namespace Celestics.Application.Repositories;

public interface ITransactionRepository
{
    IQueryable<Transaction> Query();

    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<bool> ExistsByExternalIdAsync(string externalId, CancellationToken ct = default);

    Task AddRangeAsync(IEnumerable<Transaction> transactions, CancellationToken ct = default);
}
