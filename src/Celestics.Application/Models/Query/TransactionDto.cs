using Celestics.Domain.Enums;

namespace Celestics.Application.Models.Query;

public record TransactionDto(
        Guid Id,
        DateTime CreateDate,
        TransactionDirection Direction,
        decimal Amount,
        string Currency,
        string DebtorIban,
        string BeneficiaryIban,
        TransactionStatus Status,
        string ExternalId,
        Guid MerchantId
    );
