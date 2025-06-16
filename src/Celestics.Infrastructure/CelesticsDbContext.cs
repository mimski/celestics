using Microsoft.EntityFrameworkCore;
using Celestics.Domain.Entities;

namespace Celestics.Infrastructure;

public class CelesticsDbContext : DbContext
{
    public CelesticsDbContext(DbContextOptions<CelesticsDbContext> options)
      : base(options)
    {
    }

    public DbSet<Partner> Partners { get; set; } = default!;

    public DbSet<Merchant> Merchants { get; set; } = default!;

    public DbSet<Transaction> Transactions { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CelesticsDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
