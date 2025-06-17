using Celestics.Application.Models.Query;
using Celestics.Application.Services.Query;
using Microsoft.AspNetCore.Mvc;

namespace Celestics.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class TransactionsController : ControllerBase
{
    private readonly ITransactionQueryService _service;

    public TransactionsController(ITransactionQueryService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retrieve paged, filterable list of transactions.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<TransactionDto>>> Get([FromQuery] TransactionQueryParameters parameters, CancellationToken ct)
    {
        if (!parameters.IsValid(out var error))
        {
            return BadRequest(new { error });
        }

        var result = await _service.GetTransactionsAsync(parameters, ct);

        return Ok(result);
    }
}
