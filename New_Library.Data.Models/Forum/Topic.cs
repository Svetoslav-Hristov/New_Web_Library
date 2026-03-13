using New_Web_Library.Data.Models;
using New_Web_Library.Service.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static New_Web_Library.GCommon.EntityValidations.Topics;

namespace New_Library.Data.Models.Forum
{
    public class Topic : ITopic
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(TopicTitleMaxLength)]
        public string Title { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
