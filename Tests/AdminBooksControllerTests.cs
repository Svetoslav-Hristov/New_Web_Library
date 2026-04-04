using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using New_Web_Library.Areas.Admin.Controllers;
using New_Web_Library.Data.Models;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels.Book;

namespace New_Web_Library.Services.Core.Tests
{
    [TestFixture]
    public class AdminBooksControllerTests
    {
        private Mock<IBookService> _bookServiceMock;
        private Mock<ILogger<BooksController>> _loggerMock;


        private BooksController _controller;

        [SetUp]
        public void SetUp()
        {
            _bookServiceMock = new Mock<IBookService>();
            _loggerMock = new Mock<ILogger<BooksController>>();

            _controller = new BooksController(_bookServiceMock.Object, _loggerMock.Object);

            _controller.TempData = new TempDataDictionary(new DefaultHttpContext(),
               Mock.Of<ITempDataProvider>());

        }


        [Test]
        public async Task CreateBook_ShouldRedirect_WhenServiceFails()
        {
            _bookServiceMock.Setup(x => x.GetEmptyModelBookFormWithLoadedTypesAsync())
              .ReturnsAsync(new ServiceResult<BookFormModel>
              {
                  Success = false
              });


            var result = await _controller.Create();

            Assert.IsInstanceOf<RedirectToActionResult>(result);

            var redirect = result as RedirectToActionResult;

            Assert.AreEqual("Index", redirect.ActionName);
            Assert.AreEqual("Books", redirect.ControllerName);
        }



        [Test]
        public async Task CreateBook_ShouldReturnView_WhenSuccess()
        {
            var model = new BookFormModel();

            _bookServiceMock.Setup(x => x.GetEmptyModelBookFormWithLoadedTypesAsync())
              .ReturnsAsync(new ServiceResult<BookFormModel>
              {
                  Success = true,
                  Data = model
              });

            var result = await _controller.Create();

            Assert.IsInstanceOf<ViewResult>(result);

            var view = result as ViewResult;

            Assert.AreEqual(model, view.Model);
        }


        [Test]
        public async Task ConfirmCreateBook_ShouldReturnView_WhenModelStateInvalid()
        {
            var model = new BookFormModel();

            _controller.ModelState.AddModelError("Title", "Required");

            var result = await _controller.Create(model);

            Assert.IsInstanceOf<ViewResult>(result);

            var view = result as ViewResult;

            Assert.AreEqual("Create", view.ViewName);
        }

        [Test]
        public async Task ConfirmCreateBook_ShouldReturnView_WhenAuthorMissing()
        {
            var model = new BookFormModel
            {
                NewAuthor = null,
                SelectedAuthor = null
            };

            var result = await _controller.Create(model);

            Assert.IsInstanceOf<ViewResult>(result);

            Assert.IsFalse(_controller.ModelState.IsValid);
        }


        [Test]
        public async Task ConfirmCreateBook_ShouldRedirect_WhenServiceFails()
        {
            var model = new BookFormModel
            {
                SelectedAuthor = "Test"
            };

            _bookServiceMock.Setup(x => x.CreateNewBookUsingBookFormModelAsync(model))
              .ReturnsAsync(new ServiceResult<Book>
              {
                  Success = false,
                  ErrorMessage = "Error"
              });

            var result = await _controller.Create(model);

            Assert.IsInstanceOf<RedirectToActionResult>(result);

            var redirect = result as RedirectToActionResult;

            Assert.AreEqual("Index", redirect.ActionName);
        }

        [Test]
        public async Task ConfirmCreateBook_ShouldRedirectToDetails_WhenSuccess()
        {
            var model = new BookFormModel
            {
                SelectedAuthor = "Test"
            };

            var book = new Book
            {
                Id = Guid.NewGuid()
            };

            _bookServiceMock.Setup(x => x.CreateNewBookUsingBookFormModelAsync(model))
              .ReturnsAsync(new ServiceResult<Book>
              {
                  Success = true,
                  Data = book
              });

            var result = await _controller.Create(model);

            Assert.IsInstanceOf<RedirectToActionResult>(result);

            var redirect = result as RedirectToActionResult;

            Assert.AreEqual("Details", redirect.ActionName);
        }


        [Test]
        public async Task EditBook_ShouldReturnNotFound_WhenIdIsEmpty()
        {

            var id = Guid.Empty;


            var result = await _controller.Edit(id);


            Assert.IsInstanceOf<NotFoundResult>(result);
        }


        [Test]
        public async Task EditBook_ShouldRedirectToDetails_WhenServiceFails()
        {

            var id = Guid.NewGuid();

            var serviceResult = new ServiceResult<BookFormModel>()
            {
                Success = false,
                ErrorMessage = "Something went wrong"
            };

            _bookServiceMock.Setup(s => s.EditBookUsingBookFormModelAsync(id))
              .ReturnsAsync(serviceResult);


            var result = await _controller.Edit(id);


            var redirectResult = result as RedirectToActionResult;

            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Details", redirectResult.ActionName);
            Assert.AreEqual("Books", redirectResult.ControllerName);


            Assert.AreEqual("Something went wrong", _controller.TempData["ErrorBook"]);


            Assert.AreEqual(id, redirectResult.RouteValues["Id"]);

        }


        [Test]
        public async Task EditBook_ShouldReturnViewWithModel_WhenSuccess()
        {

            var id = Guid.NewGuid();

            var model = new BookFormModel();

            var serviceResult = new ServiceResult<BookFormModel>()
            {
                Success = true,
                Data = model
            };

            _bookServiceMock.Setup(s => s.EditBookUsingBookFormModelAsync(id))
               .ReturnsAsync(serviceResult);


            var result = await _controller.Edit(id);


            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.AreEqual(model, viewResult.Model);
        }


