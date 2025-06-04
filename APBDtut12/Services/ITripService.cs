using APBDtut12.DTOs;

namespace APBDtut12.Services;

public interface ITripService
{
    Task<PagedTripsDTO> GetTripsAsync(int page, int pageSize);
    
    Task AddClientToTripAsync(int tripId, AddClientTripDTO addClientTrip);
}