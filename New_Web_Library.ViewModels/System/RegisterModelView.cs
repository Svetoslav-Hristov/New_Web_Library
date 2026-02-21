using New_Web_Library.GCommon.Enums;

namespace New_Web_Library.ViewModels.System
{
    public class RegisterModelView
    {
        public int LoanId { get; set; }

        public Guid UserId { get; set; }


        public string UserFirstName { get; set; } = null!;

        public string UserLastName { get; set; } = null!;

        public Guid BookId { get; set; }


        public string BookTitle { get; set; } = null!;


        public DateOnly? PickUpDate { get; set; }


        public DateOnly? ReturnDate { get; set; }


        public DateOnly? ReservedOn { get; set; }

        public DateOnly? ReservationExpiresOn { get; set; }

        public BookStatus Status { get; set; }

    }
}
