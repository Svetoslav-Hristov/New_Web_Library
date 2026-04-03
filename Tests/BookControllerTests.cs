using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using New_Web_Library.Controllers;
using New_Web_Library.GCommon.Enums;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels.Book;

namespace AspNetCoreArchTemplate.Services.Core.Tests
{
    [TestFixture]
    public class BookControllerTests
    {

        private   Mock<IBookService> _bookServiceMock;
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
        public async Task BookIndex_ShouldReturnView_WithBooks()
        {
            
            var expected = new BookPagingPreview();

            _bookServiceMock.Setup(s => s.GetAllBooksOrderedByTitleThanByAuthorAscAsync(
                It.IsAny<string?>(),It.IsAny<Genre?>(),
                It.IsAny<int>(),It.IsAny<int>()))
                .Returns(Task.FromResult(expected));

           
            var result = await _controller.Index("test", null, 1);

           
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.AreEqual(expected, viewResult.Model);
        }



        [Test]
        public async Task BookIndex_ShouldUseDefaultPage_WhenNotProvided()
        {
            
            string search = null;
            Genre? genre = null;

            var expected = new BookPagingPreview();

            _bookServiceMock.Setup(s => s.GetAllBooksOrderedByTitleThanByAuthorAscAsync(search, genre, 1, 4))
                .Returns(Task.FromResult(expected));

            
            var result = await _controller.Index(search, genre);

           
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.AreEqual(expected, viewResult.Model);
        
        
        }


        [Test]
        public async Task BookIndex_ShouldCallService_WithCorrectParameters()
        {
            
            string search = "test";
            Genre? genre = Genre.Fantasy;
            int page = 2;

            var expected = new BookPagingPreview();

            _bookServiceMock.Setup(s => s.GetAllBooksOrderedByTitleThanByAuthorAscAsync(search, genre, page, 4))
              .Returns(Task.FromResult(expected));

            
            await _controller.Index(search, genre, page);

            
            _bookServiceMock.Verify(s => s.GetAllBooksOrderedByTitleThanByAuthorAscAsync(search, genre, page, 4),
                Times.Once);
        
        
        
        }


        [Test]
        public async Task BookDetails_ShouldReturnNotFound_WhenIdIsEmpty()
        {
            
            var id = Guid.Empty;

           
            var result = await _controller.Details(id);

            
            Assert.IsInstanceOf<NotFoundResult>(result);
        
        
        
        
        }



        [Test]
        public async Task BookDetails_ShouldRedirectToIndex_WhenServiceFails()
        {
           
            var id = Guid.NewGuid();

          ServiceResult<FullPreviewModelBook> serviceResult = new ServiceResult<FullPreviewModelBook>
            {
                Success = false,
                ErrorMessage = "Not found"
            };

            _bookServiceMock.Setup(s => s.GetCurrentModelAsync(id))
                .Returns(Task.FromResult(serviceResult));

          
            var result = await _controller.Details(id);

           
            var redirectResult = result as RedirectToActionResult;

            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);

            Assert.AreEqual("Not found", _controller.TempData["ErrorBook"]);
        }





        [Test]
        public async Task Details_ShouldReturnView_WithModel_WhenSuccess()
        {
           
            var id = Guid.NewGuid();

            var model = new FullPreviewModelBook();

            ServiceResult<FullPreviewModelBook> serviceResult = new ServiceResult<FullPreviewModelBook>
            {
                Success = true,
                Data = model
            };

            _bookServiceMock.Setup(s => s.GetCurrentModelAsync(id))
                .Returns(Task.FromResult(serviceResult));

            
            var result = await _controller.Details(id);

          
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.AreEqual(model, viewResult.Model);
        
        
        
        }




        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }


    }
}
