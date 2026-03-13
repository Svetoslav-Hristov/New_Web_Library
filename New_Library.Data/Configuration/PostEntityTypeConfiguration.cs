using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using New_Library.Data.Models.Forum;

namespace New_Library.Data.Configuration
{
    public class PostEntityTypeConfiguration : IEntityTypeConfiguration<Post>
    {
        private readonly Post[] posts =
         {
            new Post {
                Id = 1,
                Title = "Modern novel discussion",
                Content = "Let's discuss the best modern novels of 2026.",
                CreatedOn = DateTime.UtcNow,
                TopicId = 1,
                UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
            },
            new Post {
                Id = 2,
                Title = "Classical books you love",
                Content = "Share your favorite classical books.",
                CreatedOn = DateTime.UtcNow,
                TopicId = 2,
                UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
            },
            new Post {
                Id = 3,
                Title = "Poetry recommendations",
                Content = "Which poets inspire you?",
                CreatedOn = DateTime.UtcNow,
                TopicId = 3,
                UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
            },
            new Post {
                Id = 4,
                Title = "Fantasy recommendations",
                Content = "Discuss your favorite fantasy series.",
                CreatedOn = DateTime.UtcNow,
                TopicId = 4,
                UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
            }
        };


        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasData(posts);
        }
    }
}
