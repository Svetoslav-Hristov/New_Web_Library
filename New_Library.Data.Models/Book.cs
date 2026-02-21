using New_Web_Library.GCommon.Enums;
using New_Web_Library.Data.Models.Contracts;
using System.ComponentModel.DataAnnotations;


namespace New_Web_Library.Data.Models
{
    using static New_Web_Library.GCommon.EntityValidations.Books;
    public class Book : IBook
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        public int Year { get; set; }

        [MaxLength(URLMaxLength)]
        public string? CoverImageUrl { get; set; }

        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }
        
        [Required]
        [MaxLength(AuthorMaxLengthName)]
        public string Author { get; set; } = null!;

        public Genre Genre { get; set; }

        public ICollection<UserBook> BookUsers { get; set; } = new List<UserBook>();
    }
}
