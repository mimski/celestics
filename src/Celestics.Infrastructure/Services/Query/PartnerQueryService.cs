using Celestics.Application.Models.Query;
using Celestics.Application.Services.Query;
using Celestics.Application.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Celestics.Infrastructure.Services.Query;

public sealed class PartnerQueryService : IPartnerQueryService
{
    private readonly IUnitOfWork _uow;

    public PartnerQueryService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<PagedResult<PartnerDto>> GetPartnersAsync(PartnerQueryParameters p, CancellationToken ct = default)
    {
        var query = _uow.Partners.Query();

        if (!string.IsNullOrWhiteSpace(p.NameContains))
        {
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{p.NameContains}%"));
        }

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderBy(x => x.Name)
            .Skip((p.PageNumber - 1) * p.PageSize)
            .Take(p.PageSize)
            .Select(x => new PartnerDto(
                x.Id,
                x.Name,
                x.Merchants.Count
            ))
            .ToListAsync(ct);

        return new PagedResult<PartnerDto>
        {
            Items = items,
            TotalCount = total,
            PageNumber = p.PageNumber,
            PageSize = p.PageSize
        };
    }
}
