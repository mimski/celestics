namespace Celestics.Application.Services.Parsing;

public interface IFileParser<T>
{
    IAsyncEnumerable<T> ParseAsync(Stream input, CancellationToken ct = default);
}
