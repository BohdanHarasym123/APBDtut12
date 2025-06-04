using APBDtut12.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBDtut12.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripService _service;

    public TripsController(ITripService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var res = await _service.GetTripsAsync(page, pageSize);
        return Ok(res);
    }
    
}