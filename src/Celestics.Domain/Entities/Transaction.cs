using Celestics.Domain.Enums;

namespace Celestics.Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; }

    public DateTime CreateDate { get; set; }

    public TransactionDirection Direction { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = null!;

    public string DebtorIban { get; set; } = null!;

    public string BeneficiaryIban { get; set; } = null!;

    public TransactionStatus Status { get; set; }

    public string ExternalId { get; set; } = null!;

    public Guid MerchantId { get; set; }

    public Merchant Merchant { get; set; } = null!;
}
