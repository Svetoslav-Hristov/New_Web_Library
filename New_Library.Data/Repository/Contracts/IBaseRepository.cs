using Microsoft.EntityFrameworkCore;
using New_Web_Library.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Library.Data.Repository.Contracts
{
    public interface IBaseRepository
    {

        Task<T?> GetByIdAsync<T>(int id) where T : class;

        Task AddAsync<T>(T entity) where T : class;

        Task UpdateAsync<T>(T entity) where T : class;

        Task DeleteAsync<T>(T entity) where T : class;
    }
}
