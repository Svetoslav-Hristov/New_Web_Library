using Microsoft.EntityFrameworkCore;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data;
using New_Web_Library.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Library.Data.Repository
{
    public class BooksRepository :BaseRepository,IBooksRepository
    {
        public BooksRepository(LibraryDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<List<string>> GetAllAuthors()
        {
            var authors = await _dbContext.Books.AsNoTracking().Select(b => b.Author)
                .Distinct().ToListAsync();

            return authors;
        }

        public IQueryable<Book> GetAllBooks()
        {
            var allBooks = _dbContext.Books.AsNoTracking()
                 .OrderBy(b => b.Title).ThenBy(b => b.Author);


            return allBooks;

        }

        public async Task<Book?> GetByIdAsync(Guid id) 
        {
            return await _dbContext.Books.FindAsync(id);
        }

        public async Task<bool> IsExistBook(Guid bookId)
        {
           return await _dbContext.Books.AsNoTracking().AnyAsync(b => b.Id == bookId);
        }
    }
}
