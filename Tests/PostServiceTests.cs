using Microsoft.Extensions.Logging;
using Moq;
using New_Library.Data.Models.Forum;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data.Models;
using New_Web_Library.Service.Core;
using New_Web_Library.Service.Core.Interfaces;
using New_Web_Library.ViewModels.Forum;

namespace New_Web_Library.Services.Core.Tests
{
    [TestFixture]
    public class PostServiceTests
    {
        private Mock<IPostRepository> _postsRepoMock;
        private Mock<ICommentRepository> _commentsRepoMock;
        private Mock<ICategoryRepository> _categoriesRepoMock;
        private Mock<IUserRepository> _usersRepoMock;
        private Mock<ITopicRepository> _topicsRepoMock;
        private Mock<ILogger<IPostService>> _loggerMock;

        private PostService _service;

        [SetUp]
        public void SetUp()
        {
            _postsRepoMock = new Mock<IPostRepository>();
            _commentsRepoMock = new Mock<ICommentRepository>();
            _categoriesRepoMock = new Mock<ICategoryRepository>();
            _usersRepoMock = new Mock<IUserRepository>();
            _topicsRepoMock = new Mock<ITopicRepository>();
            _loggerMock = new Mock<ILogger<IPostService>>();

            _service = new PostService(_postsRepoMock.Object, _commentsRepoMock.Object,
             _categoriesRepoMock.Object, _usersRepoMock.Object, _topicsRepoMock.Object, _loggerMock.Object);


        }


        [Test]
        public async Task PostDetailModelsPreview_ShouldReturnError_WhenPostIsNull()
        {
            int id = 1;

            _postsRepoMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync((Post)null);

            _topicsRepoMock.Setup(x => x.GetSubCategoryByName(It.IsAny<string>()))
                .ReturnsAsync((Topic)null);

            var result = await _service.PostDetailModelsPreview(id, null, 1, 5);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Not Found!", result.ErrorMessage);



        }



        [Test]
        public async Task PostDetailModelsPreview_ShouldReturnFail_WhenPostIsFromSpecialCategory()
        {
            int id = 1;

            var special = new Topic { Id = 5 };

            var post = new Post
            {
                Id = id,
                TopicId = 5,
                Comments = new List<Comment>(),
                User = new User(),
            };

            _postsRepoMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(post);

            _topicsRepoMock.Setup(x => x.GetSubCategoryByName(It.IsAny<string>())).ReturnsAsync(special);

            var result = await _service.PostDetailModelsPreview(id, null, 1, 5);

            Assert.IsFalse(result.Success);



        }





        [Test]
        public async Task PostDetailModelsPreview_ShouldReturnSuccess_WhenValid()
        {
            int id = 1;

            var userId = Guid.NewGuid();

            var post = new Post
            {
                Id = id,
                Title = "Test",
                Content = "Content",
                CreatedOn = DateTime.UtcNow,
                UserId = userId,
                TopicId = 2,
                User = new User { FirstName = "John", LastName = "Doe" },
                Comments = new List<Comment>()
            };

            _postsRepoMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(post);
            _topicsRepoMock.Setup(x => x.GetSubCategoryByName(It.IsAny<string>())).ReturnsAsync((Topic)null);

            _commentsRepoMock.Setup(x => x.GetAllCountCommentsAsync(It.IsAny<List<Guid>>()))
                .ReturnsAsync(new Dictionary<Guid, int>());

            _postsRepoMock.Setup(x => x.GetAllCountPostsAsync(It.IsAny<List<Guid>>()))
                .ReturnsAsync(new Dictionary<Guid, int>());

            var result = await _service.PostDetailModelsPreview(id, null, 1, 5);

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(id, result.Data.Post.Id);
        }



