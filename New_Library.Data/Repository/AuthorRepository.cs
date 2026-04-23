using Microsoft.EntityFrameworkCore;
using New_Library.Data.Repository;
using New_Web_Library.Data.Models;
using New_Web_Library.Data.Repository.Contracts;

namespace New_Web_Library.Data.Repository
{
    public class AuthorRepository : BaseRepository,IAuthorRepository
    {
        public AuthorRepository(LibraryDbContext dbContext)
            :base(dbContext)
        {
            
        }

        public async Task<List<string>> GetAllAuthorsAsync()
        {
            var authors = await _dbContext.Authors.AsNoTracking().Select(a => a.Name)
                .Distinct().ToListAsync();

            return authors;

        }

        public  IQueryable<Author> GetAllAuthorsFullDetailsAsync()
        {
            var authors = _dbContext.Authors.
                AsNoTracking().OrderBy(a=>a.Name);

            return authors;
        }

        public async Task<Author?> GetByIdAsync(Guid Id)
        {
            return await _dbContext.Authors.FirstOrDefaultAsync(a => a.Id == Id);
        }
    }
}
