using Celestics.Application.Models.Import;
using Celestics.Application.Repositories;
using Celestics.Application.Services.Import;
using Celestics.Application.Services.Parsing;
using Celestics.Application.Services.Query;
using Celestics.Application.UnitOfWork;
using Celestics.Infrastructure.Parsing;
using Celestics.Infrastructure.Repositories;
using Celestics.Infrastructure.Services.Import;
using Celestics.Infrastructure.Services.Query;
using Celestics.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Celestics.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<CelesticsDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        services.AddScoped<IPartnerRepository, PartnerRepository>();
        services.AddScoped<IMerchantRepository, MerchantRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        services.AddScoped<IFileParser<TransactionImportModel>, XmlTransactionParser>();

        services.AddScoped<ITransactionImportService, TransactionImportService>();
        services.AddScoped<ITransactionQueryService, TransactionQueryService>();
        services.AddScoped<IPartnerQueryService, PartnerQueryService>();
        services.AddScoped<IMerchantQueryService, MerchantQueryService>();

        return services;
    }
}
