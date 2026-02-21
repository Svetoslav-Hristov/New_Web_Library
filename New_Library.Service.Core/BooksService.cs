using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        private readonly LibraryDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;
        public BooksService(LibraryDbContext dbContext, IWebHostEnvironment environment)
        {
            this._dbContext = dbContext;
            this._environment = environment;
        }


        public async Task<IEnumerable<FullPreviewModelBook>> GetAllBooksOrderedByTitleThanByAuthorAscAsync(string? search, Genre? genre)
        {
            IQueryable<FullPreviewModelBook> allBooks = _dbContext.Books.AsNoTracking().Select(b => new FullPreviewModelBook()
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


            Book? book = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == Id);

            if (book == null)
            {
                return new ServiceResult<FullPreviewModelBook> { Success = false, ErrorMessage = "Book not found !" };

            }


            BookStatus? bookStatus = await _dbContext.UsersBooks.AsNoTracking().Where(ub => ub.BookId == Id)
                .OrderByDescending(ub => ub.Id).Select(ub => (BookStatus?)ub.Status).FirstOrDefaultAsync();

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

                await _dbContext.Books.AddAsync(newBook);
                await _dbContext.SaveChangesAsync();

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


            Book? book = await _dbContext.Books.AsNoTracking().SingleOrDefaultAsync(b => b.Id == Id);

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


            Book? book = await _dbContext.Books.FindAsync(Id);

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

                await _dbContext.SaveChangesAsync();

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
            
            
            Book? foundBook = await _dbContext.Books.FindAsync(Id);

            if (foundBook == null)
            {
               return new ServiceResult<bool> 
               {
                   Success = false,
                   ErrorMessage = "The book you are trying to delete is missing !"
               };
            
            }
            
            var isTaken = await _dbContext.UsersBooks.AnyAsync(ub => ub.BookId == Id && ub.Status==BookStatus.PickedUp);

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

                _dbContext.Remove(foundBook);

                await _dbContext.SaveChangesAsync();
            
            
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

            model.Authors = await _dbContext.Books.AsNoTracking().Select(b => b.Author).Distinct()
            .Select(a => new SelectListItem { Text = a, Value = a })
            .ToListAsync();

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
