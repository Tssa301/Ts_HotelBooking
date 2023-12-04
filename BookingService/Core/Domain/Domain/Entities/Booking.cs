using Domain.Enums;

namespace Domain.Entities;

public class Booking
{
    public int Id { get; set; }
    public DateTime PlacedAt { get; set; }
    public DateTime Start { get; set; } 
    public DateTime End { get; set; } 
    private Status Status { get; set; }

    public Status CurrentStatus()
    {
        return this.Status;
    }
}