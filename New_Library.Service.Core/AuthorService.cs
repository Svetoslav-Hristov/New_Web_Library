using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data.Models;
using New_Web_Library.Data.Repository.Contracts;
using New_Web_Library.Service.Core.Interfaces;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.ViewModels.Author;
using New_Web_Library.ViewModels.Book;

namespace New_Web_Library.Service.Core
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<IAuthorService> _logger;
        public AuthorService(IAuthorRepository authorRepository,IBookRepository bookRepository, 
            IUserRepository userRepository, IWebHostEnvironment environment, ILogger<IAuthorService> logger)
        {
            this._authorRepository = authorRepository;
            this._bookRepository = bookRepository;
            this._userRepository = userRepository;
            this._environment = environment;
            this._logger = logger;

        }

        public void AuthorModelImageFiling(AuthorDetailsForm model)
        {

            model.Images = Directory.GetFiles(Path.Combine(_environment.WebRootPath, "AuthorImages"))
                            .Select(Path.GetFileName)
                             .Select(x => new SelectListItem
                             {
                                 Value = "/AuthorImages/" + x,
                                 Text = x
                             }).ToList();



        }

        public async Task<ServiceResult<AuthorDetailsForm>> EditAuthorProfileAsync(Guid Id, Guid changerId)
        {
            if (Id == Guid.Empty)
            {
                return new ServiceResult<AuthorDetailsForm> { Success = false, ErrorMessage = "Invalid author Id!" };
            }

            if (changerId == Guid.Empty)
            {
                return new ServiceResult<AuthorDetailsForm> { Success = false, ErrorMessage = "Invalid Id!" };
            }

            var author = await _authorRepository.GetByIdAsync(Id);

            if (author == null)
            {
                return new ServiceResult<AuthorDetailsForm> { Success = false, ErrorMessage = "Author not found!" };
            }

            var isAdmin = await _userRepository.AdminOrNotAsync(changerId);

            if (!isAdmin)
            {
                return new ServiceResult<AuthorDetailsForm>
                {
                    Success = false,
                    ErrorMessage = "You don't have permission to change author profile!"
                };

            }

            AuthorDetailsForm model = new AuthorDetailsForm()
            {
                Id = Id,
                Name = author.Name,
                Biography = author.Biography,
                ImageUrl = author.ImageUrl,
                


            };

            AuthorModelImageFiling(model);

            return new ServiceResult<AuthorDetailsForm> { Success = true, Data = model };


        }

        public async Task<ServiceResult<bool>> ConfirmEditAuthorProfileAsync(AuthorDetailsForm model, Guid Id, Guid changerId)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return new ServiceResult<bool> { Success = false, ErrorMessage = "Author must have name!" };

            }

            if (Id == Guid.Empty)
            {
                return new ServiceResult<bool> { Success = false, ErrorMessage = "Invalid author Id!" };
            }

            var author = await _authorRepository.GetByIdAsync(Id);

            if (author == null)
            {
                return new ServiceResult<bool> { Success = false, ErrorMessage = "Author not found!" };
            }

            var isAdmin = await _userRepository.AdminOrNotAsync(changerId);

            if (changerId == Guid.Empty || !isAdmin)
            {
                return new ServiceResult<bool> { Success = false, ErrorMessage = "You don't have permission to edit author profile! " };

            }

            try
            {

                bool isChanged = model.Name.Trim() != author.Name || model.Biography != author.Biography || model.ImageUrl != author.ImageUrl;

                if (!isChanged)
                {
                    return new ServiceResult<bool> { Success = false, ErrorMessage = "No changes detected!" };
                }

                author.Name = model.Name.Trim();
                author.Biography = model.Biography;
                author.ImageUrl = model.ImageUrl;

                await _authorRepository.UpdateAsync(author);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error edit author profile with name {Name}", author.Name);

                return new ServiceResult<bool>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while edit author profile! Please try again later."
                };


            }

            return new ServiceResult<bool> { Success = true };

        }

        public async Task<ServiceResult<IEnumerable<AuthorPreviewDetails>>> GetAllAuthorsAsync(string? search)
        {
            var allAuthors = _authorRepository.GetAllAuthorsFullDetailsAsync();

            if (!allAuthors.Any())
            {
                return new ServiceResult<IEnumerable<AuthorPreviewDetails>>
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

            var models = await allAuthors.Select(a => new AuthorPreviewDetails()
            {
                Id = a.Id,
                Name = a.Name,
                CountBooks=a.Books.Count(),
                Biography = a.Biography,
                ImageUrl = a.ImageUrl,
              
                


            }).ToListAsync();



            return new ServiceResult<IEnumerable<AuthorPreviewDetails>> { Success = true, Data = models };


        }

        public async Task<ServiceResult<AuthorPreviewDetails>> GetAllDetailsAuthorAsync(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return new ServiceResult<AuthorPreviewDetails> { Success = false, ErrorMessage = "Invalid author Id!" };
            }

            var author = await _authorRepository.GetAuthorWithBooksAsync(Id);

            if (author == null)
            {
                return new ServiceResult<AuthorPreviewDetails> { Success = false, ErrorMessage = "Author not found!" };
            }

            AuthorPreviewDetails model = new AuthorPreviewDetails()
            {
                Id = Id,
                Name = author.Name,
                Biography = author.Biography ?? "No biography available.",
                ImageUrl = author.ImageUrl ?? "/AuthorImages/default.jpg",
                CountBooks=author.Books.Count,
                Books=author.Books.Select(b =>new PreviewBookModel
                {
                    Id=b.Id,
                    Title=b.Title,
                    CoverImageUrl=b.CoverImageUrl?? "/Images/default.jpg"


                }).OrderBy(b=>b.Title).ToList()

            };

            return new ServiceResult<AuthorPreviewDetails> { Success = true, Data = model };

        }

        public async Task<ServiceResult<AuthorDetailsForm>> CreateNewAuthorProfileAsync(Guid creatorId)
        {
            if (creatorId == Guid.Empty)
            {
                return new ServiceResult<AuthorDetailsForm> { Success = false, ErrorMessage = "Invalid Id!" };
            }

            var isAdmin = await _userRepository.AdminOrNotAsync(creatorId);

            if (!isAdmin)
            {
                return new ServiceResult<AuthorDetailsForm>
                {
                    Success = false,
                    ErrorMessage = "You don't have permission to create author profile!"
                };
            }

            AuthorDetailsForm model = new AuthorDetailsForm();

            AuthorModelImageFiling(model);

            return new ServiceResult<AuthorDetailsForm> { Success = true, Data = model};

        }

        public async Task<ServiceResult<Guid>> ConfirmNewAuthorProfileAsync(AuthorDetailsForm model, Guid creatorId)
        {
            if (creatorId == Guid.Empty)
            {
                return new ServiceResult<Guid> { Success = false, ErrorMessage = "Invalid Id!" };
            }

            var isAdmin = await _userRepository.AdminOrNotAsync(creatorId);

            if (!isAdmin)
            {
                return new ServiceResult<Guid>
                {
                    Success = false,
                    ErrorMessage = "You don't have permission to create author profile!"
                };
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return new ServiceResult<Guid> { Success = false, ErrorMessage = "Author must have name!" };
            }

            bool existAuthor = await _authorRepository.ExistByName(model.Name);

            if (existAuthor)
            {
                return new ServiceResult<Guid>
                {
                    Success = false,
                    ErrorMessage = "Already exist author with same name!"
                };
            }


            try
            {
                if (model.Biography != null)
                {
                    model.Biography = model.Biography.Trim();
                }

                Author newAuthor = new Author()
                {
                    Name = model.Name.Trim(),
                    Biography = model.Biography,
                    ImageUrl = model.ImageUrl
                };

                await _authorRepository.AddAsync(newAuthor);

                return new ServiceResult<Guid> { Success = true, Data = newAuthor.Id};

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error create new author profile name {Name}", model.Name);

                return new ServiceResult<Guid>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while create author profile! Please try again later."
                };



            }

            
        }

        public async Task<ServiceResult<bool>> HardDeleteAuthorProfileAsync(Guid Id,Guid changerId)
        {
            if (Id == Guid.Empty)
            {
                return new ServiceResult<bool> { Success = false, ErrorMessage = "Invalid author id!" };
            }

            var author = await _authorRepository.GetByIdAsync(Id);

            if (author == null)
            {
                return new ServiceResult<bool> { Success = false, ErrorMessage = "Author not found!" };
            }

            var isAdmin = await _userRepository.AdminOrNotAsync(changerId);

            if (changerId == Guid.Empty || !isAdmin)
            {

                return new ServiceResult<bool> { Success = false, ErrorMessage = "You don't have permission to delete author profile!" };
            }

            var hasBooks = await _bookRepository.AuthorBooks(Id);

            if (hasBooks)
            {

                return new ServiceResult<bool> 
                {
                    Success=false,
                    ErrorMessage ="You can't delete this author because they have associated books." 
                };
            
            }

            try
            {
                await _authorRepository.DeleteAsync(author);


            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error delete author profile with name {Name}", author.Name);

                return new ServiceResult<bool>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while delete author profile! Please try again later."
                };


            }

            return new ServiceResult<bool> { Success = true };

        }
    }
}
