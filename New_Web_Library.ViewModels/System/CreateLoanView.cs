using Microsoft.AspNetCore.Mvc.Rendering;

namespace New_Web_Library.ViewModels.System
{
    public class CreateLoanView 
    {

        public Guid UserId { get; set; }

        public Guid BookId { get; set; }

        public DateOnly PickUpDate { get; set; }

        public DateOnly ReturnDate { get; set; }

        public IEnumerable<SelectListItem> UsersList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> BookList { get; set; } = new List<SelectListItem>();

    }
}
