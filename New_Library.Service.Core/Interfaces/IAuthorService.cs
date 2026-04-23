using New_Web_Library.Services.Core.Common;
using New_Web_Library.ViewModels;
using New_Web_Library.ViewModels.Author;

namespace New_Web_Library.Service.Core.Interfaces
{
    public interface IAuthorService
    {
        Task <ServiceResult<IEnumerable<AuthorsListViewModel>>> GetAllAuthorsAsync (string? search);

        Task<ServiceResult<AuthorPreviewDetails>> GetAllDetailsAuthorAsync(Guid Id);
    }
}