        [Test]
        public async Task PostDetailModelsPreview_ShouldMarkAuthor_WhenUserIsAuthor()
        {
            int id = 1;
            var userId = Guid.NewGuid();

            var post = new Post
            {
                Id = id,
                Title = "Test",
                Content = "Content",
                CreatedOn = DateTime.UtcNow,
                UserId = userId,
                TopicId = 2,
                User = new User { FirstName = "John", LastName = "Doe" },
                Comments = new List<Comment>()
            };

            _postsRepoMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(post);

            _topicsRepoMock.Setup(x => x.GetSubCategoryByName(It.IsAny<string>())).ReturnsAsync((Topic)null);

            _commentsRepoMock.Setup(x => x.GetAllCountCommentsAsync(It.IsAny<List<Guid>>()))
                .ReturnsAsync(new Dictionary<Guid, int>());

            _postsRepoMock.Setup(x => x.GetAllCountPostsAsync(It.IsAny<List<Guid>>()))
                .ReturnsAsync(new Dictionary<Guid, int>());

            _usersRepoMock.Setup(x => x.AdminOrNotAsync(userId))
                .ReturnsAsync(false);

            var result = await _service.PostDetailModelsPreview(id, userId, 1, 5);


            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Data.Post.IsAuthor);

        }



        [Test]
        public async Task CreateNewPost_ShouldReturnError_WhenSubCategoryNotExists()
        {
            int subCategoryId = 1;


            _topicsRepoMock.Setup(x => x.GetByIdAsync<Topic>(1)).ReturnsAsync((Topic)null);


            var result = await _service.CreateNewPost(subCategoryId);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("SubCategory not found!", result.ErrorMessage);
        }



        [Test]
        public async Task CreateNewPost_ShouldReturnSuccess_WhenSubCategoryExists()
        {
            int subCategoryId = 1;

            Topic subCategory = new Topic()
            {
                Id = subCategoryId,
                Title = "Test"


            };

            _topicsRepoMock.Setup(x => x.GetByIdAsync<Topic>(subCategoryId)).ReturnsAsync(subCategory);

            var result = await _service.CreateNewPost(subCategoryId);


            Assert.IsTrue(result.Success);

            Assert.IsNotNull(result.Data);

            Assert.AreEqual(subCategoryId, result.Data.SubCategoryId);


        }


        [Test]
        public async Task ConfirmNewPost_ShouldReturnError_WhenModelReturnEmptyTitle()
        {

            int categoryId = 1;
            Guid userId = Guid.NewGuid();

            CreateContentViewModel model = new CreateContentViewModel()
            {
                Title = " ",
                Description = "Some description"


            };

            var result = await _service.ConfirmNewPost(model, userId, categoryId);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Title is required.", result.ErrorMessage);

            Assert.IsNull(result.Data);


        }

        [Test]
        public async Task ConfirmNewPost_ShouldReturnError_WhenModelReturnEmptyDescription()
        {

            int categoryId = 1;
            Guid userId = Guid.NewGuid();

            CreateContentViewModel model = new CreateContentViewModel()
            {
                Title = "Test",
                Description = " "


            };

            var result = await _service.ConfirmNewPost(model, userId, categoryId);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("The post must have content.", result.ErrorMessage);

            Assert.IsNull(result.Data);


        }


        [Test]
        public async Task ConfirmNewPost_ShouldReturnError_WhenUserIdIsEmpty()
        {

            int categoryId = 1;
            Guid userId = Guid.Empty;

            CreateContentViewModel model = new CreateContentViewModel()
            {
                Title = "Test",
                Description = "Some description "


            };

            var result = await _service.ConfirmNewPost(model, userId, categoryId);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Invalid user ID.", result.ErrorMessage);

            Assert.IsNull(result.Data);


        }



        [Test]
        public async Task ConfirmNewPost_ShouldReturnError_WhenUserIsNotExists()
        {

            int categoryId = 1;
            Guid userId = Guid.NewGuid();

            CreateContentViewModel model = new CreateContentViewModel()
            {
                Title = "Test",
                Description = "Some description "


            };

            _usersRepoMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync((User)null);

            var result = await _service.ConfirmNewPost(model, userId, categoryId);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("User not found!", result.ErrorMessage);

            Assert.IsNull(result.Data);


        }


