using Domain.Enums;
using Action = Domain.Enums.Action;


namespace DomainTests.Booking;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public void ShouldAlwaysStartWith_CreatedStatus()
    {
        var booking = new Domain.Entities.Booking();
        
        Assert.That(booking.CurrentStatus(), Is.EqualTo(Status.Created));
    }

    [Test]
    public void ShouldSetStatusToPaidWhenPayingForABookingWith_CreatedStatus()
    {
        var booking = new Domain.Entities.Booking();
        
        booking.ChangeState(Action.Pay);
        
        Assert.That(booking.CurrentStatus(), Is.EqualTo(Status.Paid));
    }
    
    [Test]
    public void ShouldSetStatusToCanceledWhenCancellationForABookingWith_CreatedStatus()
    {
        var booking = new Domain.Entities.Booking();
        
        booking.ChangeState(Action.Cancel);
        
        Assert.That(booking.CurrentStatus(), Is.EqualTo(Status.Canceled));
    }
    
    [Test]
    public void ShouldSetStatusToFinishedWhenFinishingAPaidBooking()
    {
        var booking = new Domain.Entities.Booking();
        
        booking.ChangeState(Action.Pay);
        booking.ChangeState(Action.Finish);
        
        Assert.That(booking.CurrentStatus(), Is.EqualTo(Status.Finished));
        
    }

    [Test]
    public void ShouldSetStatusToRefundedWhenRefundingAPaidBooking()
    {
        var booking = new Domain.Entities.Booking();
        
        booking.ChangeState(Action.Pay);
        booking.ChangeState(Action.Refund);
        
        Assert.That(booking.CurrentStatus(), Is.EqualTo(Status.Refunded));
    }

    [Test]
    public void ShouldSetStatusToCreatedWhenReopeningACanceledBooking()
    {
        var booking = new Domain.Entities.Booking();
        
        booking.ChangeState(Action.Cancel);
        booking.ChangeState(Action.Reopen);
        
        Assert.That(booking.CurrentStatus(), Is.EqualTo(Status.Created));
    }
    
    /************ Negative Tests ************/

    [Test]
    public void ShouldNotChangeStatusWhenRefundingABookingWithCreatedStatus()
    {
        var booking = new Domain.Entities.Booking();
        
        booking.ChangeState(Action.Refund);
        
        Assert.That(booking.CurrentStatus(), Is.EqualTo(Status.Created));
    }

    [Test]
    public void ShouldNotChangeStatusWhenRefundingAFinishedBooking()
    {
        var booking = new Domain.Entities.Booking();
        
        booking.ChangeState(Action.Pay);
        booking.ChangeState(Action.Finish);
        booking.ChangeState(Action.Refund);
        
        Assert.That(booking.CurrentStatus(),Is.EqualTo(Status.Finished));
    }
    
    [Test]
    public void ShouldNotChangeStatusWhenBookingIsNotPaid()
    {
        var booking = new Domain.Entities.Booking();
        
        booking.ChangeState(Action.Finish);
        
        Assert.That(booking.CurrentStatus(),Is.EqualTo(Status.Created));
    }
    
    [Test]
    public void ShouldNotChangeStatusToReopeningWhenBookingStatusCreated()
    {
        var booking = new Domain.Entities.Booking();
        
        booking.ChangeState(Action.Reopen);
        
        Assert.That(booking.CurrentStatus(),Is.EqualTo(Status.Created));
    }
}