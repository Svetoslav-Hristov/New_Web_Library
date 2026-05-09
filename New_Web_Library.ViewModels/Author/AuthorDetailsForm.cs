using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static New_Web_Library.GCommon.EntityValidations.Authors;

namespace New_Web_Library.ViewModels.Author
{
    public class AuthorDetailsForm
    {
        public Guid? Id { get; set; }

        [Required]
        [StringLength(AuthorNameMaxLength,MinimumLength = AuthorNameMinLength)]
        public string Name { get; set; } = null!;

        [StringLength(BiographyMaxLength,MinimumLength =BiographyMinLength)]
        public string? Biography { get; set; }

        [StringLength(UrlMaxLength)]
        public string? ImageUrl { get; set; }

        public IEnumerable<SelectListItem> Images { get; set; } = new List<SelectListItem>();  
    }
}
