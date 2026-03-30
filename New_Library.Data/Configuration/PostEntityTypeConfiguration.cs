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
            },new Post {
                Id = 5,
                Title = "Modern short story debate",
                Content = "Which modern short stories are worth reading?",
                CreatedOn = DateTime.UtcNow,
                TopicId = 1,
                UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
            },
            new Post {
                Id = 6,
                Title = "Contemporary novels insights",
                Content = "Share insights on contemporary novels you've read recently.",
                CreatedOn = DateTime.UtcNow,
                TopicId = 1,
                UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
            },

            new Post {
                Id = 7,
                Title = "Exploring classic literature",
                Content = "Let's explore the themes in classic literature.",
                CreatedOn = DateTime.UtcNow,
                TopicId = 2,
                UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
            },
            new Post {
                Id = 8,
                Title = "Favorite classic authors",
                Content = "Who are your favorite classic authors and why?",
                CreatedOn = DateTime.UtcNow,
                TopicId = 2,
                UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
            }
        };


        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasData(posts);
        }
    }
}
