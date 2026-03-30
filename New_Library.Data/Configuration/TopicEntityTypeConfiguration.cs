using Microsoft.EntityFrameworkCore;
using New_Library.Data.Models.Forum;

namespace New_Library.Data.Configuration
{
    public class TopicEntityTypeConfiguration : IEntityTypeConfiguration<Topic>
    {



        private readonly Topic[] topics =
        {
            new Topic {
                Id = 1,
                Title = "Best modern novels 2026",
                CategoryId = 1 ,
                CreatedOn = DateTime.UtcNow,
                UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
            },
            new Topic {
                Id = 2,
                Title = "Top 10 classical books",
                CategoryId = 2,
                CreatedOn = DateTime.UtcNow,
                UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
            },
            new Topic {
                Id = 3,
                Title = "Favorite poets",
                CategoryId = 3 ,
                CreatedOn = DateTime.UtcNow,
                UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
            },
            new Topic {
                Id = 4,
                Title = "Epic fantasy series",
                CategoryId = 4,
                CreatedOn = DateTime.UtcNow,
                UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
            },
            new Topic {
                Id = 5,
                Title = "Modern short stories",
                CategoryId = 1,
                CreatedOn = DateTime.UtcNow,
                UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
            },
            new Topic {
                Id = 6,
                Title = "Contemporary novels discussion",
                CategoryId = 1,
                CreatedOn = DateTime.UtcNow,
                UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
            },
            new Topic {
                Id = 7,
                Title = "Shakespeare's works",
                CategoryId = 2,
                CreatedOn = DateTime.UtcNow,
                UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
            },
            new Topic {
                Id = 8,
                Title = "Greek and Roman classics",
                CategoryId = 2,
                CreatedOn = DateTime.UtcNow,
                UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
            },
            new Topic {
                Id = 9,
                Title = "Future tech and space exploration",
                CategoryId = 5,
                CreatedOn = DateTime.UtcNow,
                UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
            },
            new Topic {
                Id = 10,
                Title = "World War II novels",
                CategoryId = 6,
                CreatedOn = DateTime.UtcNow,
                UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
            },
            new Topic {
                Id = 11,
                Title = "Detective series discussion",
                CategoryId = 7,
                CreatedOn = DateTime.UtcNow,
                UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
            },
        };


        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Topic> builder)
        {
            builder.HasData(topics);
        }
    }
}
