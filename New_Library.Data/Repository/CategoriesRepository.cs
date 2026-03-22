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

        public async Task<bool> ExistByName(string name)
        {
            return await _dbContext.Categories.AnyAsync(c => c.Name.ToLower() == name);
        }

        public async Task<bool> ExistByName(string name, int Id)
        {
            return await _dbContext.Categories.AnyAsync(c => c.Name.ToLower() == name && c.Id != Id);
        }

        public async Task<List<Category>> GetAllCategoriesWithSubCategories(int? Id = null)
        {

            var categories = _dbContext.Categories.AsNoTracking()
           .Include(c => c.Topics).ThenInclude(t => t.Posts)
            .ThenInclude(p => p.User).Include(c => c.Topics)
        .ThenInclude(t => t.Posts).ThenInclude(p => p.Comments)
        .ThenInclude(c => c.User).AsQueryable();


            if (Id != null)
            {
                categories = categories.Where(c => c.Id == Id);

            }

            return await categories.ToListAsync();

        }

        public  IQueryable<Category> GetAllDeleteCategories()
        {
            var deleteCategories = _dbContext.Categories.IgnoreQueryFilters().Where(c => c.IsDeleted);
                

            return  deleteCategories;

        }

        public async Task<Category?> GetDeleteOrNotCategory(int Id)
        {
            return await _dbContext.Categories.IgnoreQueryFilters().Include(c=>c.Topics).Where(c => c.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<Category?> LastCategory()
        {
            var lastCategory = await _dbContext.Categories.OrderByDescending(c => c.Id).FirstOrDefaultAsync();


            return lastCategory;

        }
    }
}
