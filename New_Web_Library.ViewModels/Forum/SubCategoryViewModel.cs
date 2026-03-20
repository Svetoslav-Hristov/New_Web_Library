using System.ComponentModel.DataAnnotations;
using static New_Web_Library.GCommon.EntityValidations.Categories;

namespace New_Web_Library.ViewModels.Forum
{
    public class SubCategoryViewModel
    {
        [Required]
        [StringLength(CategoryNameMaxLength,MinimumLength =CategoryNameMinLength)]
        public string CategoryName { get; set; } = null!;

        public int CategoryId { get; set; }

        public List<SubCategoryForumModel>? Posts { get; set; } = new List<SubCategoryForumModel>();

    }
}
