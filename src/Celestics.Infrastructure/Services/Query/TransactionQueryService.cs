using Celestics.Application.Models.Query;
using Celestics.Application.Services.Query;
using Celestics.Application.UnitOfWork;
using Celestics.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Celestics.Infrastructure.Services.Query;

public sealed class TransactionQueryService : ITransactionQueryService
{
    private readonly IUnitOfWork _uow;

    public TransactionQueryService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<PagedResult<TransactionDto>> GetTransactionsAsync(TransactionQueryParameters p, CancellationToken ct = default)
    {
        var query = _uow.Transactions.Query();

        if (p.DateFrom.HasValue)
        {
            query = query.Where(t => t.CreateDate >= p.DateFrom.Value);
        }

        if (p.DateTo.HasValue)
        {
            query = query.Where(t => t.CreateDate <= p.DateTo.Value);
        }

        if (p.AmountMin.HasValue)
        {
            query = query.Where(t => t.Amount >= p.AmountMin.Value);
        }

        if (p.AmountMax.HasValue)
        {
            query = query.Where(t => t.Amount <= p.AmountMax.Value);
        }

        if (p.Direction.HasValue)
        {
            query = query.Where(t => t.Direction == (p.Direction == 'C'
                ? TransactionDirection.Credit
                : TransactionDirection.Debit));
        }

        if (p.StatusCode.HasValue)
        {
            query = query.Where(t => t.Status == (p.StatusCode == 1
                ? TransactionStatus.Successful
                : TransactionStatus.Failed));
        }

        if (!string.IsNullOrWhiteSpace(p.DebtorIban))
        {
            query = query.Where(t => t.DebtorIban.Contains(p.DebtorIban));
        }

        if (!string.IsNullOrWhiteSpace(p.BeneficiaryIban))
        {
            query = query.Where(t => t.BeneficiaryIban.Contains(p.BeneficiaryIban));
        }

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(t => t.CreateDate)
            .Skip((p.PageNumber - 1) * p.PageSize)
            .Take(p.PageSize)
            .Select(t => new TransactionDto(
                t.Id,
                t.CreateDate,
                t.Direction,
                t.Amount,
                t.Currency,
                t.DebtorIban,
                t.BeneficiaryIban,
                t.Status,
                t.ExternalId,
                t.MerchantId
            ))
            .ToListAsync(ct);

        return new PagedResult<TransactionDto>
        {
            Items = items,
            TotalCount = total,
            PageNumber = p.PageNumber,
            PageSize = p.PageSize
        };
    }
}