        [Test]
        public async Task ConfirmNewPost_ShouldReturnError_WhenCategoryIsNotExists()
        {

            int categoryId = 1;

            Guid userId = Guid.NewGuid();

            CreateContentViewModel model = new CreateContentViewModel()
            {
                Title = "Test",
                Description = "Some description "


            };

            User user = new User()
            {
                Id = userId,
                FirstName = "Jon",
                LastName = "Snow",
                Address = "WinterFell"

            };

            _usersRepoMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

            _categoriesRepoMock.Setup(x => x.GetByIdAsync<Topic>(categoryId)).ReturnsAsync((Topic)null);

            var result = await _service.ConfirmNewPost(model, userId, categoryId);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("SubCategory not found!", result.ErrorMessage);

            Assert.IsNull(result.Data);


        }


        [Test]
        public async Task ConfirmNewPost_ShouldReturnSuccess_WhenValid()
        {

            var userId = Guid.NewGuid();
            int categoryId = 1;

            var model = new CreateContentViewModel
            {
                Title = "Test Title",
                Description = "Test Content"
            };

            User user = new User()
            {
                Id = userId,
                FirstName = "Jon",
                LastName = "Snow",
                Address = "WinterFell"



            };

            var subCategory = new Topic { Id = categoryId };

            _usersRepoMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            _topicsRepoMock.Setup(x => x.GetByIdAsync<Topic>(categoryId))
                .ReturnsAsync(subCategory);

            _postsRepoMock.Setup(x => x.AddAsync(It.IsAny<Post>()))
                .Returns(Task.CompletedTask);


            var result = await _service.ConfirmNewPost(model, userId, categoryId);


            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual("Test Title", result.Data.Title);
            Assert.AreEqual("Test Content", result.Data.Content);
        }



        [Test]
        public async Task ConfirmNewPost_ShouldReturnError_WhenExceptionThrown()
        {

            var userId = Guid.NewGuid();
            int categoryId = 1;

            var model = new CreateContentViewModel
            {
                Title = "Test Title",
                Description = "Test Content"
            };

            User user = new User()
            {
                Id = userId,
                FirstName = "Jon",
                LastName = "Snow",
                Address = "WinterFell"



            };

            var subCategory = new Topic { Id = categoryId };

            _usersRepoMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            _topicsRepoMock.Setup(x => x.GetByIdAsync<Topic>(categoryId))
                .ReturnsAsync(subCategory);

            _postsRepoMock.Setup(x => x.AddAsync(It.IsAny<Post>()))
                .ThrowsAsync(new Exception());


            var result = await _service.ConfirmNewPost(model, userId, categoryId);



            Assert.IsFalse(result.Success);


            Assert.AreEqual("Unexpected error is occurred while create new post! Please try again later.",
                result.ErrorMessage);


        }


        [Test]
        public async Task EditPost_ShouldReturnError_WhenPostNotExists()
        {
            int postId = 1;


            _postsRepoMock.Setup(x => x.GetByIdAsync(postId)).ReturnsAsync((Post)null);


            var result = await _service.EditPost(postId);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Post not found", result.ErrorMessage);

            Assert.IsNull(result.Data);

        }

        [Test]
        public async Task EditPost_ShouldReturnSuccess_WhenPostExists()
        {
            int postId = 1;

            Post post = new Post()
            {
                Id = postId,
                Title = "Test",
                Content = "Some content"

            };


            _postsRepoMock.Setup(x => x.GetByIdAsync<Post>(postId)).ReturnsAsync(post);


            var result = await _service.EditPost(postId);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(postId, result.Data.PostId);
            Assert.AreEqual("Test", result.Data.Title);
            Assert.AreEqual("Some content", result.Data.Description);


        }


        [Test]
        public async Task ConfirmEditPost_ShouldReturnError_WhenPostTitleIsEmpty()
        {
            int postId = 1;
            Guid userId = Guid.NewGuid();

            CreateContentViewModel model = new CreateContentViewModel()
            {

                Title = " ",
                Description = "Some description"


            };


            var result = await _service.ConfirmEditPost(model, userId, postId);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Title is required.", result.ErrorMessage);

            Assert.IsNull(result.Data);

        }

