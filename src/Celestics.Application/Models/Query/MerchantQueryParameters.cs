namespace Celestics.Application.Models.Query;

public class MerchantQueryParameters
{
    public string? NameContains { get; set; }

    public DateTime? BoardingDateFrom { get; set; }

    public DateTime? BoardingDateTo { get; set; }

    public string? CountryEquals { get; set; }

    private int _pageNumber = 1;
    private int _pageSize = 20;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? 1 : (value > 100 ? 100 : value);
    }

    public bool IsValid(out string? error)
    {
        if (BoardingDateFrom.HasValue && BoardingDateTo.HasValue && BoardingDateFrom > BoardingDateTo)
        {
            error = "BoardingDateFrom must be before BoardingDateTo.";

            return false;
        }

        error = null;

        return true;
    }
}
