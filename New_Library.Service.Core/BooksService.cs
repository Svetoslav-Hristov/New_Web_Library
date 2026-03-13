using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data;
using New_Web_Library.Data.Models;
using New_Web_Library.GCommon.Enums;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels.Book;

namespace New_Web_Library.Services.Core
{
    public class BooksService : IBooksService
    {
        private readonly IBooksRepository _booksRepository;
        private readonly ISystemsRepository _systemsRepository;
        private readonly IWebHostEnvironment _environment;
      
        public BooksService(IBooksRepository booksRepository, IWebHostEnvironment environment,
            ISystemsRepository systemsRepository)
        {
            this._booksRepository = booksRepository;
            this._environment = environment;
            this._systemsRepository = systemsRepository;
        }


        public async Task<IEnumerable<FullPreviewModelBook>> GetAllBooksOrderedByTitleThanByAuthorAscAsync(string? search, Genre? genre)
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

            IEnumerable<FullPreviewModelBook> books = await allBooks.ToArrayAsync();

            return books;
        }

        public async Task<ServiceResult<FullPreviewModelBook>> GetCurrentModelAsync(Guid Id)
        {
            if (Id == Guid.Empty)
            {

                return new ServiceResult<FullPreviewModelBook> { Success = false, ErrorMessage = "Invalid book id !" };


            }


            Book? book = await _booksRepository.GetByIdAsync(Id);

            if (book == null)
            {
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
                return new ServiceResult<Book> { Success = false, ErrorMessage = "Тhe book must have an author!" };
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
            catch(Exception)
            {
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
            catch(Exception)
            {

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
            catch(Exception)
            {
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
