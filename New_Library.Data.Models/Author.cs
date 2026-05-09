using System.ComponentModel.DataAnnotations;
using static New_Web_Library.GCommon.EntityValidations.Authors;
using New_Web_Library.Data.Models.Contracts;

namespace New_Web_Library.Data.Models
{
    public class Author:IAuthor
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(AuthorNameMaxLength)]
        public string Name { get; set; } = null!;

       
        [StringLength(BiographyMaxLength)]
        public string? Biography { get; set; }

        [Url]
        [StringLength(UrlMaxLength)]
        public string? ImageUrl { get; set; }

        public List<Book> Books { get; set; } = new List<Book>();

    }
}
