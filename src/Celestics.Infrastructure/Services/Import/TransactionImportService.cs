using Celestics.Application.Models.Import;
using Celestics.Application.Services.Import;
using Celestics.Application.Services.Parsing;
using Celestics.Application.UnitOfWork;
using Celestics.Domain.Entities;
using Celestics.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Celestics.Infrastructure.Services.Import;

public class TransactionImportService : ITransactionImportService
{
    private readonly IFileParser<TransactionImportModel> _parser;
    private readonly IUnitOfWork _uow;
    private readonly ILogger<TransactionImportService> _logger;

    public TransactionImportService(
        IFileParser<TransactionImportModel> parser,
        IUnitOfWork uow,
        ILogger<TransactionImportService> logger)
    {
        _parser = parser;
        _uow = uow;
        _logger = logger;
    }

    public async Task<int> ImportAsync(Stream input, Guid merchantId, CancellationToken ct = default)
    {
        var merchant = await _uow.Merchants.GetByIdAsync(merchantId, includeTransactions: false, ct);

        if (merchant is null)
        {
            throw new KeyNotFoundException($"Merchant '{merchantId}' not found.");
        }

        var toAdd = new List<Transaction>();

        await foreach (var model in _parser.ParseAsync(input, ct))
        {
            ct.ThrowIfCancellationRequested();

            bool exists = await _uow.Transactions.ExistsByExternalIdAsync(model.ExternalId, ct);

            if (exists)
            {
                _logger.LogInformation("Skipping duplicate transaction {ExternalId}", model.ExternalId);

                continue;
            }

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                ExternalId = model.ExternalId,
                MerchantId = merchantId,
                CreateDate = model.CreateDate,
                Direction = model.Direction == 'C' ? TransactionDirection.Credit : TransactionDirection.Debit,
                Amount = model.Value,
                Currency = model.Currency,
                Status = model.Status == 1 ? TransactionStatus.Successful : TransactionStatus.Failed,
                DebtorIban = model.DebtorIban,
                BeneficiaryIban = model.BeneficiaryIban
            };

            toAdd.Add(transaction);
        }

        if (toAdd.Count == 0)
        {
            _logger.LogInformation("No new transactions to import for merchant {MerchantId}", merchantId);

            return 0;
        }

        await _uow.Transactions.AddRangeAsync(toAdd, ct);

        var saved = await _uow.CommitAsync(ct);

        _logger.LogInformation("Imported {Count} transactions for merchant {MerchantId}", toAdd.Count, merchantId);

        return toAdd.Count;
    }
}
