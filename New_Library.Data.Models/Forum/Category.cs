using New_Web_Library.Service.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using static New_Web_Library.GCommon.EntityValidations.Categories;

namespace New_Library.Data.Models.Forum
{
    public class Category : ICategory
    {
        [Key]
        public int Id { get ; set; }

        [Required]
        [MaxLength(CategoryNameMaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(DescriptionMaxLength)]
        public string? Description { get ; set ; }
        public bool IsDeleted { get ; set ; }

        public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();

    }
}
