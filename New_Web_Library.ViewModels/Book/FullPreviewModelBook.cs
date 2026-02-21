using System.ComponentModel.DataAnnotations;
using New_Web_Library.GCommon.Enums;


namespace New_Web_Library.ViewModels.Book
{
  
    public class FullPreviewModelBook:PreviewBookModel
    {
        [Required]
        public string AuthorName { get; set; } = null!;

        public int YearOfPublished { get; set; }

        public Genre Genre { get; set; }

        public string? Description { get; set; }

        public BookStatus? BookStatus { get; set; }
    }
}
