using New_Library.Data.Models.Contracts;
using New_Web_Library.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static New_Web_Library.GCommon.EntityValidations.Posts;

namespace New_Library.Data.Models.Forum
{
    public class Post : IContent
    {
        [Key]
        public int Id { get ; set; }

        [Required]
        [MaxLength(PostTitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get ; set ; }

        [ForeignKey(nameof(Topic))]
        public int TopicId { get; set; }

        public virtual Topic Topic { get; set; } = null!;

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        public virtual User User { get; set; } = null!;

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}
