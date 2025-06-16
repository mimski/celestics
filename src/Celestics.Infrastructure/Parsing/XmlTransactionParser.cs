using Celestics.Application.Models.Import;
using Celestics.Application.Services.Parsing;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Celestics.Infrastructure.Parsing;

public class XmlTransactionParser : IFileParser<TransactionImportModel>
{
    private readonly ILogger<XmlTransactionParser> _logger;

    public XmlTransactionParser(ILogger<XmlTransactionParser> logger)
    {
        _logger = logger;
    }

    public async IAsyncEnumerable<TransactionImportModel> ParseAsync(Stream input, [EnumeratorCancellation] CancellationToken ct = default)
    {
        var settings = new XmlReaderSettings
        {
            Async = true,
            IgnoreWhitespace = true
        };

        using var reader = XmlReader.Create(input, settings);

        // Root
        await reader.MoveToContentAsync();

        if (!reader.ReadToDescendant("Transactions"))
        {
            _logger.LogError("Missing <Transactions> element – aborting parse.");

            yield break;
        }

        while (await reader.ReadAsync())
        {
            if (reader.NodeType != XmlNodeType.Element || reader.Name != "Transaction")
            {
                continue;
            }

            ct.ThrowIfCancellationRequested();

            using var transactionSubtree = reader.ReadSubtree();
            await transactionSubtree.MoveToContentAsync();

            TransactionImportModel? model = null;

            try
            {
                model = await ParseTransactionAsync(transactionSubtree, ct);
            }
            catch (FormatException ex)
            {
                _logger.LogWarning(ex, "Skipping malformed <Transaction> (line {Line})", (reader as IXmlLineInfo)?.LineNumber);

                // skip this transaction
                reader.ReadEndElement(); 

                continue;
            }

            yield return model;

            // skip this transaction
            reader.ReadEndElement();
        }
    }

    private static async Task<TransactionImportModel> ParseTransactionAsync(
        XmlReader xml, CancellationToken ct)
    {
        async Task<string> ReadElementAsync(string name)
        {
            if (!xml.ReadToDescendant(name))
            {
                throw new FormatException($"Missing <{name}>");
            }

            var text = await xml.ReadElementContentAsStringAsync();

            ct.ThrowIfCancellationRequested();

            return text;
        }

        var externalId = await ReadElementAsync("ExternalId");

        // CreateDate
        var rawDate = await ReadElementAsync("CreateDate");

        if (!DateTime.TryParse(rawDate, null, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var createDate))
        {
            throw new FormatException($"Invalid CreateDate '{rawDate}'");
        }

        // Amount block
        if (!xml.ReadToDescendant("Amount"))
        {
            throw new FormatException("Missing <Amount>");
        }

        using var amt = xml.ReadSubtree();
        await amt.MoveToContentAsync();

        var dirText = await ReadElementAsync("Direction");

        if (string.IsNullOrEmpty(dirText))
        {
            throw new FormatException("Empty <Direction>");
        }

        var direction = dirText[0];

        var valueText = await ReadElementAsync("Value");
        if (!decimal.TryParse(valueText, NumberStyles.Number,
                              CultureInfo.InvariantCulture, out var amount))
        {
            throw new FormatException($"Invalid Value '{valueText}'");
        }

        var currency = await ReadElementAsync("Currency");

        // Status
        var statusText = await ReadElementAsync("Status");
        if (!int.TryParse(statusText, out var statusCode))
        {
            throw new FormatException($"Invalid Status '{statusText}'");
        }

        // Debtor IBAN
        if (!xml.ReadToDescendant("Debtor"))
        {
            throw new FormatException("Missing <Debtor>");
        }

        using var debtor = xml.ReadSubtree();
        await debtor.MoveToContentAsync();
        var debtorIban = await ReadElementAsync("IBAN");

        // Beneficiary IBAN
        if (!xml.ReadToFollowing("Beneficiary"))
        {
            throw new FormatException("Missing <Beneficiary>");
        }

        using var benef = xml.ReadSubtree();
        await benef.MoveToContentAsync();
        var beneficiaryIban = await ReadElementAsync("IBAN");

        return new TransactionImportModel(
            ExternalId: externalId,
            CreateDate: createDate,
            Direction: direction,
            Value: amount,
            Currency: currency,
            Status: statusCode,
            DebtorIban: debtorIban,
            BeneficiaryIban: beneficiaryIban
        );
    }
}