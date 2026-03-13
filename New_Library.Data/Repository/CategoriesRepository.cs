using Microsoft.EntityFrameworkCore;
using New_Library.Data.Models.Forum;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data;

namespace New_Library.Data.Repository
{
    public class CategoriesRepository : BaseRepository, ICategoriesRepository
    {
        public CategoriesRepository(LibraryDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<List<Category>> GetAllCategoriesWithSubCategories(int? Id=null)
        {

            var categories = _dbContext.Categories.AsNoTracking().
              Include(c => c.Topics).ThenInclude(c => c.Posts).ThenInclude(c=>c.User).AsQueryable();

            if(Id != null)
            {
              categories = categories.Where(c => c.Id == Id);

            }
           
            return await categories.ToListAsync();

        }


    }
}
