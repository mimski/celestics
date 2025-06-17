using Celestics.Application.Models.Query;

namespace Celestics.Application.Services.Query;

public interface IMerchantQueryService
{
    Task<PagedResult<MerchantDto>> GetMerchantsAsync(MerchantQueryParameters parameters, CancellationToken ct = default);
}