        [Test]
        public async Task ConfirmEditPost_ShouldReturnError_WhenPostContentIsEmpty()
        {
            int postId = 1;
            Guid userId = Guid.NewGuid();

            CreateContentViewModel model = new CreateContentViewModel()
            {

                Title = "Test",
                Description = " "


            };


            var result = await _service.ConfirmEditPost(model, userId, postId);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("The post must have content.", result.ErrorMessage);

            Assert.IsNull(result.Data);

        }

        [Test]
        public async Task ConfirmEditPost_ShouldReturnError_WhenUserIdIsEmpty()
        {
            int postId = 1;
            Guid userId = Guid.Empty;

            CreateContentViewModel model = new CreateContentViewModel()
            {

                Title = "Test",
                Description = "Some description "


            };


            var result = await _service.ConfirmEditPost(model, userId, postId);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Invalid user ID.", result.ErrorMessage);

            Assert.IsNull(result.Data);

        }

        [Test]
        public async Task ConfirmEditPost_ShouldReturnError_WhenPostNotExists()
        {
            int postId = 1;
            Guid userId = Guid.NewGuid();

            CreateContentViewModel model = new CreateContentViewModel()
            {

                Title = "Test",
                Description = "Some description "


            };

            User user = new User()
            {
                Id = userId,
                FirstName = "Jon",
                LastName = "Snow",


            };


            _usersRepoMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

            _postsRepoMock.Setup(x => x.GetByIdAsync(postId)).ReturnsAsync((Post)null);


            var result = await _service.ConfirmEditPost(model, userId, postId);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Post not found!", result.ErrorMessage);

            Assert.IsNull(result.Data);

        }

