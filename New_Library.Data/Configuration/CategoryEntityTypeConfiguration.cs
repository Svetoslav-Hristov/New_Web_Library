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
            }


        };



        public void Configure(EntityTypeBuilder<Category> builder)
        {

            builder.HasData(categories);

        }
    }
}
