using New_Web_Library.GCommon.Enums;

namespace New_Web_Library.ViewModels.Book
{
    public class BookPagingPreview
    {
        public string? Search { get; set; }
        public Genre? Genre { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public IEnumerable<FullPreviewModelBook> Books { get; set; }
    }
}