        [Test]
        public async Task ConfirmEditBook_ShouldReturnView_WhenModelStateIsInvalid()
        {

            var id = Guid.NewGuid();
            var model = new BookFormModel();

            _controller.ModelState.AddModelError("Title", "Required");


            var result = await _controller.Edit(id, model);


            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Edit", viewResult.ViewName);
            Assert.AreEqual(model, viewResult.Model);

            _bookServiceMock.Verify(s => s.BookModelDataFillingAsync(model), Times.Once);
        }

        [Test]
        public async Task ConfirmEditBook_ShouldReturnView_WhenNoAuthorProvided()
        {

            var id = Guid.NewGuid();
            var model = new BookFormModel
            {
                NewAuthor = null,
                SelectedAuthor = null
            };


            var result = await _controller.Edit(id, model);


            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Edit", viewResult.ViewName);

            Assert.IsTrue(_controller.ModelState.ContainsKey("SelectedAuthor"));
            Assert.IsTrue(_controller.ModelState.ContainsKey("NewAuthor"));

            _bookServiceMock.Verify(s => s.BookModelDataFillingAsync(model), Times.Once);



        }


        [Test]
        public async Task ConfirmEditBook_ShouldRedirect_WhenServiceFails()
        {
         
            var id = Guid.NewGuid();
            var model = CreateValidModel();

            var serviceResult = new ServiceResult<Book>
            {
                Success = false,
                ErrorMessage = "Error occurred"
            };

            _bookServiceMock.Setup(s => s.ConfirmEditChangesUsingBookFormModelAsync(id, model))
                .ReturnsAsync(serviceResult);


            var result = await _controller.Edit(id, model);


            var redirectResult = result as RedirectToActionResult;

            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Details", redirectResult.ActionName);
            Assert.AreEqual("Books", redirectResult.ControllerName);

            Assert.AreEqual("Error occurred", _controller.TempData["ErrorBook"]);
        }


        [Test]
        public async Task ConfirmEditBook_ShouldRedirectWithSuccess_WhenSuccessful()
        {
            
            var id = Guid.NewGuid();
            var model = CreateValidModel();

            var returnedId = Guid.NewGuid();

            var book = new Book
            {
                Id = returnedId
            };

            var serviceResult = new ServiceResult<Book>
            {
                Success = true,
                Data =book
            };

            _bookServiceMock.Setup(s => s.ConfirmEditChangesUsingBookFormModelAsync(id, model))
              .ReturnsAsync(serviceResult);

           
            var result = await _controller.Edit(id, model);

           
            var redirectResult = result as RedirectToActionResult;

            Assert.IsNotNull(redirectResult);

            Assert.AreEqual("Details", redirectResult.ActionName);
            Assert.AreEqual("Books", redirectResult.ControllerName);

            Assert.AreEqual("You have successfully edited your book.",
                _controller.TempData["SuccessBook"]);

            Assert.AreEqual(returnedId, redirectResult.RouteValues["id"]);
        }


        [Test]
        public async Task ConfirmEditBook_ShouldNotCallConfirmService_WhenModelStateInvalid()
        {
           
            var id = Guid.NewGuid();
            var model = new BookFormModel();

            _controller.ModelState.AddModelError("Title", "Required");

            
            await _controller.Edit(id, model);

            
            _bookServiceMock.Verify(s => s.ConfirmEditChangesUsingBookFormModelAsync(It.IsAny<Guid>(), It.IsAny<BookFormModel>()),
              Times.Never);
        }


        [Test]
        public async Task DeleteBook_ShouldReturnNotFound_WhenIdIsEmpty()
        {
           
            var id = Guid.Empty;

            
            var result = await _controller.Delete(id);

           
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task DeleteBook_ShouldNotCallService_WhenIdIsEmpty()
        {
           
            var id = Guid.Empty;

           
            await _controller.Delete(id);

            
            _bookServiceMock.Verify(s => s.DeleteCurrentBookAsync(It.IsAny<Guid>()),
              Times.Never);
        
        }



        [Test]
        public async Task DeleteBook_ShouldRedirectToDetails_WhenServiceFails()
        {
           
            var id = Guid.NewGuid();

            var serviceResult = new ServiceResult<bool>
            {
                Success = false,
                ErrorMessage = "Delete failed"
            };

            _bookServiceMock
                .Setup(s => s.DeleteCurrentBookAsync(id))
                .ReturnsAsync(serviceResult);

           
            var result = await _controller.Delete(id);

          
            var redirectResult = result as RedirectToActionResult;

            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Details", redirectResult.ActionName);

            Assert.AreEqual("Delete failed", _controller.TempData["ErrorBook"]);
            Assert.AreEqual(id, redirectResult.RouteValues["Id"]);
        }


        [Test]
        public async Task Delete_ShouldRedirectToIndex_WhenSuccessful()
        {
            
            var id = Guid.NewGuid();

            var serviceResult = new ServiceResult<bool>
            {
                Success = true
            };

            _bookServiceMock.Setup(s => s.DeleteCurrentBookAsync(id))
               .ReturnsAsync(serviceResult);

          
            var result = await _controller.Delete(id);

           
            var redirectResult = result as RedirectToActionResult;

           
            
            Assert.IsNotNull(redirectResult);

            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("Books", redirectResult.ControllerName);

            Assert.AreEqual("You have successfully deleted the book",
                _controller.TempData["SuccessBook"]);
        
        
        
        }








        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }
        private BookFormModel CreateValidModel()
        {
            return new BookFormModel
            {
                SelectedAuthor = "Author"
            };
        }



    }
}