        [Test]
        public async Task ConfirmEditPost_ShouldReturnError_WhenUserHasNotPermission()
        {

            int postId = 1;
            Guid userId = Guid.NewGuid();

            CreateContentViewModel model = new CreateContentViewModel()
            {
                Title = "Test",
                Description = "Some description"
            };

            Post post = new Post()
            {
                Id = postId,
                Title = "Test",
                Content = "Some description",
                UserId = Guid.NewGuid()
            };

            User user = new User()
            {
                Id = userId,
                FirstName = "Jon",
                LastName = "Snow",
            };

            _usersRepoMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            _usersRepoMock.Setup(x => x.AdminOrNotAsync(userId))
                .ReturnsAsync(false);

            _postsRepoMock.Setup(x => x.GetByIdAsync<Post>(postId))
                .ReturnsAsync(post);


            var result = await _service.ConfirmEditPost(model, userId, postId);


            Assert.IsFalse(result.Success);
            Assert.AreEqual("You don't have permission over this post.", result.ErrorMessage);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task ConfirmEditPost_ShouldReturnSuccess_WhenValid()
        {

            int postId = 1;
            Guid userId = Guid.NewGuid();

            CreateContentViewModel model = new CreateContentViewModel()
            {
                Title = "Updated Title",
                Description = "Updated Content"
            };

            Post post = new Post()
            {
                Id = postId,
                Title = "Old Title",
                Content = "Old Content",
                UserId = userId
            };

            _postsRepoMock.Setup(x => x.GetByIdAsync<Post>(postId))
                .ReturnsAsync(post);

            _usersRepoMock.Setup(x => x.AdminOrNotAsync(userId))
                .ReturnsAsync(false);

            _postsRepoMock.Setup(x => x.UpdateAsync(post))
                .Returns(Task.CompletedTask);


            var result = await _service.ConfirmEditPost(model, userId, postId);


            Assert.IsTrue(result.Success);

            Assert.IsNotNull(result.Data);

            Assert.AreEqual("Updated Title", result.Data.Title);

            Assert.AreEqual("Updated Content", result.Data.Content);


        }


        [Test]
        public async Task ConfirmEditPost_ShouldReturnError_WhenExceptionThrown()
        {

            int postId = 1;
            Guid userId = Guid.NewGuid();

            var model = new CreateContentViewModel()
            {
                Title = "Updated Title",
                Description = "Updated Content"
            };

            var post = new Post()
            {
                Id = postId,
                Title = "Old Title",
                Content = "Old Content",
                UserId = userId
            };

            _postsRepoMock.Setup(x => x.GetByIdAsync<Post>(postId))
                .ReturnsAsync(post);

            _usersRepoMock.Setup(x => x.AdminOrNotAsync(userId))
                .ReturnsAsync(false);

            _postsRepoMock.Setup(x => x.UpdateAsync(post))
                .ThrowsAsync(new Exception());


            var result = await _service.ConfirmEditPost(model, userId, postId);


            Assert.IsFalse(result.Success);


            Assert.AreEqual("Unexpected error is occurred while edit  post! Please try again later.",
                result.ErrorMessage);



        }


        [Test]
        public async Task SofDeletePost_ShouldReturnError_WhenPostNotExists()
        {
            int postId = 1;
            Guid userId = Guid.NewGuid();



            _postsRepoMock.Setup(x => x.GetByIdAsync<Post>(postId)).ReturnsAsync((Post)null);

            var result = await _service.SoftDeletePost(1, userId);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Post not found!", result.ErrorMessage);

            Assert.IsNull(result.Data);

        }


        [Test]
        public async Task SofDeletePost_ShouldReturnError_WhenUserIdIsEmpty()
        {
            int subCategoryId = 1;
            int postId = 1;
            Guid userId = Guid.Empty;

            Topic topic = new Topic()
            {
                Id = subCategoryId,
                Title = "Test",
                Posts = new List<Post>()
                {
                    new Post{
                    Id = postId,
                    Title = "Test",
                    Content = "Some content",
                    TopicId=subCategoryId


                }
              }

            };




            _topicsRepoMock.Setup(x => x.GetByIdAsync<Topic>(subCategoryId)).ReturnsAsync(topic);

            _postsRepoMock.Setup(x => x.GetByIdAsync(postId)).ReturnsAsync(topic.Posts.First());

            var result = await _service.SoftDeletePost(1, userId);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Invalid user Id", result.ErrorMessage);

            Assert.IsNull(result.Data);

        }


        [Test]
        public async Task SoftDeletePost_ShouldReturnError_WhenUserHasNoPermission()
        {

            int postId = 1;
            Guid userId = Guid.NewGuid();

            var post = new Post()
            {
                Id = postId,
                TopicId = 10,
                UserId = Guid.NewGuid()
            };

            _postsRepoMock.Setup(x => x.GetByIdAsync(postId))
                .ReturnsAsync(post);

            _topicsRepoMock.Setup(x => x.GetByIdAsync<Topic>(post.TopicId))
                .ReturnsAsync(new Topic());

            _usersRepoMock.Setup(x => x.AdminOrNotAsync(userId))
                .ReturnsAsync(false);


            var result = await _service.SoftDeletePost(postId, userId);


            Assert.IsFalse(result.Success);
            Assert.AreEqual("You don't have permission over this post.", result.ErrorMessage);
        }



        [Test]
        public async Task SoftDeletePost_ShouldReturnError_WhenExceptionThrown()
        {

            int postId = 1;
            Guid userId = Guid.NewGuid();

            Post post = new Post()
            {
                Id = postId,
                TopicId = 10,
                UserId = userId
            };

            var topic = new Topic { Id = 10 };

            _postsRepoMock.Setup(x => x.GetByIdAsync(postId))
                .ReturnsAsync(post);

            _topicsRepoMock.Setup(x => x.GetByIdAsync<Topic>(post.TopicId))
                .ReturnsAsync(topic);

            _usersRepoMock.Setup(x => x.AdminOrNotAsync(userId))
                .ReturnsAsync(false);

            _postsRepoMock.Setup(x => x.UpdateAsync(post))
                .ThrowsAsync(new Exception());


            var result = await _service.SoftDeletePost(postId, userId);


            Assert.IsFalse(result.Success);


            Assert.AreEqual("Unexpected error is occurred while delete post! Please try again later.",
                result.ErrorMessage);


        }



        [Test]
        public async Task SoftDeletePost_ShouldReturnSuccess_WhenValid()
        {

            int postId = 1;
            Guid userId = Guid.NewGuid();

            var post = new Post()
            {
                Id = postId,
                TopicId = 10,
                UserId = userId
            };

            var topic = new Topic { Id = 10 };

            _postsRepoMock.Setup(x => x.GetByIdAsync(postId))
                .ReturnsAsync(post);

            _topicsRepoMock.Setup(x => x.GetByIdAsync<Topic>(post.TopicId))
                .ReturnsAsync(topic);

            _usersRepoMock.Setup(x => x.AdminOrNotAsync(userId))
                .ReturnsAsync(false);

            _postsRepoMock.Setup(x => x.UpdateAsync(post))
                .Returns(Task.CompletedTask);


            var result = await _service.SoftDeletePost(postId, userId);


            Assert.IsTrue(result.Success);
            Assert.IsTrue(post.IsDeleted);
            
            Assert.IsNotNull(post.DeleteAt);
            Assert.IsNotNull(result.Data);
        }


        [Test]
        public async Task RestoreDeletePos_ShouldReturnError_WhenPostNotExist()
        {
            int postId = 1;


            _postsRepoMock.Setup(x => x.GetDeleteOrNotPostAsync(postId)).ReturnsAsync((Post)null);

            var result = await _service.RestoreDeletePost(postId);

           
            
            Assert.IsFalse(result.Success);

            Assert.AreEqual("Post not found!", result.ErrorMessage);


        }

        [Test]
        public async Task RestoreDeletePos_ShouldReturnError_WhenParentCategoryNotExist()
        {
            int subCategoryId = 1;
            int postId = 1;


            Topic subCategory = new Topic()
            {
                Id = subCategoryId,
                Title = "Test",
                IsDeleted = true


            };




            Post post = new Post()
            {
                Id = postId,
                Title = "Test",
                Content = "Some content",
                IsDeleted = true,
                TopicId = 1

            };



            _topicsRepoMock.Setup(x => x.GetDeleteOrNotSubCategoryAsync(subCategoryId)).ReturnsAsync(subCategory);

            _postsRepoMock.Setup(x => x.GetDeleteOrNotPostAsync(postId)).ReturnsAsync(post);

            var result = await _service.RestoreDeletePost(postId);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("You can't restore the post because the subcategory is deleted.", result.ErrorMessage);


        }


        [Test]
        public async Task RestoreDeletePos_ShouldReturnSuccess_WhenParentCategoryExist()
        {

            int subCategoryId = 1;
            int postId = 1;


            Topic subCategory = new Topic()
            {
                Id = subCategoryId,
                Title = "Test",
                IsDeleted = false


            };


            Post post = new Post()
            {
                Id = postId,
                Title = "Test",
                Content = "Some content",
                IsDeleted = true,
                TopicId = 1

            };


            _topicsRepoMock.Setup(x => x.GetDeleteOrNotSubCategoryAsync(subCategoryId)).ReturnsAsync(subCategory);

            _postsRepoMock.Setup(x => x.GetDeleteOrNotPostAsync(postId)).ReturnsAsync(post);

            var result = await _service.RestoreDeletePost(postId);

            Assert.IsTrue(result.Success);

            Assert.IsFalse(post.IsDeleted);

            Assert.IsNull(post.DeleteAt);

        }



        [Test]
        public async Task RestoreDeletePost_ShouldReturnError_WhenExceptionThrown()
        {
            
            int postId = 1;

            var post = new Post()
            {
                Id = postId,
                TopicId = 10,
                IsDeleted = true
            };

            var topic = new Topic()
            {
                Id = 10,
                IsDeleted = false 
            };

            
            
            
            _postsRepoMock.Setup(x => x.GetDeleteOrNotPostAsync(postId))
                .ReturnsAsync(post);

            _topicsRepoMock.Setup(x => x.GetDeleteOrNotSubCategoryAsync(post.TopicId))
                .ReturnsAsync(topic);

            _postsRepoMock.Setup(x => x.UpdateAsync(post))
                .ThrowsAsync(new Exception());

            var result = await _service.RestoreDeletePost(postId);

          
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Unexpected error is occurred while restore delete Post! Please try again later.",
                result.ErrorMessage);
        
        
        
        }


