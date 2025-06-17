namespace Celestics.Application.Models.Query;

public record MerchantDto(
       Guid Id,
       string Name,
       DateTime BoardingDate,
       string? Url,
       string Country,
       string Address1,
       string? Address2,
       Guid PartnerId,
       int TransactionCount
   );