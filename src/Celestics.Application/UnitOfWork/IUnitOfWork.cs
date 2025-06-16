using Celestics.Application.Repositories;

namespace Celestics.Application.UnitOfWork;

public interface IUnitOfWork
{
    ITransactionRepository Transactions { get; }

    IMerchantRepository Merchants { get; }

    IPartnerRepository Partners { get; }

    Task<int> CommitAsync(CancellationToken ct = default);
}
