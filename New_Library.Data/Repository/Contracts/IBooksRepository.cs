using Microsoft.EntityFrameworkCore;
using New_Web_Library.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Library.Data.Repository.Contracts
{
    public interface IBooksRepository:IBaseRepository
    {
       

       IQueryable<Book> GetAllBooks();

         Task<Book?> GetByIdAsync(Guid id);

        Task<List<string>> GetAllAuthors();

        Task<bool> IsExistBook(Guid bookId);

        

    }
}
