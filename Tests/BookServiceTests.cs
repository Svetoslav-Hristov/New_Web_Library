using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data.Models;
using New_Web_Library.Data.Repository.Contracts;
using New_Web_Library.GCommon.Enums;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels.Book;

namespace New_Web_Library.Services.Core.Tests
{
    [TestFixture]
    public class BookServiceTests
    {
        private Mock<IBookRepository> _booksRepoMock;
        private Mock<ISystemRepository> _systemsRepoMock;
        private Mock<IWebHostEnvironment> _envMock;
        private Mock<ILogger<IBookService>> _loggerRepoMock;
        private Mock<IAuthorRepository> _authorRepoMock;

        private BookService _service;

        [SetUp]
        public void SetUp()
        {
            _booksRepoMock = new Mock<IBookRepository>();
            _systemsRepoMock = new Mock<ISystemRepository>();
            _envMock = new Mock<IWebHostEnvironment>();
            _loggerRepoMock = new Mock<ILogger<IBookService>>();
            _authorRepoMock = new Mock<IAuthorRepository>();

            _service = new BookService(_booksRepoMock.Object, _envMock.Object
                , _systemsRepoMock.Object,  _loggerRepoMock.Object,_authorRepoMock.Object);


        }


        [Test]
        public async Task GetAllBooks_ShouldReturnAll_WhenNoFilters()
        {

            var books = new List<Book>
            {
                new Book 
                { 
                    Id = Guid.NewGuid(),
                    Title = "B",
                    Author = new Author
                    {
                        Name="B" 
                    } 
                },
                new Book 
                {
                    Id = Guid.NewGuid(),
                    Title = "A",
                    Author = new Author
                    {
                        Name= "A" 
                    } 
                }
            }
            .AsQueryable();

            _booksRepoMock.Setup(x => x.GetAllBooks()).Returns(books);


            var result = await _service
                .GetAllBooksOrderedByTitleThanByAuthorAscAsync(null, null, 1, 10);


            Assert.AreEqual(2, result.Books.Count());
            Assert.AreEqual("A", result.Books.First().Title);

        }

        [Test]
        public async Task GetAllBooks_ShouldFilterBySearch()
        {
            var books = new List<Book>
        {
            new Book 
            { 
                Id = Guid.NewGuid(),
                Title = "CSharp",
                Author = new Author
                {
                    Name="Ivan" 
                }
            },
            new Book 
            {
                Id = Guid.NewGuid(),
                Title = "Java", 
                Author = new Author
                {
                    Name="Petar" 
                }
            }
        }
            .AsQueryable();

            _booksRepoMock.Setup(x => x.GetAllBooks()).Returns(books);

            var result = await _service.GetAllBooksOrderedByTitleThanByAuthorAscAsync("csharp", null, 1, 10);

            Assert.AreEqual(1, result.Books.Count());
        }


        [Test]
        public async Task GetAllBooks_ShouldFilterByGenre()
        {
            var books = new List<Book>
        {
            new Book { Id = Guid.NewGuid(), Title = "C#", Genre = Genre.Mystery },
            new Book { Id = Guid.NewGuid(), Title = "History", Genre = Genre.History }
        }
            .AsQueryable();

            _booksRepoMock.Setup(x => x.GetAllBooks()).Returns(books);

            var result = await _service.GetAllBooksOrderedByTitleThanByAuthorAscAsync(null, Genre.Mystery, 1, 10);

            Assert.AreEqual(1, result.Books.Count());
        }



        [Test]
        public async Task GetAllBooks_ShouldReturnPagedResult()
        {
            var books = new List<Book>
        {
            new Book { Id = Guid.NewGuid(), Title = "A" },
            new Book { Id = Guid.NewGuid(), Title = "B" },
            new Book { Id = Guid.NewGuid(), Title = "C" }
        }
            .AsQueryable();

            _booksRepoMock.Setup(x => x.GetAllBooks()).Returns(books);

            var result = await _service.GetAllBooksOrderedByTitleThanByAuthorAscAsync(null, null, 1, 2);

            Assert.AreEqual(2, result.Books.Count());
            Assert.AreEqual(2, result.TotalPages);
        }


