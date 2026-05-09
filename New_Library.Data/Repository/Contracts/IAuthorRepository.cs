using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data.Models;

namespace New_Web_Library.Data.Repository.Contracts
{
    public interface IAuthorRepository : IBaseRepository
    {
        Task<Dictionary<string,Guid>> GetAllAuthorsAsync();

        IQueryable<Author?> GetAllAuthorsFullDetailsAsync();

        Task<Author?> GetAuthorWithBooksAsync(Guid Id);

        Task<Author?> GetByIdAsync(Guid Id);

        Task<bool> ExistByName(string name);

    }
}
