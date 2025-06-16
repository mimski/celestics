namespace Celestics.Application.Services.Import;

public interface ITransactionImportService
{
    Task<int> ImportAsync(Stream input, Guid merchantId, CancellationToken ct = default);
}
