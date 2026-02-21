using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using New_Web_Library.Data.Models.Contracts;
using New_Web_Library.GCommon.Enums;

namespace New_Web_Library.Data.Models
{
    public class UserBook : IUserBook
    {

        [Key]
        public int Id { get; set; }

        public DateOnly? ReservedOn { get; set; }

        public DateOnly? ReservationExpiresOn { get; set; }

        public DateOnly? PickUpDate { get; set; }

        public DateOnly? ReturnDate { get; set; }

        public BookStatus Status { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        public virtual User User { get; set; } = null!;

        [ForeignKey(nameof(Book))]
        public Guid BookId { get; set; }

        public virtual Book Book { get; set; } = null!;

    }
}
