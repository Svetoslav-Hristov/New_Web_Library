using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using New_Library.Data.Models.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Web_Library.Data.Configuration
{
    public class CommentEntityTypeConfiguration : IEntityTypeConfiguration<Comment>
    {

        private readonly Comment[] comments =
        {

            new Comment
        {
            Id = 1,
            Content = "I think 2026 has some really strong releases already.",
            CreatedOn = DateTime.UtcNow,
            PostId = 1,
            UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
        },
        new Comment {
            Id = 2,
            Content = "Any recommendations for modern drama novels?",
            CreatedOn = DateTime.UtcNow,
            PostId = 1,
            UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
        },
        new Comment {
            Id = 3,
            Content = "I've recently read a great psychological novel, highly recommend!",
            CreatedOn = DateTime.UtcNow,
            PostId = 1,
            UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
        },
        new Comment
        {
            Id = 4,
            Content = "Modern literature is getting more diverse, which is awesome.",
            CreatedOn = DateTime.UtcNow,
            PostId = 1,
            UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
        },
        new Comment
        {
            Id = 5,
            Content = "Do you prefer physical books or eBooks?",
            CreatedOn = DateTime.UtcNow,
            PostId = 1,
            UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
        },
        new Comment
        {
            Id = 6,
            Content = "I feel like modern novels focus more on characters than plot.",
            CreatedOn = DateTime.UtcNow,
            PostId = 1,
            UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
        },
        new Comment
        {
            Id = 7,
            Content = "Can someone suggest a good mystery novel from 2026?",
            CreatedOn = DateTime.UtcNow,
            PostId = 1,
            UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
        },
        new Comment
        {
            Id = 8,
            Content = "Audiobooks are also becoming very popular lately.",
            CreatedOn = DateTime.UtcNow,
            PostId = 1,
            UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
        },
        new Comment
        {
            Id = 9,
            Content = "I love how modern authors experiment with storytelling.",
            CreatedOn = DateTime.UtcNow,
            PostId = 1,
            UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
        },
        new Comment
        {
            Id = 10,
            Content = "Looking forward to your suggestions!",
            CreatedOn = DateTime.UtcNow,
            PostId = 1,
            UserId = new Guid("8FD866B1-9516-429A-3AAF-08DE7AB2EFC7")
        }
        };


        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasData(comments);
        }
    }
}
