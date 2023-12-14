using Domain.ValueObjects;

namespace Domain.Entities;

public class Room
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Level { get; set; }
    public bool InMaintenance { get; set; }
    public Price Price { get; set; }

    public bool IsAvailable()
    {
        if (this.InMaintenance || HasGuest())
        {
            return false;
        }
        return true;
    }
    public bool HasGuest()
    {
        // TODO: Verificar se existem Bookings abertos para este Room.
        return true;
    }
}