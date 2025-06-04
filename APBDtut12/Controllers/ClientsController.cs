using APBDtut12.Services;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APBDtut12.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        try
        {
            var res = await _clientService.DeleteClientAsync(id);
            if (!res) return BadRequest("Client cannot be deleted because they are assigned to one or more trips");

            return NoContent();
        }
        catch (Exception ex)
        {
            if(ex.Message.Contains("not found")) return NotFound(ex.Message);
            
            return BadRequest(ex.Message);
        }
        
    }
}