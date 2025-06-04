using APBDtut12.Data;
using Microsoft.EntityFrameworkCore;

namespace APBDtut12.Services;

public class ClientService : IClientService
{
    private readonly MasterContext _dbContext;

    public ClientService(MasterContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> DeleteClientAsync(int clientId)
    {
        var client = await _dbContext.Clients
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.IdClient == clientId);

        if (client == null) throw new Exception("Client not found");
        

        if (client.ClientTrips.Any()) return false;
        
        _dbContext.Clients.Remove(client);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}