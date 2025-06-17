using Celestics.Application.Models.Query;
using Celestics.Application.Services.Query;
using Microsoft.AspNetCore.Mvc;

namespace Celestics.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class MerchantsController : ControllerBase
{
    private readonly IMerchantQueryService _service;

    public MerchantsController(IMerchantQueryService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retrieve paged, filterable list of merchants.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<MerchantDto>>> Get([FromQuery] MerchantQueryParameters parameters, CancellationToken ct)
    {
        if (!parameters.IsValid(out var error))
        {
            return BadRequest(new { error });
        }

        var result = await _service.GetMerchantsAsync(parameters, ct);

        return Ok(result);
    }
}
