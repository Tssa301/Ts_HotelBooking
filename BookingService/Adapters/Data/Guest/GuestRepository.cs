﻿using Domain.Ports;

namespace Data.Guest;

public class GuestRepository : IGuestRepository
{
    private HotelDbContext _hotelDbContext;

    public GuestRepository(HotelDbContext hotelDbContext)
    {
        _hotelDbContext = hotelDbContext;
    }
    
    public Task<Domain.Entities.Guest> Get(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<int> Create(Domain.Entities.Guest guest)
    {
        _hotelDbContext.Guests?.Add(guest);
        await _hotelDbContext.SaveChangesAsync();
        return guest.Id;
    }
}