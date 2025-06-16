using Celestics.Application.Repositories;
using Celestics.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Celestics.Infrastructure.Repositories;

public class PartnerRepository : IPartnerRepository
{
    private readonly CelesticsDbContext _dbContext;
    public PartnerRepository(CelesticsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Partner> Query()
    {
        return _dbContext.Partners.AsNoTracking();
    }

    public async Task<Partner?> GetByIdAsync(Guid id, bool includeMerchants = false, CancellationToken ct = default)
    {
        IQueryable<Partner> query = _dbContext.Partners;

        if (includeMerchants)
        {
            query = query.Include(merchant => merchant.Merchants);
        }

        return await query.AsNoTracking().FirstOrDefaultAsync(partner => partner.Id == id, ct);
    }
}
