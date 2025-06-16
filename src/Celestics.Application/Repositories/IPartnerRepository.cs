using Celestics.Domain.Entities;

namespace Celestics.Application.Repositories;

public interface IPartnerRepository
{
    IQueryable<Partner> Query();

    Task<Partner?> GetByIdAsync(Guid id, bool includeMerchants = false, CancellationToken ct = default);
}
