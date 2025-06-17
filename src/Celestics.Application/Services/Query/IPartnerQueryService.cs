using Celestics.Application.Models.Query;

namespace Celestics.Application.Services.Query;

public interface IPartnerQueryService
{
    Task<PagedResult<PartnerDto>> GetPartnersAsync(PartnerQueryParameters parameters, CancellationToken ct = default);
}