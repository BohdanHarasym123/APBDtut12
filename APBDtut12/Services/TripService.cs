using APBDtut12.Data;
using APBDtut12.DTOs;
using APBDtut12.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace APBDtut12.Services;

public class TripService : ITripService
{
    private readonly MasterContext _context;

    public TripService(MasterContext context)
    {
        _context = context;
    }
    
    public async Task<PagedTripsDTO> GetTripsAsync(int page, int pageSize)
    {
        var totalTrips = await _context.Trips.CountAsync();
        var allPages = (int)Math.Ceiling(totalTrips / (double)pageSize);
        
        var trips = await _context.Trips
            .OrderByDescending(t => t.DateFrom)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .Select(t => new TripDTO
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries.Select(c => new CountryDTO
                {
                    Name = c.Name
                }).ToList(),
                Clients = t.ClientTrips.Select(c => new ClientDTO
                {
                    FirstName = c.IdClientNavigation.FirstName,
                    LastName = c.IdClientNavigation.LastName
                }).ToList()
            }).ToListAsync();

        return new PagedTripsDTO
        {
            PageNum = page,
            PageSize = pageSize,
            AllPages = allPages,
            Trips = trips
        };
    }

    public async Task AddClientToTripAsync(int tripId, AddClientTripDTO addClientTrip)
    {
        if( await _context.Clients.AnyAsync(c => c.Pesel == addClientTrip.Pesel)) throw new Exception("Client with given PESEL already exists");
        
        var trip = await _context.Trips.FirstOrDefaultAsync(t => t.IdTrip == tripId);
        if(trip == null) throw new Exception("Trip not found");
        
        if(trip.DateFrom < DateTime.Now) throw new Exception("Trip has already started");

        var newClient = new Client
        {
            FirstName = addClientTrip.FirstName,
            LastName = addClientTrip.LastName,
            Email = addClientTrip.Email,
            Telephone = addClientTrip.Telephone,
            Pesel = addClientTrip.Pesel,
        };
        
        await _context.Clients.AddAsync(newClient);
        await _context.SaveChangesAsync();

        var clientTrip = new ClientTrip
        {
            IdClient = newClient.IdClient,
            IdTrip = tripId,
            RegisteredAt = DateTime.Now,
            PaymentDate = addClientTrip.PaymentDate
        };
        
        await _context.ClientTrips.AddAsync(clientTrip);
        await _context.SaveChangesAsync();
    }
}