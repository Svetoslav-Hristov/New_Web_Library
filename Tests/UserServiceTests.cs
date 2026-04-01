using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using New_Library.Data.Repository;
using New_Library.Data.Repository.Contracts;
using New_Library.Services.Core;
using New_Web_Library.Data;
using New_Web_Library.Data.Models;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels.User;

namespace AspNetCoreArchTemplate.Services.Core.Tests
{
    [TestFixture]
    public class UserServiceTests
    {

        private LibraryDbContext _context;
        private UserRepository _repo;
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<SignInManager<User>> _signInManagerMock;
        private Mock<ISystemRepository> _systemRepositoryMock;
        private Mock<ILogger<IUserService>> _loggerMock;
        private UserService _service;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            _context = new LibraryDbContext(options);

            _repo = new UserRepository(_context);

            _userManagerMock = new Mock<UserManager<User>>(
              Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

            _signInManagerMock = new Mock<SignInManager<User>>(
                _userManagerMock.Object, Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<User>>(), null, null, null, null);

            _systemRepositoryMock = new Mock<ISystemRepository>();

            _loggerMock = new Mock<ILogger<IUserService>>();

            _service = new UserService(_userManagerMock.Object,
            _signInManagerMock.Object, _repo, _systemRepositoryMock.Object,
            _loggerMock.Object);
        }



        [TearDown]
        public void TearDown()
        {
            _context?.Dispose();
            _context = null;
        }



        [Test]
        public async Task GetAllUsers_ShouldReturnEmpty_WhenNoUsers()
        {

            var result = await _service.GetAllUsersWithOrWithoutSearchCriteriaAsync(null, 1, 5);

            Assert.IsFalse(result.Success);

            Assert.AreEqual(0, result.Data.Users.Count());

        }


        [Test]

        public async Task GetAllUsers_ShouldReturnUsers_WhenUsersExist()
        {


            _context.AddRange(new List<User>
            {
                new User { Id = Guid.NewGuid(), FirstName = "A", LastName = "A", Age = 20,Address="Test Address", Email = "a@a.com", PhoneNumber = "1"},
                new User {Id = Guid.NewGuid(), FirstName = "B", LastName = "B", Age = 21,Address="Test Address", Email = "b@b.com", PhoneNumber = "2" },
                new User { Id = Guid.NewGuid(), FirstName = "C", LastName = "C", Age = 22,Address="Test Address", Email = "c@c.com", PhoneNumber = "3" }
            });

            await _context.SaveChangesAsync();

            var result = await _service.GetAllUsersWithOrWithoutSearchCriteriaAsync(null, 1, 4);


            Assert.AreEqual(3, result.Data.Users.Count());

            Assert.IsTrue(result.Success);



        }

        [Test]

        public async Task GetAllUsers_ShouldReturnPagedUsers()
        {

            _context.AddRange(new List<User>
            {
                new User { Id = Guid.NewGuid(), FirstName = "A", LastName = "A", Age = 20,Address="Test Address", Email = "a@a.com", PhoneNumber = "1"},
                new User {Id = Guid.NewGuid(), FirstName = "B", LastName = "B", Age = 21,Address="Test Address", Email = "b@b.com", PhoneNumber = "2" },
                new User { Id = Guid.NewGuid(), FirstName = "C", LastName = "C", Age = 22,Address="Test Address", Email = "c@c.com", PhoneNumber = "3" }
            });


            await _context.SaveChangesAsync();

            var result = await _service.GetAllUsersWithOrWithoutSearchCriteriaAsync(null, 1, 2);

            Assert.IsTrue(result.Success);

            Assert.AreEqual(2, result.Data.Users.Count());

            Assert.AreEqual(2, result.Data.TotalPages);



        }

        [Test]

        public async Task GetAllUsers_ShouldFilterBySearch()
        {

            _context.Users.AddRange(new List<User>
            {
                new User { Id = Guid.NewGuid(), FirstName = "Ivan", LastName = "Petrov", Age = 20,Address="Test Address", Email="a@a.com", PhoneNumber="1" },
                new User { Id = Guid.NewGuid(), FirstName = "Georgi", LastName = "Ivanov", Age = 30,Address="Test Address", Email="b@b.com", PhoneNumber="2" }
            });


            await _context.SaveChangesAsync();


            var result = await _service.GetAllUsersWithOrWithoutSearchCriteriaAsync("Ivan", 1, 5);

            Assert.IsTrue(result.Success);

            Assert.AreEqual(2, result.Data.Users.Count());


        }
        [Test]

        public async Task GetAllUsers_ShouldReturnEmpty_WhenSearchDoesNotMatch()
        {


            _context.Users.AddRange(new List<User>
            {
                new User { Id = Guid.NewGuid(), FirstName = "Ivan", LastName = "Petrov", Age = 20,Address="Test Address", Email="a@a.com", PhoneNumber="1" },
                new User { Id = Guid.NewGuid(), FirstName = "Georgi", LastName = "Ivanov", Age = 30,Address="Test Address", Email="b@b.com", PhoneNumber="2" }
            });


            await _context.SaveChangesAsync();


            var result = await _service.GetAllUsersWithOrWithoutSearchCriteriaAsync("Jon", 1, 5);

            Assert.IsFalse(result.Success);

            Assert.AreEqual(0, result.Data.Users.Count());


        }

