using APBDtut12.DTOs;
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

    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AddClientToTrip(int idTrip, [FromBody] AddClientTripDTO addClientTrip)
    {
        try
        {
            await _service.AddClientToTripAsync(idTrip, addClientTrip);
            return Ok("Client added");
        }
        catch (Exception e)
        {
            if(e.Message.Contains("already exists") || e.Message.Contains("already started")) return BadRequest(e.Message);
            if(e.Message.Contains("not found")) return NotFound("Client not found");
            return BadRequest(e.Message);
        }
    }
    
    
}