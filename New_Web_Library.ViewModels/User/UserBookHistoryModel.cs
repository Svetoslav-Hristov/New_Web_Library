namespace New_Web_Library.ViewModels.User
{
    public class UserBookHistoryModel
    {
        public Guid BookId { get; set; }

        public string Title { get; set; } = null!;

        public DateOnly? PickUpDate { get; set; }

        public DateOnly? ReturnDate { get; set; }

    }
}