        [Test]
        public async Task HardDeletePost_ShouldReturnError_WhenPostNotExists()
        {
            int postId = 1;


            _postsRepoMock.Setup(x => x.GetDeleteOrNotPostAsync(postId)).ReturnsAsync((Post)null);

            var result = await _service.HardDeletePost(postId);


            Assert.IsFalse(result.Success);

            Assert.AreEqual("Post not found!", result.ErrorMessage);

        }


        [Test]
        public async Task HardDeletePost_ShouldReturnSuccess_WhenPostExist()
        {
            int postId = 1;

            Post post = new Post()
            {
                Id = postId,
                Title = "Test",
                Content = "Some content",
                IsDeleted = true


            };

            _postsRepoMock.Setup(x => x.GetDeleteOrNotPostAsync(postId)).ReturnsAsync(post);

            var result = await _service.HardDeletePost(postId);

            Assert.IsTrue(result.Success);

            _postsRepoMock.Verify(x => x.DeleteAsync(post), Times.Once);


        }


        [Test]
        public async Task HardDeletePost_ShouldReturnError_WhenExceptionThrown()
        {
          
            int postId = 1;

            var post = new Post()
            {
                Id = postId
            };

            _postsRepoMock.Setup(x => x.GetDeleteOrNotPostAsync(postId))
                .ReturnsAsync(post);

            _postsRepoMock.Setup(x => x.DeleteAsync(post))
                .ThrowsAsync(new Exception());

           
            var result = await _service.HardDeletePost(postId);

           
            Assert.IsFalse(result.Success);
            
            
            Assert.AreEqual("Unexpected error is occurred while hard delete Post! Please try again later.",
                result.ErrorMessage);
        
        
        }


