using Domain.Enums;
using Action = Domain.Enums.Action;

namespace Domain.Entities;

public class Booking
{
    public int Id { get; set; }
    public DateTime PlacedAt { get; set; }
    public DateTime Start { get; set; } 
    public DateTime End { get; set; } 
    private Status Status { get; set; } = Status.Created;

    public Status CurrentStatus()
    {
        return Status;
    }

    public void ChangeState(Action action)
    {
        Status = (Status, action) switch
        {
            (Status.Created,  Action.Pay)     => Status.Paid,
            (Status.Created,  Action.Cancel)  => Status.Canceled,
            (Status.Paid,     Action.Finish)  => Status.Finished,
            (Status.Paid,     Action.Refund)  => Status.Refunded,
            (Status.Canceled, Action.Reopen)  => Status.Created,
            _=> Status
        };
    }
}