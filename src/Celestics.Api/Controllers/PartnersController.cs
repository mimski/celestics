using Celestics.Application.Models.Query;
using Celestics.Application.Services.Query;
using Microsoft.AspNetCore.Mvc;

namespace Celestics.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class PartnersController : ControllerBase
{
    private readonly IPartnerQueryService _service;

    public PartnersController(IPartnerQueryService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retrieve paged, filterable list of partners.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<PartnerDto>>> Get([FromQuery] PartnerQueryParameters parameters, CancellationToken ct)
    {
        var result = await _service.GetPartnersAsync(parameters, ct);

        return Ok(result);
    }
}