        [Test]

        public async Task ChangeUserStatus_ShouldReturnError_WhenUserNotBlocked()
        {

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User",
                Email = "test@test.com",
                PhoneNumber = "123",
                Address = "Test",
                Age = 20,
                IsBlocked = false
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _service.ChangeUserStatusAsync(user.Id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Тhe user is not blocked !", result.ErrorMessage);

        }

        [Test]
        public async Task ChangeUserStatus_ShouldReturnError_WhenUserNotFound()
        {

            var result = await _service.ChangeUserStatusAsync(Guid.NewGuid());

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Not found!", result.ErrorMessage);
        }

        [Test]
        public async Task ChangeUserStatus_ShouldUnblockUser_WhenBlocked()
        {

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User",
                Email = "test@test.com",
                PhoneNumber = "123",
                Address = "Test",
                Age = 20,
                IsBlocked = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _service.ChangeUserStatusAsync(user.Id);

            Assert.IsTrue(result.Success);

            var updatedUser = await _context.Users.FindAsync(user.Id);
            Assert.IsFalse(updatedUser.IsBlocked);
        }


        [Test]
        public async Task GetAllUserDetail_GetCorectly_WhenUserExist()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User",
                Email = "test@test.com",
                PhoneNumber = "123",
                Address = "Test",
                Age = 20,
                IsBlocked = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _service.GetAllUserDetailsAsync(user.Id);


            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);

            Assert.AreEqual(user.FirstName, result.Data.FirstName);
            Assert.AreEqual(user.LastName, result.Data.LastName);
            Assert.AreEqual(user.Email, result.Data.Email);
            Assert.AreEqual(user.PhoneNumber, result.Data.PhoneNumber);
            Assert.AreEqual(user.Address, result.Data.Address);
            Assert.AreEqual(user.Age, result.Data.Age);
            Assert.AreEqual(user.IsBlocked, result.Data.IsBlocked);


        }


        [Test]

        public async Task GetAllUserDetails_ShouldReturnError_WhenUserDoesNotExist()
        {


            var result = await _service.GetAllUserDetailsAsync(Guid.NewGuid());


            Assert.IsFalse(result.Success);
            Assert.IsNull(result.Data);

            Assert.AreEqual("Тhe user does not exist", result.ErrorMessage);


        }


        [Test]

        public async Task GetAllUserDetails_ShouldReturnOrderedHistory()
        {
            var book1 = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Old Book",
                Author = "Test Author"
            };

            var book2 = new Book
            {
                Id = Guid.NewGuid(),
                Title = "New Book",
                Author = "Test Author"
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User",
                Email = "test@test.com",
                PhoneNumber = "123",
                Address = "Test",
                Age = 20,
                IsBlocked = false
            };

            var userBooks = new List<UserBook>
            {
                new UserBook
                {
                    UserId = user.Id,
                    User = user,
                    BookId = book1.Id,
                    Book = book1,
                    PickUpDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1))
                },
                new UserBook
                {
                    UserId = user.Id,
                    User = user,
                    BookId = book2.Id,
                    Book = book2,
                    PickUpDate = DateOnly.FromDateTime(DateTime.Now)
                }
            };

            user.UserBooks = userBooks;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _service.GetAllUserDetailsAsync(user.Id);

            Assert.IsTrue(result.Success);

            Assert.AreEqual(2, result.Data.UserHistory.Count());

            Assert.AreEqual("New Book", result.Data.UserHistory.First().Title);


        }



        [Test]
        public async Task GetAllUserDetails_ShouldReturnEmptyHistory_WhenNoBooks()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User",
                Email = "test@test.com",
                PhoneNumber = "123",
                Address = "Test",
                Age = 20,
                IsBlocked = false,
                UserBooks = new List<UserBook>()
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _service.GetAllUserDetailsAsync(user.Id);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(0, result.Data.UserHistory.Count());

        }


        [Test]
        public async Task DeleteUserProfile_ShouldBeExecute_WhenUserExist()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User",
                Email = "test@test.com",
                PhoneNumber = "123",
                Address = "Test",
                Age = 20,
                IsBlocked = false
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();


            Assert.IsNotNull(user);

            var result = await _service.DeleteUserProfileAsync(user.Id);

            Assert.IsTrue(result.Success);

            Assert.IsTrue(user.IsDeleted);



        }

        [Test]
        public async Task DeleteUserProfile_NotBeExecute_WhenUserDoesNotExist()
        {

            var result = await _service.DeleteUserProfileAsync(Guid.NewGuid());

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Тhe user does not exist", result.ErrorMessage);



        }


        [Test]
        public async Task DeleteUserProfile_NotBeExecute_WhenUserHasNotReturnedBook()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User",
                Email = "test@test.com",
                PhoneNumber = "123",
                Address = "Test",
                Age = 20,
                IsBlocked = false
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _systemRepositoryMock.Setup(x => x.UserExtraLoanAsync(user.Id)).ReturnsAsync(true);
            
            Assert.IsNotNull(user);

            var result = await _service.DeleteUserProfileAsync(user.Id);


            Assert.IsFalse(result.Success);
            Assert.IsFalse(user.IsDeleted);
            Assert.AreEqual("The user cannot be deleted due to unspecified obligations !!", result.ErrorMessage);


        }



        



    }
}
