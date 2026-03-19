using System.ComponentModel.DataAnnotations;
using static New_Web_Library.GCommon.EntityValidations.Topics;

namespace New_Web_Library.ViewModels.Forum
{
    public class CreateSubCategoryViewModel
    {
        [Required]
        [StringLength(TopicTitleMaxLength,MinimumLength =TopicTitleMinLength)]
        public string TopicName { get; set; } = null!;
        
        public int? SubCategoryId { get; set; }
        public int CategoryId { get; set; }
    }
}
