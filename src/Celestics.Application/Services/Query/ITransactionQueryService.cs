using Celestics.Application.Models.Query;

namespace Celestics.Application.Services.Query;

public interface ITransactionQueryService
{
    Task<PagedResult<TransactionDto>> GetTransactionsAsync(TransactionQueryParameters parameters, CancellationToken ct = default);
}
