namespace New_Web_Library.Data.Models.Contracts
{
    public class IUserBook
    {
        DateOnly ReservedOn { get; set; }
        DateOnly ReservationExpiresOn { get; set; }
        DateOnly PickUpDate { get; set; }
        DateOnly ReturnDate { get; set; }
    }
}