        [Test]
        public async Task GetAllBooks_ShouldFilterBySearchAndGenre()
        {
            var books = new List<Book>
        {
            new Book 
            { 
                Title = "CSharp", 
                Author = new Author
                {
                    Name="Ivan" 
                },
                Genre = Genre.Horror 
            },
            new Book 
            {
                Title = "CSharp",
                Author = new Author
                {
                    Name= "Ivan" 
                },
                Genre = Genre.History 
            }
        }
            .AsQueryable();

            _booksRepoMock.Setup(x => x.GetAllBooks()).Returns(books);

            var result = await _service.GetAllBooksOrderedByTitleThanByAuthorAscAsync("csharp", Genre.Horror, 1, 10);

            Assert.AreEqual(1, result.Books.Count());
        }


        [Test]
        public async Task GetCurrentModel_ShouldReturnError_WithIncorrectId()
        {

            var id = Guid.NewGuid();

            _booksRepoMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((Book)null);



            var result = await _service.GetCurrentModelAsync(Guid.NewGuid());


            Assert.IsFalse(result.Success);

            Assert.AreEqual("Book not found !", result.ErrorMessage);



        }

        [Test]
        public async Task GetCurrentModel_ShouldReturnError_WithEmptyId()
        {


            var id = Guid.Empty;

            _booksRepoMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((Book)null);


            var result = await _service.GetCurrentModelAsync(Guid.Empty);


            Assert.IsFalse(result.Success);

            Assert.AreEqual("Invalid book id !", result.ErrorMessage);



        }

        [Test]
        public async Task GetCurrentModel_ShouldReturnCorrectResult_WhenIsValidId()
        {

            var id = Guid.NewGuid();

            var book = new Book
            {
                Id = id,
                Title = "Test Book",
                Author = new Author 
                { 
                    Name = "Ivan" 
                },
                Year = 2020,
                Description = "Test Description",
                Genre = Genre.History,
                CoverImageUrl = "url"
            };

            _booksRepoMock.Setup(x => x.GetByIdAsync(id))
             .ReturnsAsync(book);

            _systemsRepoMock.Setup(x => x.ReturnStatusAsync(id))
            .ReturnsAsync(BookStatus.Returned);

            var result = await _service.GetCurrentModelAsync(id);


            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);

            Assert.AreEqual(book.Id, result.Data.Id);
            Assert.AreEqual(book.Title, result.Data.Title);
            Assert.AreEqual(book.Author.Name, result.Data.AuthorName);
            Assert.AreEqual(book.Year, result.Data.YearOfPublished);
            Assert.AreEqual(BookStatus.Returned, result.Data.BookStatus);




        }


        [Test]
        public async Task GetCurrentModel_ShouldReturnReturnedStatus_WhenStatusIsNull()
        {
            var id = Guid.NewGuid();

            var book = new Book { Id = id, Title = "Test" };

            _booksRepoMock.Setup(x => x.GetByIdAsync(id))
             .ReturnsAsync(book);

            _systemsRepoMock.Setup(x => x.ReturnStatusAsync(id))
              .ReturnsAsync((BookStatus?)null);

            var result = await _service.GetCurrentModelAsync(id);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(BookStatus.Returned, result.Data.BookStatus);



        }

        [Test]
        public async Task GetEmptyModelBookForm_ShouldLoadAuthorsAndGenres()
        {
            _authorRepoMock.Setup(x => x.GetAllAuthorsAsync()).ReturnsAsync(new List<string> { "Ivan", "Peter" });


            var rootPath = Directory.GetCurrentDirectory();

            var imagesPath = Path.Combine(rootPath, "images");

            Directory.CreateDirectory(imagesPath);


            _envMock.Setup(x => x.WebRootPath).Returns(Directory.GetCurrentDirectory());


            var result = await _service.GetEmptyModelBookFormWithLoadedTypesAsync();


            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);


            Assert.IsNotNull(result.Data.Authors);
            Assert.IsTrue(result.Data.Authors.Any());


