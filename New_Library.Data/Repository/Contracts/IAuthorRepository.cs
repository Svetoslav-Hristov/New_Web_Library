using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data.Models;

namespace New_Web_Library.Data.Repository.Contracts
{
    public interface IAuthorRepository : IBaseRepository
    {
        Task<List<string>> GetAllAuthorsAsync();

        IQueryable<Author> GetAllAuthorsFullDetailsAsync();

        Task<Author?> GetByIdAsync(Guid Id);
    }
}
