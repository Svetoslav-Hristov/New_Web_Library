using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data;
using New_Web_Library.Data.Models;
using New_Web_Library.Data.Models.Contracts;
using New_Web_Library.GCommon.Enums;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels.Book;
using System.Numerics;
using static New_Web_Library.GCommon.EntityValidations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace New_Web_Library.Services.Core
{
    public class BooksService : IBooksService
    {
        private readonly IBooksRepository _booksRepository;
        private readonly ISystemsRepository _systemsRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<IBooksService> _logger;

      
        public BooksService(IBooksRepository booksRepository, IWebHostEnvironment environment,
            ISystemsRepository systemsRepository ,ILogger<IBooksService> logger )
        {
            this._booksRepository = booksRepository;
            this._environment = environment;
            this._systemsRepository = systemsRepository;
            this._logger = logger;
        }


        public async Task<BookPagingPreview> GetAllBooksOrderedByTitleThanByAuthorAscAsync
            (string? search, Genre? genre ,int page, int pageSize)
        {

            var allCollection = _booksRepository.GetAllBooks();

            IQueryable<FullPreviewModelBook> allBooks = allCollection.Select(b => new FullPreviewModelBook()
            {
                Id = b.Id,
                Title = b.Title,
                AuthorName = b.Author,
                YearOfPublished = b.Year,
                Genre = b.Genre,
                CoverImageUrl = b.CoverImageUrl

            }).OrderBy(b => b.Title).ThenBy(b => b.AuthorName);


            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower().Trim();

                allBooks = allBooks.Where(b => b.Title.ToLower().Contains(search)
                 || b.AuthorName.ToLower().Contains(search));


            }

            if (genre != null)
            {
                allBooks = allBooks.Where(b => b.Genre == genre);

            }

            int totalCount = allBooks.Count();

            var books = await allBooks.OrderBy(b => b.Title).ThenBy(b => b.AuthorName)
                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            BookPagingPreview pagingPreview = new BookPagingPreview()
            {

                Books = books,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Search = search,
                Genre = genre

            };


            return pagingPreview;
        }

        public async Task<ServiceResult<FullPreviewModelBook>> GetCurrentModelAsync(Guid Id)
        {
            if (Id == Guid.Empty)
            {

                _logger.LogWarning("Invalid book id !");

                return new ServiceResult<FullPreviewModelBook> { Success = false, ErrorMessage = "Invalid book id !" };


            }


            Book? book = await _booksRepository.GetByIdAsync(Id);

            if (book == null)
            {
                _logger.LogWarning("Book not found !");

                return new ServiceResult<FullPreviewModelBook> { Success = false, ErrorMessage = "Book not found !" };

            }

            BookStatus? bookStatus = await _systemsRepository.ReturnStatus(Id);

            

            BookStatus currentStatus = bookStatus ?? BookStatus.Returned;



            FullPreviewModelBook newBook = new FullPreviewModelBook()
            {
                Id = book.Id,
                Title = book.Title,
                YearOfPublished = book.Year,
                AuthorName = book.Author,
                Description = book.Description,
                Genre = book.Genre,
                BookStatus = currentStatus,
                CoverImageUrl = book.CoverImageUrl
            };

            return new ServiceResult<FullPreviewModelBook> { Success = true, Data = newBook };

        }

        public async Task<ServiceResult<BookFormModel>> GetEmptyModelBookFormWithLoadedTypesAsync()
        {

            BookFormModel model = new BookFormModel();

            await BookModelDataFillingAsync(model);


            return new ServiceResult<BookFormModel> { Success = true, Data = model };

        }

        public async Task<ServiceResult<Book>> CreateNewBookUsingBookFormModelAsync(BookFormModel model)
        {

            if (model == null)
            {
               

               
                return new ServiceResult<Book>
                {
                    Success = false,
                    ErrorMessage = "Invalid book data."
                };
            }


            string? authorName = null;

            if (!string.IsNullOrEmpty(model.NewAuthor))
            {
                authorName = model.NewAuthor.Trim();
            }

            else
            {
                authorName = model.SelectedAuthor;
            }

            if (authorName == null)
            {
                
                return new ServiceResult<Book> 
                {
                    Success = false,
                    ErrorMessage = "Тhe book must have an author!"
                };
            }



            Book newBook = new Book
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Year = model.Year,
                CoverImageUrl = model.CoverImage,
                Description = model.Description,
                Author = authorName,
                Genre = model.Genre

            };

            try
            {
                await _booksRepository.AddAsync(newBook);
                

            }
            catch(Exception ex)
            {
                
                _logger.LogError(ex,"Error creating book with title {Title}", newBook.Title);

                return new ServiceResult<Book> 
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while create new book! Please try again later."
                };


            }


            return new ServiceResult<Book> { Success = true, Data = newBook };

        }

        public async Task<ServiceResult<BookFormModel>> EditBookUsingBookFormModelAsync(Guid Id)
        {

            if (Id == Guid.Empty)
            {
                return new ServiceResult<BookFormModel> { Success = false, ErrorMessage = "Not found!" };

            }


            Book? book = await _booksRepository.GetByIdAsync(Id);

            if (book == null)
            {
                return new ServiceResult<BookFormModel> { Success = false, ErrorMessage = "Book not found !" };
               
            }


            BookFormModel model = new BookFormModel()
            {
                Title = book.Title,
                Year = book.Year,
                CoverImage = book.CoverImageUrl,
                Description = book.Description,
                SelectedAuthor = book.Author,
                Genre = book.Genre,

            };

            await BookModelDataFillingAsync(model);

            return new ServiceResult<BookFormModel> { Success = true, Data = model };

        }

        public async Task<ServiceResult<Book>> ConfirmEditChangesUsingBookFormModelAsync(Guid Id, BookFormModel model)
        {
            if (Id == Guid.Empty)
            {
                return new ServiceResult<Book>
                {
                    Success = false,
                    ErrorMessage = "Invalid book id."
                };
            }


            string? authorName = null;

            if (!string.IsNullOrEmpty(model.NewAuthor))
            {
                authorName = model.NewAuthor.Trim();
            }

            else
            {
                authorName = model.SelectedAuthor;
            }

            if (authorName == null)
            {
                return new ServiceResult<Book> { Success = false, ErrorMessage = "Тhe book must have an author!" };
            }


            Book? book = await _booksRepository.GetByIdAsync(Id);

            if (book == null )
            {
                return new ServiceResult<Book> 
                {
                    Success = false,
                    ErrorMessage = "The book you are trying to edit is missing." 
                };
            
            }


            try
            {

                book.Title = model.Title;
                book.Year = model.Year;
                book.CoverImageUrl = model.CoverImage ?? book.CoverImageUrl;
                book.Description = model.Description;
                book.Author = authorName;
                book.Genre = model.Genre;

                

                await _booksRepository.UpdateAsync(book);

            }
            catch(Exception ex)
            {

                _logger.LogError(ex, "Error edit book with title {Title}", book.Title);

                return new ServiceResult<Book> 
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while edit book! Please try again later." 
                };
                

            }

            return new ServiceResult<Book> { Success = true, Data = book };


        }

        public async Task <ServiceResult<bool>> DeleteCurrentBookAsync(Guid Id)
        {
            
            
            if (Id == Guid.Empty)
            {
                return new ServiceResult<bool> { Success = false, ErrorMessage = "Not found !" };

            }


            Book? foundBook = await _booksRepository.GetByIdAsync(Id);

            if (foundBook == null)
            {
               return new ServiceResult<bool> 
               {
                   Success = false,
                   ErrorMessage = "The book you are trying to delete is missing !"
               };
            
            }

            var isTaken = await _systemsRepository.IsTakenBook(Id);

            if (isTaken)
            {
               return new ServiceResult<bool> 
               { 
                   Success = false, 
                   ErrorMessage = "Book cannot be deleted because it is currently taken." 
               
               };
           

            }


            try
            {


                await _booksRepository.DeleteAsync(foundBook);
            
            }
            catch(Exception ex)
            {

                _logger.LogError(ex, "Error edit book with title {Title}", foundBook.Title);

                return new ServiceResult<bool> 
                { 
                    Success = false,
                    ErrorMessage = "Unexpected error occurred! Please try again later." 
                
                };

            }


            return new ServiceResult<bool> { Success = true };

        }

        public async Task BookModelDataFillingAsync(BookFormModel model)
        {
            List<string> authors = await _booksRepository.GetAllAuthors();

            model.Authors = authors.Select(a => new SelectListItem 
            {
                Text = a,
                Value = a 
            });

            model.Genres = Enum.GetValues(typeof(Genre)).Cast<Genre>()
            .Select(g => new SelectListItem
            {
                Text = g.ToString(),
                Value = g.ToString()
            }).ToList();

            model.Covers = Directory.GetFiles(Path.Combine(_environment.WebRootPath, "images"))
                .Select(f => Path.GetFileName(f)).Select(f => new SelectListItem
                {
                    Text = f,
                    Value = f
                }).ToList();


        }

    }
}
