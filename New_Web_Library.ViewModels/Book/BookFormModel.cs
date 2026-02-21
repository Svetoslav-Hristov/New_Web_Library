using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace New_Web_Library.ViewModels.Book
{
    
    using static New_Web_Library.GCommon.EntityValidations.Books;
    using  New_Web_Library.GCommon.Enums;

    public class BookFormModel
    {
        [Required]
        [StringLength(TitleMaxLength,MinimumLength =TitleMinLength)]
        public string Title { get; set; } = null!;

        [Range(1500,2100)]
        public int Year { get; set; }


        [MaxLength(URLMaxLength)]
        public string? CoverImage { get; set; }

        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        public string? SelectedAuthor { get; set; }

        
        [StringLength(AuthorMaxLengthName,MinimumLength =AuthorMinLengthName)]
        public string? NewAuthor { get; set; }

        [Required]
        public Genre Genre { get; set; }

        public IEnumerable<SelectListItem> Authors { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> Genres { get; set; } = new List<SelectListItem>();


        public IEnumerable<SelectListItem> Covers { get; set; } = new List<SelectListItem>();

    }
}