            Assert.IsNotNull(result.Data.Genres);
            Assert.IsTrue(result.Data.Genres.Any());

        }



        [Test]
        public async Task CreateBook_ShouldRetunError_WhenModelIsNull()
        {


            var result = await _service.CreateNewBookUsingBookFormModelAsync(null);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Invalid book data.", result.ErrorMessage);

        }

        [Test]
        public async Task CreateBook_ShouldReturnError_WhenMisssingAuthor()
        {
            BookFormModel model = new BookFormModel()
            {
                Title = "Test",
                NewAuthor = null,
                SelectedAuthor = null
            };

            var result = await _service.CreateNewBookUsingBookFormModelAsync(model);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Тhe book must have an author!", result.ErrorMessage);

        }

        [Test]
        public async Task CreateBook_ShouldReturnError_WhenRepositoryThrows()
        {
            BookFormModel model = new BookFormModel()
            {
                Title = "Test",
                NewAuthor = "Ivan"

            };

            _booksRepoMock.Setup(x => x.AddAsync(It.IsAny<Book>()))
            .ThrowsAsync(new Exception());


            var result = await _service.CreateNewBookUsingBookFormModelAsync(model);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Unexpected error is occurred while create new book! Please try again later.", result.ErrorMessage);

        }


        [Test]
        public async Task CreateBook_ShouldCreateBook_WhenNewAuthorProvided()
        {
            var model = new BookFormModel
            {
                Title = "Test",
                Year = 2020,
                NewAuthor = "Ivan",
                Genre = Genre.History
            };

            var result = await _service.CreateNewBookUsingBookFormModelAsync(model);

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);

            Assert.AreEqual("Ivan", result.Data.Author.Name);

            _booksRepoMock.Verify(x => x.AddAsync(It.IsAny<Book>()), Times.Once);
        }



        [Test]
        public async Task CreateBook_ShouldUseSelectedAuthor_WhenNoNewAuthor()
        {
            var model = new BookFormModel
            {
                Title = "Test",
                SelectedAuthor = "Peter",
                Genre = Genre.History
            };

            var result = await _service.CreateNewBookUsingBookFormModelAsync(model);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Peter", result.Data.Author.Name);
        }


        [Test]
        public async Task CreateBook_ShouldTrimNewAuthor()
        {
            var model = new BookFormModel
            {
                Title = "Test",
                NewAuthor = "  Ivan  "
            };

            var result = await _service.CreateNewBookUsingBookFormModelAsync(model);

            Assert.AreEqual("Ivan", result.Data.Author.Name);
        }


        [Test]
        public async Task EditBook_ShouldReturnModel_WhenBookExists()
        {

            var id = Guid.NewGuid();

            var book = new Book
            {
                Id = id,
                Title = "Test",
                Author = new Author 
                { 
                    Name = "Pesho" 
                },
                Year = 2020,
                Genre = Genre.History,
                Description = "Desc"
            };

            _booksRepoMock.Setup(x => x.GetByIdAsync(id))
             .ReturnsAsync(book);


            _authorRepoMock.Setup(x => x.GetAllAuthorsAsync())
             .ReturnsAsync(new List<string> { "Pesho", "Ivan" });

            _envMock.Setup(x => x.WebRootPath)
             .Returns(Directory.GetCurrentDirectory());


            var result = await _service.EditBookUsingBookFormModelAsync(id);


            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);

            Assert.AreEqual(book.Title, result.Data.Title);
            Assert.AreEqual(book.Author.Name, result.Data.SelectedAuthor);
        }


        [Test]
        public async Task EditBook_ShouldReturnError_WhenIdIsEmpty()
        {
            var id = Guid.Empty;

            var result = await _service.EditBookUsingBookFormModelAsync(id);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Not found!", result.ErrorMessage);

            Assert.IsNull(result.Data);

        }


        [Test]
        public async Task EditBook_ShouldReturnError_WhenIdIsNotValid()
        {
            var id = Guid.NewGuid();


            var result = await _service.EditBookUsingBookFormModelAsync(id);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Book not found !", result.ErrorMessage);

            Assert.IsNull(result.Data);

        }


        [Test]
        public async Task ConfirmEdit_ShouldReturnError_WhenIdIsEmpty()
        {
            var result = await _service.ConfirmEditChangesUsingBookFormModelAsync(Guid.Empty, new BookFormModel());

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Invalid book id.", result.ErrorMessage);
        }

        [Test]
        public async Task ConfirmEdit_ShouldReturnError_WhenAuthorMissing()
        {
            var id = Guid.NewGuid();

            var model = new BookFormModel
            {
                NewAuthor = null,
                SelectedAuthor = null
            };

            var result = await _service.ConfirmEditChangesUsingBookFormModelAsync(id, model);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Тhe book must have an author!", result.ErrorMessage);
        }


        [Test]
        public async Task ConfirmEdit_ShouldReturnError_WhenBookNotFound()
        {
            var id = Guid.NewGuid();

            var model = new BookFormModel
            {
                SelectedAuthor = "Pesho"
            };

            _booksRepoMock.Setup(x => x.GetByIdAsync(id))
              .ReturnsAsync((Book)null);

            var result = await _service.ConfirmEditChangesUsingBookFormModelAsync(id, model);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("The book you are trying to edit is missing.", result.ErrorMessage);
        }


        [Test]
        public async Task ConfirmEdit_ShouldReturnError_WhenUpdateFails()
        {
            var id = Guid.NewGuid();

            var book = new Book { Id = id };

            var model = new BookFormModel
            {
                Title = "Test",
                SelectedAuthor = "Pesho"
            };

            _booksRepoMock.Setup(x => x.GetByIdAsync(id))
             .ReturnsAsync(book);

            _booksRepoMock.Setup(x => x.UpdateAsync(It.IsAny<Book>()))
              .ThrowsAsync(new Exception());

            var result = await _service.ConfirmEditChangesUsingBookFormModelAsync(id, model);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Unexpected error is occurred while edit book! Please try again later.", result.ErrorMessage);
        }


        [Test]
        public async Task ConfirmEdit_ShouldUpdateBookSuccessfully()
        {
            var id = Guid.NewGuid();

            var book = new Book
            {
                Id = id,
                Title = "Old",
                Author =new Author 
                { 
                    Name = "OldAuthor" 
                },
                CoverImageUrl = "old.jpg"
            };

            var model = new BookFormModel
            {
                Title = "New",
                SelectedAuthor = "Pesho",
                CoverImage = "new.jpg",
                Genre = Genre.History
            };

            _booksRepoMock.Setup(x => x.GetByIdAsync(id))
              .ReturnsAsync(book);

            var result = await _service.ConfirmEditChangesUsingBookFormModelAsync(id, model);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("New", book.Title);
            Assert.AreEqual("Pesho", book.Author.Name);
        }


        [Test]
        public async Task ConfirmEdit_ShouldKeepOldCover_WhenNewCoverIsNull()
        {
            var id = Guid.NewGuid();

            var book = new Book
            {
                Id = id,
                CoverImageUrl = "old.jpg"
            };

            var model = new BookFormModel
            {
                SelectedAuthor = "Pesho",
                CoverImage = null
            };

            _booksRepoMock.Setup(x => x.GetByIdAsync(id))
              .ReturnsAsync(book);

            var result = await _service.ConfirmEditChangesUsingBookFormModelAsync(id, model);

            Assert.AreEqual("old.jpg", book.CoverImageUrl);
        }

        [Test]
        public async Task DeleteBook_ShouldReturnError_WithEmptyId()
        {
            var id = Guid.Empty;

            var result = await _service.DeleteCurrentBookAsync(id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Not found !", result.ErrorMessage);


        }

        [Test]
        public async Task DeleteBook_ShouldReturnError_WithInCorrectId()
        {
            var id = Guid.NewGuid();

            var result = await _service.DeleteCurrentBookAsync(id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("The book you are trying to delete is missing !", result.ErrorMessage);


        }

        [Test]
        public async Task DeleteBook_ShouldReturnError_WhenBookIsTaken()
        {
            
            var id = Guid.NewGuid();

            var book = new Book { Id = id };

            _booksRepoMock.Setup(x => x.GetByIdAsync(id))
              .ReturnsAsync(book);

            _systemsRepoMock.Setup(x => x.IsTakenBookAsync(id))
               .ReturnsAsync(true);

            
            var result = await _service.DeleteCurrentBookAsync(id);

           
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Book cannot be deleted because it is currently taken.", result.ErrorMessage);

        }

        [Test]
        public async Task DeleteBook_ShouldDeleteSuccessfully_WhenBookIsNotTaken()
        {
            var id = Guid.NewGuid();

            var book = new Book { Id = id };

            _booksRepoMock.Setup(x => x.GetByIdAsync(id))
              .ReturnsAsync(book);

            _systemsRepoMock.Setup(x => x.IsTakenBookAsync(id))
               .ReturnsAsync(false);

            var result = await _service.DeleteCurrentBookAsync(id);

            Assert.IsTrue(result.Success);

            _booksRepoMock.Verify(x => x.DeleteAsync(book), Times.Once);
        
        
        }

        [Test]
        public async Task DeleteBook_ShouldReturnError_WhenDeleteFails()
        {
            var id = Guid.NewGuid();

            var book = new Book { Id = id };

            _booksRepoMock.Setup(x => x.GetByIdAsync(id))
              .ReturnsAsync(book);

            _systemsRepoMock.Setup(x => x.IsTakenBookAsync(id))
              .ReturnsAsync(false);

            _booksRepoMock.Setup(x => x.DeleteAsync(book))
              .ThrowsAsync(new Exception());

            var result = await _service.DeleteCurrentBookAsync(id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Unexpected error occurred! Please try again later.", result.ErrorMessage);
        }



    }
}