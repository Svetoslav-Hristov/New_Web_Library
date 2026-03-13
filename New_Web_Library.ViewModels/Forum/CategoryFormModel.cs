using System.ComponentModel.DataAnnotations;
using static New_Web_Library.GCommon.EntityValidations.Categories;

namespace New_Web_Library.ViewModels.Forum
{
    public class CategoryFormModel
    {
       
        [Required]
        [StringLength(CategoryNameMaxLength,MinimumLength =CategoryNameMinLength)]
        public string Name { get; set; } = null!;

        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }
       

    }
}
