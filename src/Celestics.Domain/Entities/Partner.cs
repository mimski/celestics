namespace Celestics.Domain.Entities;

public class Partner
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<Merchant> Merchants { get; set; } = new List<Merchant>();
}
