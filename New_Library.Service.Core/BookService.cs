using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data.Models;
using New_Web_Library.Data.Repository.Contracts;
using New_Web_Library.GCommon.Enums;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels.Book;

namespace New_Web_Library.Services.Core
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _booksRepository;
        private readonly ISystemRepository _systemsRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<IBookService> _logger;
        private readonly IAuthorRepository _authorRepository;
      
        public BookService(IBookRepository booksRepository, IWebHostEnvironment environment,
            ISystemRepository systemsRepository ,ILogger<IBookService> logger,IAuthorRepository authorRepository)
        {
            this._booksRepository = booksRepository;
            this._environment = environment;
            this._systemsRepository = systemsRepository;
            this._authorRepository = authorRepository;
            this._logger = logger;
        }


        public  Task<BookPagingPreview> GetAllBooksOrderedByTitleThanByAuthorAscAsync
            (string? search, Genre? genre ,int page, int pageSize)
        {

            var allCollection = _booksRepository.GetAllBooks();

            IQueryable<FullPreviewModelBook> allBooks = allCollection.Select(b => new FullPreviewModelBook()
            {
                Id = b.Id,
                Title = b.Title,
                AuthorName = b.Author.Name,
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

            var books =  allBooks.OrderBy(b => b.Title).ThenBy(b => b.AuthorName)
                .Skip((page - 1) * pageSize).Take(pageSize).ToList();

            BookPagingPreview pagingPreview = new BookPagingPreview()
            {

                Books = books,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Search = search,
                Genre = genre

            };


            return Task.FromResult(pagingPreview);
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

            BookStatus? bookStatus = await _systemsRepository.ReturnStatusAsync(Id);

            

            BookStatus currentStatus = bookStatus ?? BookStatus.Returned;



            FullPreviewModelBook newBook = new FullPreviewModelBook()
            {
                Id = book.Id,
                Title = book.Title,
                YearOfPublished = book.Year,
                AuthorId=book.Author.Id,
                HasBiography=!string.IsNullOrWhiteSpace(book.Author.Biography),
                HasImage=!string.IsNullOrWhiteSpace(book.Author.ImageUrl),
                AuthorName = book.Author.Name,
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


            if (model.SelectedAuthor == Guid.Empty)
            {
                return new ServiceResult<Book> { Success = false, ErrorMessage = "Invalid author Id!" };
            }

            var author = await _authorRepository.GetByIdAsync(model.SelectedAuthor);

            

            if (author == null)
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
                AuthorId=model.SelectedAuthor,
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
                return new ServiceResult<BookFormModel> { Success = false, ErrorMessage = "Invalid book id!" };

            }


            Book? book = await _booksRepository.GetByIdAsync(Id);

            if (book == null)
            {
                return new ServiceResult<BookFormModel> { Success = false, ErrorMessage = "Book not found!" };
               
            }


            BookFormModel model = new BookFormModel()
            {
                Title = book.Title,
                Year = book.Year,
                CoverImage = book.CoverImageUrl,
                Description = book.Description,
                SelectedAuthor = book.AuthorId,
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
                    ErrorMessage = "Invalid book id!"
                };
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
            
            if (model.SelectedAuthor == Guid.Empty)
            {
                return new ServiceResult<Book> { Success = false, ErrorMessage = "Invalid author Id" };
            }

            var author = await _authorRepository.GetByIdAsync(model.SelectedAuthor);
          

            if (author == null)
            {
                return new ServiceResult<Book> { Success = false, ErrorMessage = "The book must have an author!" };
            }





            try
            {
                bool isDifferent = book.Title != model.Title || book.Year != model.Year || book.CoverImageUrl != model.CoverImage
                    || book.Description != model.Description || book.AuthorId != model.SelectedAuthor || book.Genre != model.Genre;

               
                if (isDifferent)
                {
                    book.Title = model.Title;
                    book.Year = model.Year;
                    book.CoverImageUrl = model.CoverImage ?? book.CoverImageUrl;
                    book.Description = model.Description;
                    book.AuthorId = model.SelectedAuthor;
                    book.Author = author;
                    book.Genre = model.Genre;

                    await _booksRepository.UpdateAsync(book);

                    return new ServiceResult<Book> { Success = true, Data = book };

                }
                else
                {
                    return new ServiceResult<Book> { Success = false,ErrorMessage="No changes detected!" };

                } 

               

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

        }

        public async Task <ServiceResult<bool>> DeleteCurrentBookAsync(Guid Id)
        {
            
            
            if (Id == Guid.Empty)
            {
                return new ServiceResult<bool> { Success = false, ErrorMessage = "Not found!" };

            }


            Book? foundBook = await _booksRepository.GetByIdAsync(Id);

            if (foundBook == null)
            {
               return new ServiceResult<bool> 
               {
                   Success = false,
                   ErrorMessage = "The book you are trying to delete is missing!"
               };
            
            }

            var isTaken = await _systemsRepository.IsTakenBookAsync(Id);

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


            return new ServiceResult<bool> { Success = true,Data=true };

        }

        public async Task BookModelDataFillingAsync(BookFormModel model)
        {
            Dictionary<string,Guid> authors = await _authorRepository.GetAllAuthorsAsync();

            model.Authors = authors.OrderBy(a=>a.Key).Select(a => new SelectListItem 
            {
                Text = a.Key,
                Value = a.Value.ToString() 
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
