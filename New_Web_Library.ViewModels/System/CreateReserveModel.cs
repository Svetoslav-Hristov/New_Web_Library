using System.ComponentModel.DataAnnotations;

namespace New_Web_Library.ViewModels.System
{

    public class CreateReserveModel
    {
        [Required]
        public Guid BookId { get; set; }

        [Required]
        public string BookTitle { get; set; } = null!; 

        
        public string? SearchingCriteria { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public string? UserName { get; set; }

        public DateOnly? ReservedOn { get; set; }

        public DateOnly? ReservationExpiresOn { get; set; }

    }
}
