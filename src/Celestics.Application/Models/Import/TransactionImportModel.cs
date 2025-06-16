namespace Celestics.Application.Models.Import;

public sealed record TransactionImportModel
(
    string ExternalId,
    DateTime CreateDate,
    char Direction,
    decimal Value,
    string Currency,
    int Status,
    string DebtorIban,
    string BeneficiaryIban
);
