namespace Domain.Enums;

public enum Action
{
    Pay = 0,
    Finish = 1, //After paid and used
    Cancel = 2, //Can't be paid anymore
    Refund = 3, //Right to refund
    Reopen = 4, //Canceled
}