        [Test]
        public async Task GetUserComplaint_ShouldReturnError_WhenPostNotExist()
        {
            int postId = 1;



            _postsRepoMock.Setup(x => x.GetByIdAsync(postId)).ReturnsAsync((Post)null);

            var result = await _service.GetUserComplaint(postId);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Post not found!", result.ErrorMessage);

        }

        [Test]
        public async Task GetUserComplaint_ShouldReturnSuccess_WhenPostExists()
        {
          
            int postId = 1;
            Guid userId = Guid.NewGuid();

            Post post = new Post()
            {
                Id = postId,
                Title = "Test Title",
                Content = "Test Content",
                CreatedOn = DateTime.UtcNow,
                UserId = userId,
                TopicId = 5,
                User = new User
                {
                    FirstName = "Jon",
                    LastName = "Snow"
                }
            };

            _postsRepoMock.Setup(x => x.GetByIdAsync(postId))
                .ReturnsAsync(post);

            _postsRepoMock.Setup(x => x.GetAllPostCountAsync(userId))
                .ReturnsAsync(3);

            _commentsRepoMock.Setup(x => x.GetAllCommentsCountAsync(userId))
                .ReturnsAsync(5);

           
            var result = await _service.GetUserComplaint(postId);

          
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);

            Assert.AreEqual(postId, result.Data.Id);
            Assert.AreEqual("Test Title", result.Data.Title);
            Assert.AreEqual("Test Content", result.Data.Content);
            Assert.AreEqual("Jon Snow", result.Data.AuthorName);

            Assert.AreEqual(3, result.Data.UserPostCount);
            Assert.AreEqual(5, result.Data.UserCommentCount);
        
        
        
        
        
        }





    }
}