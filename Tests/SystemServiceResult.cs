using Microsoft.Extensions.Logging;
using Moq;
using New_Library.Data.Models.Forum;
using New_Library.Data.Repository.Contracts;
using New_Library.Services.Core;
using New_Web_Library.Data.Models;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels.System;

namespace AspNetCoreArchTemplate.Services.Core.Tests
{
    [TestFixture]
    public class SystemServiceResult
    {
        private Mock<ISystemRepository> _systemsRepoMock;
        private Mock<IUserRepository> _usersRepoMock;
        private Mock<IBookRepository> _booksRepoMock;
        private Mock<ICategoryRepository> _categoriesRepoMock;
        private Mock<ITopicRepository> _topicsRepoMock;
        private Mock<IPostRepository> _postsRepoMock;
        private Mock<ICommentRepository> _commentsRepoMock;
        private Mock<ILogger<ISystemService>> _loggerMock;

        private SystemService _service;

        [SetUp]
        public void SetUp()
        {
            _systemsRepoMock = new Mock<ISystemRepository>();
            _usersRepoMock = new Mock<IUserRepository>();
            _booksRepoMock = new Mock<IBookRepository>();
            _categoriesRepoMock = new Mock<ICategoryRepository>();
            _topicsRepoMock = new Mock<ITopicRepository>();
            _postsRepoMock = new Mock<IPostRepository>();
            _commentsRepoMock = new Mock<ICommentRepository>();
            _loggerMock = new Mock<ILogger<ISystemService>>();

            _service = new SystemService(_systemsRepoMock.Object, _usersRepoMock.Object
                , _booksRepoMock.Object, _categoriesRepoMock.Object, _topicsRepoMock.Object
                , _postsRepoMock.Object, _commentsRepoMock.Object, _loggerMock.Object);



        }

        [Test]
        public async Task DeleteLoan_ShouldReturnError_WhenLoanNotFound()
        {

            _systemsRepoMock.Setup(x => x.GetByIdAsync<UserBook>(It.IsAny<int>()))
              .ReturnsAsync((UserBook)null);

            var result = await _service.DeleteLoanAsync(1);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Not found !", result.ErrorMessage);

        }

        [Test]
        public async Task DeleteLoan_ShouldReturnSuccess_WhenValid()
        {

            var loan = new UserBook
            {
                Id = 1,
                UserId = Guid.NewGuid()
            };

            _systemsRepoMock.Setup(x => x.GetByIdAsync<UserBook>(1))
              .ReturnsAsync(loan);

            _systemsRepoMock.Setup(x => x.GetByIdAsync<UserBook>(1))
              .ReturnsAsync(loan);

            _systemsRepoMock.Setup(x => x.UserExtraLoanAsync(loan.UserId, loan.Id))
              .ReturnsAsync(true);

            var result = await _service.DeleteLoanAsync(1);

            Assert.IsTrue(result.Success);

        }


        [Test]
        public async Task DeleteLoan_ShouldUnblockUser_WhenNoOtherLoans()
        {

            var userId = Guid.NewGuid();

            var loan = new UserBook
            {
                Id = 1,
                UserId = userId
            };


            var user = new User
            {
                Id = userId,
                IsBlocked = true
            };


            _systemsRepoMock.Setup(x => x.GetByIdAsync<UserBook>(1))
               .ReturnsAsync(loan);


            _usersRepoMock.Setup(x => x.FindByIdAsync(userId))
              .ReturnsAsync(user);


            _systemsRepoMock.Setup(x => x.UserExtraLoanAsync(userId, loan.Id))
              .ReturnsAsync(false);


            var result = await _service.DeleteLoanAsync(1);



            Assert.IsTrue(result.Success);
            Assert.IsFalse(user.IsBlocked);

        }

        [Test]
        public async Task DeleteLoan_ShouldKeepUserBlocked_WhenHasOtherLoans()
        {
            var userId = Guid.NewGuid();

            var loan = new UserBook
            {
                Id = 1,
                UserId = userId
            };

            var user = new User
            {
                Id = userId,
                IsBlocked = true
            };

            _systemsRepoMock.Setup(x => x.GetByIdAsync<UserBook>(1))
              .ReturnsAsync(loan);

            _usersRepoMock.Setup(x => x.FindByIdAsync(userId))
              .ReturnsAsync(user);

            _systemsRepoMock.Setup(x => x.UserExtraLoanAsync(userId, loan.Id))
              .ReturnsAsync(true);

            var result = await _service.DeleteLoanAsync(1);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(user.IsBlocked);




        }

        [Test]
        public async Task DeleteLoan_ShouldReturnError_WhenExceptionThrown()
        {
            var loan = new UserBook
            {
                Id = 1,
                UserId = Guid.NewGuid()
            };


            _systemsRepoMock.Setup(x => x.GetByIdAsync<UserBook>(1))
              .ReturnsAsync(loan);


            _usersRepoMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>()))
              .ReturnsAsync(new User());


