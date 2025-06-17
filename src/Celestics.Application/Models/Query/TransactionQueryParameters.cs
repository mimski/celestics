namespace Celestics.Application.Models.Query;

public class TransactionQueryParameters
{
    public DateTime? DateFrom { get; set; }

    public DateTime? DateTo { get; set; }

    public decimal? AmountMin { get; set; }

    public decimal? AmountMax { get; set; }

    public char? Direction { get; set; }

    public int? StatusCode { get; set; }

    public string? DebtorIban { get; set; }

    public string? BeneficiaryIban { get; set; }

    private int _pageNumber = 1;
    private int _pageSize = 20;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = (value < 1 ? 1 : value);
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value < 1 ? 1 : (value > 100 ? 100 : value));
    }

    public bool IsValid(out string? error)
    {
        if (DateFrom.HasValue && DateTo.HasValue && DateFrom > DateTo)
        {
            error = "DateFrom must be earlier than or equal to DateTo.";

            return false;
        }

        if (AmountMin.HasValue && AmountMax.HasValue && AmountMin > AmountMax)
        {
            error = "AmountMin must be less than or equal to AmountMax.";

            return false;
        }

        if (Direction.HasValue && Direction != 'D' && Direction != 'C')
        {
            error = "Direction must be either 'D' (Debit) or 'C' (Credit).";

            return false;
        }

        if (StatusCode.HasValue && StatusCode is not (0 or 1))
        {
            error = "StatusCode must be 0 (Failed) or 1 (Successful).";

            return false;
        }

        error = null;

        return true;
    }
}
