using APBDtut12.Data;
using Microsoft.EntityFrameworkCore;

namespace APBDtut12.Services;

public class ClientService : IClientService
{
    private readonly MasterContext _context;

    public ClientService(MasterContext context)
    {
        _context = context;
    }

    public async Task<bool> DeleteClientAsync(int clientId)
    {
        var client = await _context.Clients
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.IdClient == clientId);

        if (client == null) throw new Exception("Client not found");
        

        if (client.ClientTrips.Any()) return false;
        
        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return true;
    }
}