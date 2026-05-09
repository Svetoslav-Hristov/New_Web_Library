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
                , _systemsRepoMock.Object, _loggerRepoMock.Object, _authorRepoMock.Object);


        }


        [Test]
        public async Task GetAllBooks_ShouldReturnAll_WhenNoFilters()
        {

            var authorA = new Author
            {
                Id = Guid.NewGuid(),
                Name = "A"

            };

            var authorB = new Author
            {
                Id = Guid.NewGuid(),
                Name = "B"

            };


            var books = new List<Book>
            {
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "B",
                    AuthorId=authorB.Id,
                    Author = authorB

                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "A",
                    AuthorId=authorA.Id,
                    Author = authorA

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
            Author authorA = new Author
            {
                Id = Guid.NewGuid(),
                Name = "Ivan"

            };

            Author authorB = new Author
            {
                Id = Guid.NewGuid(),
                Name = "Petar"

            };



            var books = new List<Book>
        {
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "CSharp",
                AuthorId=authorA.Id,
                Author =authorA

            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Java",
                AuthorId=authorB.Id,
                Author = authorB

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
            Author authorA = new Author
            {
                Id = Guid.NewGuid(),
                Name = "Ivan"

            };

            Author authorB = new Author
            {
                Id = Guid.NewGuid(),
                Name = "Petar"

            };


            var books = new List<Book>
        {
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "C#",
                AuthorId=authorA.Id,
                Author=authorA,
                Genre = Genre.Mystery
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "History",
                AuthorId=authorB.Id,
                Author=authorB,
                Genre = Genre.History
            }
        }
            .AsQueryable();

            _booksRepoMock.Setup(x => x.GetAllBooks()).Returns(books);

            var result = await _service.GetAllBooksOrderedByTitleThanByAuthorAscAsync(null, Genre.Mystery, 1, 10);

            Assert.AreEqual(1, result.Books.Count());
        }



        [Test]
        public async Task GetAllBooks_ShouldReturnPagedResult()
        {

            Author authorA = new Author
            {
                Id = Guid.NewGuid(),
                Name = "Jon"

            };

            Author authorB = new Author
            {
                Id = Guid.NewGuid(),
                Name = "Peter"

            };
            Author authorC = new Author
            {
                Id = Guid.NewGuid(),
                Name = "Glen"

            };



            var books = new List<Book>
        {
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "A",
                AuthorId=authorA.Id,
                Author=authorA

            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "B",
                AuthorId=authorB.Id,
                Author=authorB
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "C",
                AuthorId=authorC.Id,
                Author=authorC
            }
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
            Guid authorId = Guid.NewGuid();

            var books = new List<Book>
        {
            new Book
            {
                Title = "CSharp",
                AuthorId=authorId,
                Author = new Author
                {
                    Id=authorId,
                    Name="Ivan"
                },
                Genre = Genre.Horror
            },
            new Book
            {
                Title = "CSharp",
                AuthorId=authorId,
                Author = new Author
                {
                    Id=authorId,
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

            Guid id = Guid.NewGuid();

            Author author = new Author
            {
                Id = Guid.NewGuid(),
                Name = "Jon"


            };


            Book book = new Book
            {
                Id = id,
                Title = "Test Book",
                Year = 2020,
                Description = "Test Description",
                AuthorId = author.Id,
                Author = author,
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
            Guid id = Guid.NewGuid();

            Author author = new Author
            {
                Id = Guid.NewGuid(),
                Name = "Jon"
            };


            var book = new Book
            {
                Id = id,
                Title = "Test",
                AuthorId = author.Id,
                Author = author
            };

            _booksRepoMock.Setup(x => x.GetByIdAsync(id))
             .ReturnsAsync(book);

            _systemsRepoMock.Setup(x => x.ReturnStatusAsync(id))
              .ReturnsAsync((BookStatus?)null);

            var result = await _service.GetCurrentModelAsync(id);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(BookStatus.Returned, result.Data.BookStatus);



        }

        [Test]
        public async Task GetEmptyModelBookForm_ShouldLoadAuthorsGenresAndCovers()
        {
            _authorRepoMock.Setup(x => x.GetAllAuthorsAsync())
                .ReturnsAsync(new Dictionary<string, Guid>
                {
            { "Ivan", Guid.NewGuid() },
            { "Peter", Guid.NewGuid() }
                });

            var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempPath);

            var imagesPath = Path.Combine(tempPath, "images");
            Directory.CreateDirectory(imagesPath);

            File.WriteAllText(Path.Combine(imagesPath, "test1.jpg"), "");
            File.WriteAllText(Path.Combine(imagesPath, "test2.jpg"), "");

            _envMock.Setup(x => x.WebRootPath).Returns(tempPath);

            var result = await _service.GetEmptyModelBookFormWithLoadedTypesAsync();

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);

            Assert.IsNotNull(result.Data.Authors);
            Assert.IsTrue(result.Data.Authors.Any());

            Assert.IsNotNull(result.Data.Genres);
            Assert.IsTrue(result.Data.Genres.Any());

            Assert.IsNotNull(result.Data.Covers);
            Assert.IsTrue(result.Data.Covers.Any());
        }

        [Test]
        public async Task CreateBook_ShouldReturnError_WhenModelIsNull()
        {


            var result = await _service.CreateNewBookUsingBookFormModelAsync(null);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Invalid book data.", result.ErrorMessage);

        }

        [Test]
        public async Task CreateBook_ShouldReturnError_WhenMissingAuthor()
        {
            BookFormModel model = new BookFormModel()
            {
                Title = "Test",
                SelectedAuthor = Guid.NewGuid(),


            };

            _authorRepoMock.Setup(x => x.GetByIdAsync(model.SelectedAuthor)).ReturnsAsync((Author?)null);

            var result = await _service.CreateNewBookUsingBookFormModelAsync(model);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Тhe book must have an author!", result.ErrorMessage);

        }

        [Test]
        public async Task CreateBook_ShouldReturnError_WhenRepositoryThrows()
        {
            Author author = new Author
            {
                Id = Guid.NewGuid(),
                Name = "Test Author"
            };


            BookFormModel model = new BookFormModel()
            {
                Title = "Test",
                Year = 2020,
                Genre = Genre.History,
                SelectedAuthor = author.Id



            };

            _authorRepoMock.Setup(x => x.GetByIdAsync(author.Id)).ReturnsAsync(author);

            _booksRepoMock.Setup(x => x.AddAsync(It.IsAny<Book>()))
            .ThrowsAsync(new Exception());


            var result = await _service.CreateNewBookUsingBookFormModelAsync(model);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Unexpected error is occurred while create new book! Please try again later.", result.ErrorMessage);

        }

        [Test]
        public async Task CreateBook_ShouldCreateBook_WhenValidAuthorSelected()
        {


            var author = new Author
            {
                Id = Guid.NewGuid(),
                Name = "Test Author"
            };


            var model = new BookFormModel
            {
                Title = "Test",
                Year = 2020,
                SelectedAuthor = author.Id,
                Genre = Genre.History
            };


            _authorRepoMock
                .Setup(x => x.GetByIdAsync(author.Id))
                .ReturnsAsync(author);

            var result = await _service.CreateNewBookUsingBookFormModelAsync(model);

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(author.Id, result.Data.AuthorId);

            _booksRepoMock.Verify(x => x.AddAsync(It.IsAny<Book>()), Times.Once);
        }




        [Test]
        public async Task EditBook_ShouldReturnModel_WhenBookExists()
        {
            var bookId = Guid.NewGuid();

            Author author = new Author
            {
                Id = Guid.NewGuid(),
                Name = "Peter"
            };

            var book = new Book
            {
                Id = bookId,
                Title = "Test",
                AuthorId = author.Id,
                Author = author,
                Year = 2020,
                Genre = Genre.History,
                Description = "Desc"
            };

            _booksRepoMock
                .Setup(x => x.GetByIdAsync(bookId))
                .ReturnsAsync(book);

            _authorRepoMock.Setup(x => x.GetAllAuthorsAsync())
                .ReturnsAsync(new Dictionary<string, Guid>
                {
                    {
                        "Peter",
                        author.Id
                    },
                    {
                        "Ivan",
                        Guid.NewGuid()
                    }
                });

            var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempPath);
            Directory.CreateDirectory(Path.Combine(tempPath, "images"));

            _envMock.Setup(x => x.WebRootPath).Returns(tempPath);

            var result = await _service.EditBookUsingBookFormModelAsync(bookId);

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);

            Assert.AreEqual(book.Title, result.Data.Title);
            Assert.AreEqual(book.AuthorId, result.Data.SelectedAuthor);
            Assert.AreEqual(book.Year, result.Data.Year);
            Assert.AreEqual(book.Genre, result.Data.Genre);
            Assert.AreEqual(book.Description, result.Data.Description);

            Assert.IsNotNull(result.Data.Authors);
            Assert.IsTrue(result.Data.Authors.Any());
        }

        [Test]
        public async Task EditBook_ShouldReturnError_WhenIdIsEmpty()
        {
            var id = Guid.Empty;


            var result = await _service.EditBookUsingBookFormModelAsync(id);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Invalid book id!", result.ErrorMessage);

            Assert.IsNull(result.Data);

            _booksRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        }



        [Test]
        public async Task EditBook_ShouldReturnError_WhenIdIsNotValid()
        {
            var id = Guid.NewGuid();


            _booksRepoMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((Book?)null);

            var result = await _service.EditBookUsingBookFormModelAsync(id);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Book not found!", result.ErrorMessage);

            Assert.IsNull(result.Data);

            _booksRepoMock.Verify(x => x.GetByIdAsync(id),Times.Once);

        }


        [Test]
        public async Task ConfirmEdit_ShouldReturnError_WhenIdIsEmpty()
        {
            var result = await _service.ConfirmEditChangesUsingBookFormModelAsync(Guid.Empty, new BookFormModel());

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Invalid book id!", result.ErrorMessage);
        }


        [Test]
        public async Task ConfirmEdit_ShouldReturnError_WhenBookNotFound()
        {
            var bookId = Guid.NewGuid();

           

            BookFormModel model = new BookFormModel
            {
                Title = "Test",
                SelectedAuthor =Guid.NewGuid(),



            };


            _booksRepoMock.Setup(x => x.GetByIdAsync(bookId))
              .ReturnsAsync((Book?)null);

            var result = await _service.ConfirmEditChangesUsingBookFormModelAsync(bookId, model);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("The book you are trying to edit is missing.", result.ErrorMessage);

            _booksRepoMock.Verify(x => x.GetByIdAsync(bookId), Times.Once);

            _authorRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);

        }



        [Test]
        public async Task ConfirmEdit_ShouldReturnError_WhenAuthorIdIsEmpty()
        {
            var bookId = Guid.NewGuid();

            Book book = new Book
            {
                Id = bookId,
                Title = "Test",
                

            };


            BookFormModel model = new BookFormModel
            {

                SelectedAuthor = Guid.Empty
            };

            _booksRepoMock.Setup(x => x.GetByIdAsync(bookId)).ReturnsAsync(book);

            var result = await _service.ConfirmEditChangesUsingBookFormModelAsync(bookId, model);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Invalid author Id", result.ErrorMessage);

            _booksRepoMock.Verify(x => x.GetByIdAsync(bookId), Times.Once);

            _authorRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);

        }
        
        
        [Test]
        public async Task ConfirmEdit_ShouldReturnError_WhenAuthorIsMissing()
        {
            Guid bookId = Guid.NewGuid();
            Guid authorId = Guid.NewGuid();


            Book book = new Book
            {
                Id=bookId,
                Title = "Test",
                AuthorId = authorId
            };

            BookFormModel model = new BookFormModel
            {

                SelectedAuthor = authorId
            };

            _booksRepoMock.Setup(x => x.GetByIdAsync(bookId)).ReturnsAsync(book);

            _authorRepoMock.Setup(x => x.GetByIdAsync(authorId)).ReturnsAsync((Author?)null);

            var result = await _service.ConfirmEditChangesUsingBookFormModelAsync(bookId, model);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("The book must have an author!", result.ErrorMessage);

            _booksRepoMock.Verify(x => x.GetByIdAsync(bookId), Times.Once);
            _authorRepoMock.Verify(x => x.GetByIdAsync(authorId), Times.Once);

        }



        [Test]
        public async Task ConfirmEdit_ShouldReturnError_WhenNoChangesDetected()
        {
            Guid bookId = Guid.NewGuid();
            Guid authorId = Guid.NewGuid();

            Author author = new Author
            {
                Id = authorId,
                Name = "Peter"

            };


            Book book = new Book
            {
                Id = bookId,
                Title = "Title",
                AuthorId = authorId

            };

            BookFormModel model = new BookFormModel
            {
                Title = "Title",
                SelectedAuthor = authorId
            };


            _authorRepoMock.Setup(x => x.GetByIdAsync(authorId)).ReturnsAsync(author);

            _booksRepoMock.Setup(x => x.GetByIdAsync(bookId))
             .ReturnsAsync(book);

            

            var result = await _service.ConfirmEditChangesUsingBookFormModelAsync(bookId, model);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("No changes detected!", result.ErrorMessage);
            Assert.IsNull(result.Data);

            _booksRepoMock.Verify(x => x.GetByIdAsync(bookId), Times.Once);
            _authorRepoMock.Verify(x => x.GetByIdAsync(authorId), Times.Once);
            

        }




        [Test]
        public async Task ConfirmEdit_ShouldReturnError_WhenUpdateFails()
        {
            Guid bookId = Guid.NewGuid();
            Guid authorId = Guid.NewGuid();

            Author author = new Author
            {
                Id = authorId,
                Name = "Peter"

            };
            

            Book book = new Book 
            { 
                Id = bookId,
                Title="Test",
                AuthorId=authorId
                
            };

            BookFormModel model = new BookFormModel
            {
                Title = "New Title",
                SelectedAuthor =authorId
            };


            _authorRepoMock.Setup(x => x.GetByIdAsync(authorId)).ReturnsAsync(author);

            _booksRepoMock.Setup(x => x.GetByIdAsync(bookId))
             .ReturnsAsync(book);

            _booksRepoMock.Setup(x => x.UpdateAsync(It.IsAny<Book>()))
              .ThrowsAsync(new Exception());

            var result = await _service.ConfirmEditChangesUsingBookFormModelAsync(bookId, model);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Unexpected error is occurred while edit book! Please try again later.", result.ErrorMessage);
            Assert.IsNull(result.Data);

            _booksRepoMock.Verify(x => x.GetByIdAsync(bookId), Times.Once);
            _authorRepoMock.Verify(x => x.GetByIdAsync(authorId), Times.Once);
            _booksRepoMock.Verify(x => x.UpdateAsync(It.IsAny<Book>()), Times.Once);

        }


        [Test]
        public async Task ConfirmEdit_ShouldUpdateBookSuccessfully()
        {
            Guid bookId = Guid.NewGuid();
            Guid authorId = Guid.NewGuid();

            Author author = new Author
            {
                Id = authorId,
                Name = "Peter"


            };

            Book book = new Book
            {
                Id = bookId,
                Title = "Old",
                AuthorId=Guid.NewGuid(),
                Author = new Author
                {

                    Name = "OldAuthor"
                },
                CoverImageUrl = "old.jpg"
            };

            BookFormModel model = new BookFormModel
            {
                Title = "New",
                SelectedAuthor =authorId,
                CoverImage = "new.jpg",
                Genre = Genre.History
            };

            _authorRepoMock.Setup(x => x.GetByIdAsync(authorId)).ReturnsAsync(author);

            _booksRepoMock.Setup(x => x.GetByIdAsync(bookId))
              .ReturnsAsync(book);

            var result = await _service.ConfirmEditChangesUsingBookFormModelAsync(bookId, model);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("New", book.Title);
            Assert.AreEqual("Peter", book.Author.Name);

            _authorRepoMock.Verify(x => x.GetByIdAsync(authorId), Times.Once);
            _booksRepoMock.Verify(x => x.GetByIdAsync(bookId), Times.Once);

        }


        [Test]
        public async Task ConfirmEdit_ShouldKeepOldCover_WhenNewCoverIsNull()
        {
            Guid bookId = Guid.NewGuid();
            Guid authorId = Guid.NewGuid();

            Author author = new Author
            {
                Id = authorId,
                Name = "Peter"


            };
           
            Book book = new Book
            {
                Id = bookId,
                Title = "Old Title",
                AuthorId=authorId,
                CoverImageUrl = "old.jpg"
            };

            BookFormModel model = new BookFormModel
            {
                Title="New Title",
                SelectedAuthor=authorId,
                CoverImage = null
            };


            _authorRepoMock.Setup(x => x.GetByIdAsync(authorId)).ReturnsAsync(author);

            _booksRepoMock.Setup(x => x.GetByIdAsync(bookId))
              .ReturnsAsync(book);

            var result = await _service.ConfirmEditChangesUsingBookFormModelAsync(bookId, model);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("old.jpg", book.CoverImageUrl);

           
            _booksRepoMock.Verify(x => x.UpdateAsync(It.IsAny<Book>()),Times.Once);

        }

        [Test]
        public async Task DeleteBook_ShouldReturnError_WithEmptyId()
        {
            Guid bookId = Guid.Empty;

            var result = await _service.DeleteCurrentBookAsync(bookId);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Not found!", result.ErrorMessage);

           
        }

        [Test]
        public async Task DeleteBook_ShouldReturnError_WhenBookMissing()
        {
            Guid bookId = Guid.NewGuid();


            _booksRepoMock.Setup(x => x.GetByIdAsync(bookId)).ReturnsAsync((Book?)null);

            var result = await _service.DeleteCurrentBookAsync(bookId);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("The book you are trying to delete is missing!", result.ErrorMessage);

            _booksRepoMock.Verify(x => x.GetByIdAsync(bookId), Times.Once);

        }

        [Test]
        public async Task DeleteBook_ShouldReturnError_WhenBookIsTaken()
        {

            Guid bookId = Guid.NewGuid();
            Guid authorId = Guid.NewGuid();

            

            Book book = new Book 
            { 
                Id = bookId,
                Title="Test",
                AuthorId=authorId
                
            
            };

            
            _booksRepoMock.Setup(x => x.GetByIdAsync(bookId))
              .ReturnsAsync(book);

            _systemsRepoMock.Setup(x => x.IsTakenBookAsync(bookId))
               .ReturnsAsync(true);


            var result = await _service.DeleteCurrentBookAsync(bookId);


            Assert.IsFalse(result.Success);
            Assert.AreEqual("Book cannot be deleted because it is currently taken.", result.ErrorMessage);
            Assert.IsFalse(result.Data);

            _booksRepoMock.Verify(x => x.GetByIdAsync(bookId), Times.Once);

            _systemsRepoMock.Verify(x => x.IsTakenBookAsync(bookId), Times.Once);

            _booksRepoMock.Verify(x => x.DeleteAsync(It.IsAny<Book>()), Times.Never);


        }

        [Test]
        public async Task DeleteBook_ShouldDeleteSuccessfully_WhenBookIsNotTaken()
        {
            Guid bookId = Guid.NewGuid();
            Guid authorId = Guid.NewGuid();   

            Book book = new Book 
            { 
                Id = bookId,
                Title="Test",
                AuthorId=authorId

            };

            _booksRepoMock.Setup(x => x.GetByIdAsync(bookId))
              .ReturnsAsync(book);

            _systemsRepoMock.Setup(x => x.IsTakenBookAsync(bookId))
               .ReturnsAsync(false);

            var result = await _service.DeleteCurrentBookAsync(bookId);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Data);
            Assert.IsNull(result.ErrorMessage);

            _booksRepoMock.Verify(x => x.GetByIdAsync(bookId), Times.Once);

            _systemsRepoMock.Verify(x => x.IsTakenBookAsync(bookId), Times.Once);

            _booksRepoMock.Verify(x => x.DeleteAsync(It.Is<Book>(b=>b.Id==bookId)), Times.Once);


        }

        [Test]
        public async Task DeleteBook_ShouldReturnError_WhenDeleteFails()
        {
            Guid bookId = Guid.NewGuid();
            Guid authorId = Guid.NewGuid();

            Book book = new Book 
            { 
                Id = bookId,
                Title="Test",
                AuthorId=authorId
            };

            _booksRepoMock.Setup(x => x.GetByIdAsync(bookId))
              .ReturnsAsync(book);

            _systemsRepoMock.Setup(x => x.IsTakenBookAsync(bookId))
              .ReturnsAsync(false);

            _booksRepoMock.Setup(x => x.DeleteAsync(book))
              .ThrowsAsync(new Exception());

            var result = await _service.DeleteCurrentBookAsync(bookId);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Unexpected error occurred! Please try again later.", result.ErrorMessage);

            _booksRepoMock.Verify(x => x.GetByIdAsync(bookId), Times.Once);

            _systemsRepoMock.Verify(x => x.IsTakenBookAsync(bookId), Times.Once);

            _booksRepoMock.Verify(x => x.DeleteAsync(It.Is<Book>(b => b.Id == bookId)), Times.Once);


        }



    }
}
