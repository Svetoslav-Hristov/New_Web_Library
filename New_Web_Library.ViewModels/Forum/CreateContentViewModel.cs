using System.ComponentModel.DataAnnotations;
using static New_Web_Library.GCommon.EntityValidations.Posts;

namespace New_Web_Library.ViewModels.Forum
{
    public class CreateContentViewModel
    {
        [Required]
        [StringLength(PostTitleMaxLength,MinimumLength = PostTitleMinLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(ContentMaxLength,MinimumLength =ContentMinLength)]
        public string Description { get; set; } = null!;

        public Guid UserId { get; set; }

        public int PostId { get; set; }
        
    }
}