            _systemsRepoMock.Setup(x => x.UserExtraLoanAsync(It.IsAny<Guid>(), It.IsAny<int>()))
              .ReturnsAsync(false);


            _systemsRepoMock.Setup(x => x.UpdateAsync(It.IsAny<UserBook>()))
              .ThrowsAsync(new Exception());



            var result = await _service.DeleteLoanAsync(1);



            Assert.IsFalse(result.Success);
            Assert.AreEqual("Unexpected error is occurred please try again!", result.ErrorMessage);



        }


        [Test]
        public async Task FindUser_ShouldReturnError_WhenCriteriaIsEmpty()
        {

            CreateReserveModel model = new CreateReserveModel()
            {
                SearchingCriteria = null

            };

            var result = await _service.FindUserByCriteriaAsync(model);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("User with this email or phone number was not found!", result.ErrorMessage);

            Assert.AreEqual(model, result.Data);
        }


        [Test]
        public async Task FindUser_ShouldReturnError_WhenUserNotFound()
        {

            CreateReserveModel model = new CreateReserveModel
            {
                SearchingCriteria = "test@test.com",


            };

            _usersRepoMock.Setup(x => x.SearchByPhoneOrEmail(It.IsAny<string>()))
              .ReturnsAsync((User)null);

            var result = await _service.FindUserByCriteriaAsync(model);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("User with this email or phone number was not found!", result.ErrorMessage);
        }


        [Test]
        public async Task FindUser_ShouldReturnSuccess_WhenUserFound()
        {

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Jon",
                LastName = "Petrov"
            };


            CreateReserveModel model = new CreateReserveModel()
            {
                SearchingCriteria = "jon@test.com"


            };

            _usersRepoMock.Setup(x => x.SearchByPhoneOrEmail(It.IsAny<string>()))
              .ReturnsAsync(user);


            var result = await _service.FindUserByCriteriaAsync(model);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(user.Id, result.Data.UserId);

            Assert.AreEqual("Jon Petrov", result.Data.UserName);
            Assert.AreEqual("jon@test.com", result.Data.SearchingCriteria);


        }


        [Test]
        public async Task FindUser_ShouldTrimAndLowerCriteria()
        {
            var user = new User { Id = Guid.NewGuid(), FirstName = "A", LastName = "B" };

            var model = new CreateReserveModel
            {
                SearchingCriteria = "  TEST@MAIL.COM  "
            };

            _usersRepoMock.Setup(x => x.SearchByPhoneOrEmail("test@mail.com"))
              .ReturnsAsync(user);

            var result = await _service.FindUserByCriteriaAsync(model);

            Assert.AreEqual("test@mail.com", result.Data.SearchingCriteria);
        }


        [Test]
        public async Task GetSpecialArea_ShouldReturnError_WhenNotFound()
        {

            _topicsRepoMock.Setup(x => x.GetSubCategoryByName(It.IsAny<string>()))
              .ReturnsAsync((Topic)null);

            var result = await _service.GetSpecialArea();

            Assert.IsFalse(result.Success);

            Assert.AreEqual("SubCategory is not created or found!", result.ErrorMessage);

        }


        [Test]
        public async Task GetSpecialArea_ShouldReturnModel_WhenExists()
        {
            var topic = new Topic
            {
                Id = 1,
                Title = "Special",
                Posts = new List<Post>
            {
            new Post
            {
                Id = 1,
                Title = "Post1",
                CreatedOn = DateTime.UtcNow,
                User = new User { FirstName = "Jon", LastName = "Petrov" },
                Comments = new List<Comment> { new Comment(), new Comment() }
            }

                }
            };


            _topicsRepoMock.Setup(x => x.GetSubCategoryByName(It.IsAny<string>()))
              .ReturnsAsync(topic);

            var result = await _service.GetSpecialArea();

            Assert.IsTrue(result.Success);

            Assert.IsNotNull(result.Data);

            Assert.AreEqual(topic.Title, result.Data.CategoryName);
            
            Assert.AreEqual(1, result.Data.Posts.Count);

        }


        [Test]
        public async Task RestoreReservation_ShouldNotChangeModel_WhenBookNotFound()
        {
            var model = new CreateReserveModel
            {
                BookId = Guid.NewGuid(),
                BookTitle = null
            };

            _booksRepoMock.Setup(x => x.GetByIdAsync(model.BookId))
              .ReturnsAsync((Book)null);

            await _service.RestoreReservationModelAsync(model);

            Assert.IsNull(model.BookTitle);
        }

        [Test]
        public async Task RestoreReservation_ShouldSetBookData_WhenBookExists()
        {
            var id = Guid.NewGuid();

            var model = new CreateReserveModel
            {
                BookId = id
            };

            var book = new Book
            {
                Id = id,
                Title = "Test Book"
            };

            _booksRepoMock.Setup(x => x.GetByIdAsync(id))
              .ReturnsAsync(book);

            
            await _service.RestoreReservationModelAsync(model);

            
            Assert.AreEqual("Test Book", model.BookTitle);
            Assert.AreEqual(id, model.BookId);
        
        
        }


    }
}