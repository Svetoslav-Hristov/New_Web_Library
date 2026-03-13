using System.ComponentModel.DataAnnotations;
using static New_Web_Library.GCommon.EntityValidations.Posts;

namespace New_Web_Library.ViewModels.Forum
{
    public class ContentDetailsModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(PostTitleMaxLength,MinimumLength = PostTitleMinLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(ContentMaxLength,MinimumLength = ContentMinLength)]
        public string Content { get; set; } = null!;

        public string AuthorName { get; set; } = null!;
        public DateTime CreatedOn { get; set; }

        public Guid UserId { get; set; }
        public int UserPostCount { get; set; }
        public int UserCommentCount { get; set; }

        public bool IsAuthor { get; set; }

    }
}
