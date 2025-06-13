namespace Celestics.Domain.Entities;

public class Merchant
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime BoardingDate { get; set; }

    public string? Url { get; set; }

    public string Country { get; set; } = null!;

    public string Address1 { get; set; } = null!;

    public string? Address2 { get; set; }

    public Guid PartnerId { get; set; }

    public Partner Partner { get; set; } = null!;

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
