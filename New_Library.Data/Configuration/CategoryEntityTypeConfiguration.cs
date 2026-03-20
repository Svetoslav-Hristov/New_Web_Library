using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using New_Library.Data.Models.Forum;

namespace New_Library.Data.Configuration
{
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        private readonly Category[] categories =

        {
            new Category {
                Id = 1,
                Name = "Modern Literature",
                Description = "Modern literary works"
            },
            new Category {
                Id = 2,
                Name = "Classical Literature",
                Description = "Timeless classics"
            },
            new Category {
                Id = 3,
                Name = "Poetry",
                Description = "Poems and verse"
            },
            new Category {
                Id = 4,
                Name = "Fantasy",
                Description = "Fantasy worlds and stories"
            },
            new Category {
            Id = 5,
            Name = "Science Fiction",
            Description = "Sci-fi adventures and futuristic stories"
            
            },
            new Category {
            Id = 6,
            Name = "Historical Fiction",
            Description = "Stories set in historical periods"
           
           },
           new Category {
           Id = 7,
           Name = "Mystery & Thriller",
           Description = "Suspenseful and mysterious stories"
            
           },
           new Category {
           Id = 8,
           Name = "Non-Fiction",
           Description = "Informative and factual works"
            
           }


        };



        public void Configure(EntityTypeBuilder<Category> builder)
        {

            builder.HasData(categories);

        }
    }
}
