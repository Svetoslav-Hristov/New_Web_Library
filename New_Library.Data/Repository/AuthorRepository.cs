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

        public Task<bool> ExistByName(string name)
        {
            string normalized = name.Trim();


            return _dbContext.Authors.AnyAsync(a => a.Name == normalized);
           
        }

        public async Task<Dictionary<string,Guid>> GetAllAuthorsAsync()
        {
            Dictionary<string, Guid> authors = await _dbContext.Authors.AsNoTracking()
                .ToDictionaryAsync(a => a.Name, a => a.Id);
            
            return authors;

        }

        public  IQueryable<Author?> GetAllAuthorsFullDetailsAsync()
        {
            var authors = _dbContext.Authors.Include(a=>a.Books).
                AsNoTracking().OrderBy(a=>a.Name);

            return authors;
        }

        public async Task<Author?> GetAuthorWithBooksAsync(Guid Id)
        {
            return await _dbContext.Authors.Include(a=>a.Books).FirstOrDefaultAsync(a => a.Id == Id);
        
        }

        public async Task<Author?> GetByIdAsync(Guid Id)
        {
            return await _dbContext.Authors.FirstOrDefaultAsync(a => a.Id == Id);
        }

        

    }
}
