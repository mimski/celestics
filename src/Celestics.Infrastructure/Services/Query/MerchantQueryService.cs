using Celestics.Application.Models.Query;
using Celestics.Application.Services.Query;
using Celestics.Application.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Celestics.Infrastructure.Services.Query;

public sealed class MerchantQueryService : IMerchantQueryService
{
    private readonly IUnitOfWork _uow;

    public MerchantQueryService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<PagedResult<MerchantDto>> GetMerchantsAsync(MerchantQueryParameters p, CancellationToken ct = default)
    {
        var query = _uow.Merchants.Query();


        if (!string.IsNullOrWhiteSpace(p.NameContains))
        {
            query = query.Where(m => EF.Functions.Like(m.Name, $"%{p.NameContains}%"));
        }

        if (p.BoardingDateFrom.HasValue)
        {
            query = query.Where(m => m.BoardingDate >= p.BoardingDateFrom.Value);
        }

        if (p.BoardingDateTo.HasValue)
        {
            query = query.Where(m => m.BoardingDate <= p.BoardingDateTo.Value);
        }

        if (!string.IsNullOrWhiteSpace(p.CountryEquals))
        {
            query = query.Where(m => m.Country == p.CountryEquals);
        }

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderBy(m => m.Name)
            .Skip((p.PageNumber - 1) * p.PageSize)
            .Take(p.PageSize)
            .Select(m => new MerchantDto(
                m.Id,
                m.Name,
                m.BoardingDate,
                m.Url,
                m.Country,
                m.Address1,
                m.Address2,
                m.PartnerId,
                m.Transactions.Count
            ))
            .ToListAsync(ct);

        return new PagedResult<MerchantDto>
        {
            Items = items,
            TotalCount = total,
            PageNumber = p.PageNumber,
            PageSize = p.PageSize
        };
    }
}
