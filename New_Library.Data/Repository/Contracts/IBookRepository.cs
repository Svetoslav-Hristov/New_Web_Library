using Microsoft.EntityFrameworkCore;
using New_Web_Library.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Library.Data.Repository.Contracts
{
    public interface IBookRepository:IBaseRepository
    {
       

       IQueryable<Book> GetAllBooks();

         Task<Book?> GetByIdAsync(Guid id);


        Task<bool> IsExistBookAsync(Guid bookId);

        

    }
}
