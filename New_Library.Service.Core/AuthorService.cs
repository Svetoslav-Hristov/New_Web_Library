using Microsoft.EntityFrameworkCore;
using New_Web_Library.Data.Models;
using New_Web_Library.Data.Repository.Contracts;
using New_Web_Library.Service.Core.Interfaces;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.ViewModels.Author;

namespace New_Web_Library.Service.Core
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        public AuthorService(IAuthorRepository authorRepository)
        {
            this._authorRepository = authorRepository;

        }

        public async Task<ServiceResult<IEnumerable<AuthorsListViewModel>>> GetAllAuthorsAsync(string? search)
        {
            var allAuthors = _authorRepository.GetAllAuthorsFullDetailsAsync();

            if (!allAuthors.Any())
            {
                return new ServiceResult<IEnumerable<AuthorsListViewModel>> 
                { 
                    Success = false, 
                    ErrorMessage = "There no added authors in data base!" 
                };
                
                    
                
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower().Trim();

                allAuthors = allAuthors
                   .Where(a => a.Name.ToLower().Contains(search));

            }

            var models = await allAuthors.Select(a => new AuthorsListViewModel()
            {
                Id = a.Id,
                Name = a.Name,
                Biography = a.Biography,
                ImageUrl=a.ImageUrl


            }).ToListAsync();



            return new ServiceResult<IEnumerable<AuthorsListViewModel>> { Success = true, Data = models };


        }

        public async Task<ServiceResult<AuthorPreviewDetails>> GetAllDetailsAuthorAsync(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return new ServiceResult<AuthorPreviewDetails> { Success = false, ErrorMessage = "Invalid author Id!" };
            }

            var author = await _authorRepository.GetByIdAsync(Id);

            if (author == null)
            {
                return new ServiceResult<AuthorPreviewDetails> { Success = false, ErrorMessage = "Author not found!" };
            }

            AuthorPreviewDetails model = new AuthorPreviewDetails()
            {

                Name = author.Name,
                Biography = author.Biography ?? "No biography available.",
                ImageUrl = author.ImageUrl ?? "/AuthorImages/default.jpg"


            };

            return new ServiceResult<AuthorPreviewDetails> { Success = true, Data = model };

        }
    }
}
