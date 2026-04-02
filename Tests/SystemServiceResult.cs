using Microsoft.Extensions.Logging;
using Moq;
using New_Library.Data.Repository.Contracts;
using New_Library.Services.Core;
using New_Web_Library.Services.Core.Interfaces;

namespace AspNetCoreArchTemplate.Services.Core.Tests
{
    [TestFixture]
    public class SystemServiceResult
    {
        private  Mock<ISystemRepository> _systemsRepoMock;
        private  Mock<IUserRepository> _usersRepoMock;
        private  Mock<IBookRepository> _booksRepoMock;
        private  Mock<ICategoryRepository> _categoriesRepoMock;
        private  Mock<ITopicRepository> _topicsRepoMock;
        private  Mock<IPostRepository> _postsRepoMock;
        private  Mock<ICommentRepository> _commentsRepoMock;
        private  Mock<ILogger<ISystemService>> _loggerMock;

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









    }
}
