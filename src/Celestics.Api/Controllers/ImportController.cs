using Celestics.Application.Services.Import;
using Microsoft.AspNetCore.Mvc;

namespace Celestics.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Consumes("multipart/form-data")]
[Produces("application/json")]
public class ImportController : ControllerBase
{
    private readonly ITransactionImportService _importService;

    public ImportController(ITransactionImportService importService)
    {
        _importService = importService;
    }

    /// <summary>
    /// Import transactions from an XML file.
    /// </summary>
    /// <param name="merchantId">ID of the merchant to which these transactions belong.</param>
    /// <param name="file">XML file containing the &lt;Operation&gt;…&lt;/Operation&gt; payload.</param>
    [HttpPost("transactions")]
    public async Task<IActionResult> ImportTransactions(
        [FromQuery] Guid merchantId,
        IFormFile file,
        CancellationToken ct)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        if (!file.FileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("Only .xml files are supported.");
        }

        try
        {
            using var stream = file.OpenReadStream();
            var count = await _importService.ImportAsync(stream, merchantId, ct);

            return Ok(new { Imported = count });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (FormatException ex)
        {
            return BadRequest($"Invalid file format: {ex.Message}");
        }
        catch (Exception)
        {
            return StatusCode(500, "Server error during import.");
        }
    }
}
