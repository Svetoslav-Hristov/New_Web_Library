using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using New_Web_Library.Areas.Admin.Controllers;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels.Book;

namespace AspNetCoreArchTemplate.Services.Core.Tests
{
    [TestFixture]
    public class BooksControllerTests
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
        public async Task Create_ShouldRedirect_WhenServiceFails()
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
        public async Task Create_ShouldReturnView_WhenSuccess()
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
        public async Task CreatePost_ShouldReturnView_WhenModelStateInvalid()
        {
            var model = new BookFormModel();

            _controller.ModelState.AddModelError("Title", "Required");

            var result = await _controller.Create(model);

            Assert.IsInstanceOf<ViewResult>(result);

            var view = result as ViewResult;

            Assert.AreEqual("Create", view.ViewName);
        }

        [Test]
        public async Task CreatePost_ShouldReturnView_WhenAuthorMissing()
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







        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }




    }
}
