using APBDtut12.DTOs;

namespace APBDtut12.Services;

public interface IClientService
{
    Task<bool> DeleteClientAsync(int clientId);
}