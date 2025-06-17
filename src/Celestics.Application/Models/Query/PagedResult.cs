namespace Celestics.Application.Models.Query;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = Array.Empty<T>();

    public int TotalCount { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalPages =>
        PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
}