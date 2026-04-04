using Microsoft.Extensions.Logging;
using Moq;
using New_Library.Data.Models.Forum;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data.Models;
using New_Web_Library.Service.Core;
using New_Web_Library.Service.Core.Interfaces;
using New_Web_Library.ViewModels.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace New_Web_Library.Services.Core.Tests
{
    [TestFixture]
    public class TopicServiceTests
    {
        private Mock<ITopicRepository> _topicsRepoMock;
        private Mock<ICategoryRepository> _categoriesRepoMock;
        private Mock<IUserRepository> _usersRepoMock;
        private Mock<ILogger<ITopicService>> _loggerMock;

        private TopicService _service;

        [SetUp]
        public void SetUp()
        {
            _topicsRepoMock = new Mock<ITopicRepository>();
            _categoriesRepoMock = new Mock<ICategoryRepository>();
            _usersRepoMock = new Mock<IUserRepository>();
            _loggerMock = new Mock<ILogger<ITopicService>>();

            _service = new TopicService(_topicsRepoMock.Object, _categoriesRepoMock.Object
                , _usersRepoMock.Object, _loggerMock.Object);


        }


        [Test]
        public async Task CreateNewSubCategory_ShouldReturnError_WhenCategoryMissing()
        {



            var result = await _service.CreateNewSubCategory(1);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Category not found!", result.ErrorMessage);


        }



        [Test]
        public async Task CreateNewSubCategory_ShouldReturnSuccess_WhenCategoryExist()
        {
            Category category = new Category()
            {
                Id = 1,
                Name = "Test"

            };


            _categoriesRepoMock.Setup(x => x.GetByIdAsync<Category>(1)).ReturnsAsync(category);

            var result = await _service.CreateNewSubCategory(1);

            Assert.IsTrue(result.Success);

            Assert.NotNull(result.Data);

            Assert.AreEqual(result.Data.CategoryId, category.Id);

        }


        [Test]
        public async Task ConfirmCreationNewSubcategory_ShouldReturnError_WhenNameIsEmpty()
        {

            CreateSubCategoryViewModel model = new CreateSubCategoryViewModel()
            {

                TopicName = " ",

            };

            var result = await _service.ConfirmCreationNewSubcategory(model, Guid.NewGuid());

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Sub category name is required.", result.ErrorMessage);


        }


        [Test]
        public async Task ConfirmCreationNewSubcategory_ShouldReturnError_WhenUserIdIsEmpty()
        {

            CreateSubCategoryViewModel model = new CreateSubCategoryViewModel()
            {

                TopicName = "Test",
                CategoryId = 1
            };

            var result = await _service.ConfirmCreationNewSubcategory(model, Guid.Empty);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Invalid user Id!", result.ErrorMessage);

            Assert.IsNull(result.Data);

        }

        [Test]
        public async Task ConfirmCreationNewSubcategory_ShouldReturnError_WhenUserIsMissing()
        {

            Guid userId = Guid.NewGuid();

            CreateSubCategoryViewModel model = new CreateSubCategoryViewModel()
            {

                TopicName = "Test",
                CategoryId = 1
            };

            _usersRepoMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync((User)null);


            var result = await _service.ConfirmCreationNewSubcategory(model, userId);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("User not found!", result.ErrorMessage);


        }


        [Test]
        public async Task ConfirmCreationNewSubcategory_ShouldReturnError_WhenAddThrowsException()
        {
            var userId = Guid.NewGuid();

            var model = new CreateSubCategoryViewModel
            {
                TopicName = "Test",
                CategoryId = 1
            };

            var user = new User
            {
                Id = userId,
                FirstName = "Ivan",
                LastName = "Petrov"
            };

            _usersRepoMock.Setup(x => x.FindByIdAsync(userId))
              .ReturnsAsync(user);

            _topicsRepoMock.Setup(x => x.AddAsync(It.IsAny<Topic>()))
              .ThrowsAsync(new Exception());

            var result = await _service.ConfirmCreationNewSubcategory(model, userId);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Unexpected error is occurred while create new sub-category! Please try again later.",
                result.ErrorMessage);


        }

        [Test]
        public async Task ConfirmCreationNewSubcategory_ShouldCreate_WhenValid()
        {
            var userId = Guid.NewGuid();

            CreateSubCategoryViewModel model = new CreateSubCategoryViewModel
            {
                TopicName = "Test",
                CategoryId = 1
            };

            _usersRepoMock.Setup(x => x.FindByIdAsync(userId))
              .ReturnsAsync(new User { Id = userId });

            _topicsRepoMock.Setup(x => x.IsExistWithSameName(It.IsAny<string>(), model.CategoryId))
                .ReturnsAsync(false);

            var result = await _service.ConfirmCreationNewSubcategory(model, userId);

            Assert.IsTrue(result.Success);

            Assert.IsNotNull(result.Data);

            Assert.AreEqual(model.TopicName, result.Data.Title);



        }


        [Test]
        public async Task EditSubCategory_ShouldReturnError_WhenNotFound()
        {

            var result = await _service.EditSubCategory(1);


            Assert.IsFalse(result.Success);
            Assert.AreEqual("SubCategory not found!", result.ErrorMessage);



        }


        [Test]
        public async Task EditSubCategory_ShouldBeCorrect_WhenExists()
        {
            Topic subCategory = new Topic()
            {
                Id = 1,
                Title = "Test",


            };

            _topicsRepoMock.Setup(x => x.GetByIdAsync<Topic>(1)).ReturnsAsync(subCategory);

            var result = await _service.EditSubCategory(1);

            Assert.IsTrue(result.Success);

            Assert.IsNotNull(result.Data);



        }


        [Test]
        public async Task ConfirmEditSubCategory_ShouldReturnError_WhenTopicNameIsEmpty()
        {
            Guid userId = Guid.NewGuid();

            CreateSubCategoryViewModel subCategory = new CreateSubCategoryViewModel()
            {

                SubCategoryId = 1,
                TopicName = " "


            };

            var result = await _service.ConfirmEditSubCategory(subCategory, 1, userId);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Sub category name is required.", result.ErrorMessage);


        }

        [Test]
        public async Task ConfirmEditSubCategory_ShouldReturnError_WhenUserIdIsEmpty()
        {
            Guid userId = Guid.Empty;

            CreateSubCategoryViewModel subCategory = new CreateSubCategoryViewModel()
            {

                SubCategoryId = 1,
                TopicName = "Test"


            };

            var result = await _service.ConfirmEditSubCategory(subCategory, 1, userId);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Invalid user ID", result.ErrorMessage);


        }


        [Test]
        public async Task ConfirmEditSubCategory_ShouldReturnError_WhenUserNotExists()
        {
            Guid userId = Guid.NewGuid();

            CreateSubCategoryViewModel subCategory = new CreateSubCategoryViewModel()
            {

                SubCategoryId = 1,
                TopicName = "Test"


            };

            _usersRepoMock.Setup(x => x.FindByIdAsync(userId))
              .ReturnsAsync((User)null);

            var result = await _service.ConfirmEditSubCategory(subCategory, 1, userId);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("User not found!", result.ErrorMessage);


        }

        [Test]
        public async Task ConfirmEditSubCategory_ShouldReturnError_WhenSubCategoryNotExists()
        {
            Guid userId = Guid.NewGuid();

            int subCategoryId = 1;

            var user = new User
            {
                Id = userId,
                FirstName = "Jon",
                LastName = "Smith"
            };

            _usersRepoMock.Setup(x => x.FindByIdAsync(userId))
              .ReturnsAsync(user);

            CreateSubCategoryViewModel model = new CreateSubCategoryViewModel
            {
                TopicName = "Test"
            };

            _topicsRepoMock.Setup(x => x.GetByIdAsync<Topic>(subCategoryId))
              .ReturnsAsync((Topic)null);

            var result = await _service.ConfirmEditSubCategory(model, subCategoryId, userId);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Invalid Sub Category!", result.ErrorMessage);
        }

        [Test]
        public async Task ConfirmEditSubCategory_ShouldReturnError_WhenUserDoesNoHavePermission()
        {


            User user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Jon",
                LastName = "Smith"
            };

            _usersRepoMock.Setup(x => x.FindByIdAsync(user.Id))
              .ReturnsAsync(user);

            CreateSubCategoryViewModel model = new CreateSubCategoryViewModel
            {
                SubCategoryId = 1,
                TopicName = "Test"
            };

            Topic subCategory = new Topic()
            {
                Id = 1,
                Title = "Test",
                UserId = Guid.NewGuid()


            };



            _topicsRepoMock.Setup(x => x.GetByIdAsync<Topic>(subCategory.Id))
              .ReturnsAsync(subCategory);

            var result = await _service.ConfirmEditSubCategory(model, subCategory.Id, user.Id);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("You don't have permission to edit this sub-category!", result.ErrorMessage);
        }


        [Test]
        public async Task ConfirmEditSubCategory_ShouldReturnError_WhenUpdateThrowsException()
        {
            var userId = Guid.NewGuid();

            CreateSubCategoryViewModel model = new CreateSubCategoryViewModel
            {
                TopicName = "New Title"
            };

            var user = new User
            {
                Id = userId
            };

            var subCategory = new Topic
            {
                Id = 1,
                Title = "Old Title",
                UserId = userId
            };

            _usersRepoMock.Setup(x => x.FindByIdAsync(userId))
              .ReturnsAsync(user);

            _topicsRepoMock.Setup(x => x.GetByIdAsync<Topic>(1))
                .ReturnsAsync(subCategory);

            _topicsRepoMock.Setup(x => x.UpdateAsync(It.IsAny<Topic>()))
                .ThrowsAsync(new Exception());

            var result = await _service.ConfirmEditSubCategory(model, 1, userId);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Unexpected error is occurred while edit sub-category! Please try again later.",
              result.ErrorMessage);
        }


        [Test]
        public async Task ConfirmEditSubCategory_ShouldUpdate_WhenDataIsValidAndTitleChanged()
        {
            var userId = Guid.NewGuid();

            CreateSubCategoryViewModel model = new CreateSubCategoryViewModel
            {
                TopicName = "New Title"
            };

            var user = new User { Id = userId };

            var subCategory = new Topic
            {
                Id = 1,
                Title = "Old Title",
                UserId = userId
            };

            _usersRepoMock.Setup(x => x.FindByIdAsync(userId))
              .ReturnsAsync(user);

            _topicsRepoMock.Setup(x => x.GetByIdAsync<Topic>(1))
              .ReturnsAsync(subCategory);

            var result = await _service.ConfirmEditSubCategory(model, 1, userId);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("New Title", subCategory.Title);

            _topicsRepoMock.Verify(x => x.UpdateAsync(subCategory),
              Times.Once);



        }



        [Test]
        public async Task SubCategoryIndexPreview_ShouldReturnFail_WhenSubCategoryIsNull()
        {

            int id = 1;

            _topicsRepoMock.Setup(r => r.GetAllSubCategoryWithComments(id))
               .ReturnsAsync((Topic)null);


            var result = await _service.SubCategoryIndexPreview(id);


            Assert.IsFalse(result.Success);
            Assert.AreEqual("SubCategory not found!", result.ErrorMessage);
        }


        [Test]
        public async Task SubCategoryIndexPreview_ShouldReturnModel_WhenSubCategoryExists()
        {

            int id = 1;

            var topic = new Topic
            {
                Id = id,
                Title = "Test Category",
                Posts = new List<Post>
            {
                new Post
                {
                    Id = 10,
                    Title = "Post 1",
                    CreatedOn = DateTime.Now,
                    User = new User
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    },
                      Comments = new List<Comment>
                   {
                    new Comment(),
                    new Comment()
                   }
                }
             }
            };

            _topicsRepoMock.Setup(r => r.GetAllSubCategoryWithComments(id))
                .ReturnsAsync(topic);


            var result = await _service.SubCategoryIndexPreview(id);


            Assert.IsTrue(result.Success);

            var model = result.Data;

            Assert.AreEqual("Test Category", model.CategoryName);
            Assert.AreEqual(id, model.CategoryId);

            Assert.AreEqual(1, model.Posts.Count);

            var post = model.Posts.First();

            Assert.AreEqual(10, post.Id);
            Assert.AreEqual("Post 1", post.PostTitle);
            Assert.AreEqual("John Doe", post.PostAuthor);
            Assert.AreEqual(2, post.CommentCount);
        }

        [Test]
        public async Task SoftDeleteTopic_ShouldReturnError_WhenUserIdIsEmpty()
        {

            int subCategoriId = 1;
            Guid userId = Guid.Empty;

            var result = await _service.SoftDeleteSubCategory(subCategoriId, userId);


            Assert.IsFalse(result.Success);

            Assert.AreEqual("Invalid user ID!", result.ErrorMessage);


        }




        [Test]
        public async Task SoftDeleteTopic_ShouldReturnError_WhenUserNoExists()
        {
            int subCategoryId = 1;
            Guid userId = Guid.NewGuid();


            _usersRepoMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync((User)null);


            var result = await _service.SoftDeleteSubCategory(subCategoryId, userId);


            Assert.IsFalse(result.Success);

            Assert.AreEqual("User not found!", result.ErrorMessage);

        }


        [Test]
        public async Task SoftDeleteTopic_ShouldReturnError_WhenSubCategoryNoExists()
        {

            int subCategoryId = 1;
            Guid userId = Guid.NewGuid();

            User user = new User()
            {
                Id = userId,
                FirstName = "Jon",
                LastName = "Snow",
                Address = "Winterfell"

            };


            _usersRepoMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

            _topicsRepoMock.Setup(x => x.GetByIdAsync<Topic>(1)).ReturnsAsync((Topic)null);



            var result = await _service.SoftDeleteSubCategory(subCategoryId, userId);


            Assert.IsFalse(result.Success);

            Assert.AreEqual("SubCategory not exist!", result.ErrorMessage);

        }

        [Test]
        public async Task SoftDeleteTopic_ShouldReturnError_WhenUserHasNoPermission()
        {

            int subCategoryId = 1;
            Guid userId = Guid.NewGuid();

            User user = new User()
            {
                Id = userId,
                FirstName = "Jon",
                LastName = "Snow",
                Address = "Winterfell"

            };

            Topic subCategory = new Topic()
            {
                Id = subCategoryId,
                Title = "Test",
                UserId = Guid.NewGuid(),
                IsDeleted = false

            };


            _usersRepoMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

            _topicsRepoMock.Setup(x => x.GetByIdAsync<Topic>(1)).ReturnsAsync(subCategory);



            var result = await _service.SoftDeleteSubCategory(subCategoryId, userId);


            Assert.IsFalse(result.Success);

            Assert.AreEqual("You do not have permission to delete this subcategory.", result.ErrorMessage);

            Assert.IsFalse(subCategory.IsDeleted);
            Assert.IsNull(subCategory.DeleteAt);

        }


        [Test]
        public async Task SoftDeleteSubCategory_ShouldReturnFail_WhenExceptionThrown()
        {

            int id = 1;
            var userId = Guid.NewGuid();

            var topic = new Topic { UserId = userId };

            _usersRepoMock.Setup(u => u.FindByIdAsync(userId))
                .ReturnsAsync(new User { Id = userId });

            _topicsRepoMock.Setup(t => t.GetByIdAsync<Topic>(id))
                .ReturnsAsync(topic);

            _topicsRepoMock.Setup(t => t.UpdateAsync(topic))
                .ThrowsAsync(new Exception());


            var result = await _service.SoftDeleteSubCategory(id, userId);


            Assert.IsFalse(result.Success);
            Assert.AreEqual("Unexpected error is occurred while delete sub category! Please try again later.", result.ErrorMessage);



        }

        [Test]
        public async Task SoftDeleteSubCategory_ShouldReturnSuccess_WhenValid()
        {

            int id = 1;
            var userId = Guid.NewGuid();

            var topic = new Topic { UserId = userId };

            _usersRepoMock.Setup(u => u.FindByIdAsync(userId))
                .ReturnsAsync(new User { Id = userId });

            _topicsRepoMock.Setup(t => t.GetByIdAsync<Topic>(id))
                .ReturnsAsync(topic);

            _topicsRepoMock.Setup(t => t.UpdateAsync(topic))
                .Returns(Task.CompletedTask);


            var result = await _service.SoftDeleteSubCategory(id, userId);


            Assert.IsTrue(result.Success);
            Assert.IsTrue(topic.IsDeleted);
            Assert.IsNotNull(topic.DeleteAt);

        }


        [Test]
        public async Task HardDeleteSubCategory_ShouldReturnError_WhenUserIdIsEmpty()
        {

            int subCategoryId = 1;

            var result = await _service.HardDeleteSubCategory(subCategoryId, Guid.Empty);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Invalid user ID!", result.ErrorMessage);

        }



        [Test]
        public async Task HardDeleteSubCategory_ShouldReturnError_WhenUserNotExists()
        {

            int subCategoryId = 1;
            Guid userId = Guid.NewGuid();



            _usersRepoMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync((User)null);

            var result = await _service.HardDeleteSubCategory(subCategoryId, userId);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("User not found!", result.ErrorMessage);


        }

        [Test]
        public async Task HardDeleteSubCategory_ShouldReturnError_WhenSubCategoryNotExists()
        {

            int subCategoryId = 1;
            Guid userId = Guid.NewGuid();


            User user = new User()
            {
                Id = userId,
                FirstName = "Jon",
                LastName = "Snow",
                Address = "Winterfell"

            };




            _usersRepoMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

            _topicsRepoMock.Setup(x => x.GetByIdAsync<Topic>(subCategoryId)).ReturnsAsync((Topic)null);


            var result = await _service.HardDeleteSubCategory(subCategoryId, userId);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Sub Category not found!", result.ErrorMessage);


        }

        [Test]
        public async Task HardDeleteSubCategory_ShouldReturnError_WhenSubCategoryHasPosts()
        {
            int id = 1;
            Guid userId = Guid.NewGuid();

            var user = new User { Id = userId };

            var subCategory = new Topic
            {
                Id = id,
                UserId = userId,
                Posts = new List<Post>
             {
               new Post()

            }
            };

            _usersRepoMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            _topicsRepoMock.Setup(x => x.GetDeleteOrNotSubCategoryAsync(id))
                .ReturnsAsync(subCategory);

            var result = await _service.HardDeleteSubCategory(id, userId);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Sub Category has posts!", result.ErrorMessage);
        }


        [Test]
        public async Task HardDeleteSubCategory_ShouldReturnError_WhenExceptionThrown()
        {
            int id = 1;
            Guid userId = Guid.NewGuid();

            var user = new User { Id = userId };

            var subCategory = new Topic
            {
                Id = id,
                UserId = userId,
                Posts = new List<Post>()
            };

            _usersRepoMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            _topicsRepoMock.Setup(x => x.GetDeleteOrNotSubCategoryAsync(id))
                .ReturnsAsync(subCategory);

            _topicsRepoMock.Setup(x => x.DeleteAsync(subCategory))
                .ThrowsAsync(new Exception());

            var result = await _service.HardDeleteSubCategory(id, userId);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(
                "Unexpected error is occurred while hard delete SubCategory! Please try again later.",
                result.ErrorMessage);
        }


        [Test]
        public async Task HardDeleteSubCategory_ShouldReturnSuccess_WhenValid()
        {
            int id = 1;
            Guid userId = Guid.NewGuid();

            var user = new User { Id = userId };

            var subCategory = new Topic
            {
                Id = id,
                UserId = userId,
                Posts = new List<Post>()
            };

            _usersRepoMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            _topicsRepoMock.Setup(x => x.GetDeleteOrNotSubCategoryAsync(id))
                .ReturnsAsync(subCategory);

            _topicsRepoMock.Setup(x => x.DeleteAsync(subCategory))
                .Returns(Task.CompletedTask);

            var result = await _service.HardDeleteSubCategory(id, userId);

            Assert.IsTrue(result.Success);


        }


        [Test]
        public async Task RestoreSubCategory_ShouldReturnError_WhenUserIdIsEmpty()
        {
            Guid userId = Guid.Empty;
            int subCategoryId = 1;


            var result = await _service.RestoreSubCategory(subCategoryId, userId);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Invalid user ID!", result.ErrorMessage);

        }

        [Test]
        public async Task RestoreSubCategory_ShouldReturnError_WhenUserNotExists()
        {
            Guid userId = Guid.NewGuid();
            int subCategoryId = 1;



            _usersRepoMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync((User)null);

            var result = await _service.RestoreSubCategory(subCategoryId, userId);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("User not found!", result.ErrorMessage);

        }

        [Test]
        public async Task RestoreSubCategory_ShouldReturnError_WhenCategoryNotExists()
        {
            Guid userId = Guid.NewGuid();
            int subCategoryId = 1;

            User user = new User()
            {
                Id = userId,
                FirstName = "Jon",
                LastName = "Snow",
                Address = "WinterFell"

            };



            _usersRepoMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

            _topicsRepoMock.Setup(x => x.GetByIdAsync<Topic>(subCategoryId)).ReturnsAsync((Topic)null);

            var result = await _service.RestoreSubCategory(subCategoryId, userId);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Sub Category not found!", result.ErrorMessage);

        }



        [Test]
        public async Task RestoreSubCategory_ShouldFail_WhenCategoryIsDeleted()
        {
            int id = 1;
            Guid userId = Guid.NewGuid();

            var user = new User { Id = userId };

            var subCategory = new Topic
            {
                Id = id,
                CategoryId = 5,
                UserId = userId
            };

            var category = new Category
            {
                Id = 5,
                IsDeleted = true
            };

            _usersRepoMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

            _topicsRepoMock.Setup(x => x.GetDeleteOrNotSubCategoryAsync(id))
                .ReturnsAsync(subCategory);

            _categoriesRepoMock.Setup(x => x.GetDeleteOrNotCategoryAsync(5))
                .ReturnsAsync(category);

            var result = await _service.RestoreSubCategory(id, userId);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("You won't be able to return the SubCategory because the Category is also missing!",
                result.ErrorMessage);



        }

        [Test]
        public async Task RestoreSubCategory_ShouldFail_WhenUserHasNoPermission()
        {
            int id = 1;
            Guid userId = Guid.NewGuid();

            var user = new User { Id = userId };

            var subCategory = new Topic
            {
                Id = id,
                CategoryId = 5,
                UserId = Guid.NewGuid()
            };

            _usersRepoMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

            _topicsRepoMock.Setup(x => x.GetDeleteOrNotSubCategoryAsync(id))
                .ReturnsAsync(subCategory);

            _categoriesRepoMock.Setup(x => x.GetDeleteOrNotCategoryAsync(5))
                .ReturnsAsync((Category)null);

            var result = await _service.RestoreSubCategory(id, userId);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("You do not have permission to restore this subcategory.",
                result.ErrorMessage);



        }

        [Test]
        public async Task RestoreSubCategory_ShouldFail_WhenExceptionThrown()
        {
            int id = 1;
            Guid userId = Guid.NewGuid();

            var user = new User { Id = userId };

            var subCategory = new Topic
            {
                Id = id,
                CategoryId = 5,
                UserId = userId
            };

            _usersRepoMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

            _topicsRepoMock.Setup(x => x.GetDeleteOrNotSubCategoryAsync(id))
                .ReturnsAsync(subCategory);

            _categoriesRepoMock.Setup(x => x.GetDeleteOrNotCategoryAsync(5))
                .ReturnsAsync((Category)null);

            _topicsRepoMock.Setup(x => x.UpdateAsync(subCategory))
                .ThrowsAsync(new Exception());

            var result = await _service.RestoreSubCategory(id, userId);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Unexpected error is occurred while restore SubCategory! Please try again later.",
                result.ErrorMessage);



        }



        [Test]
        public async Task RestoreSubCategory_ShouldReturnSuccess_WhenValid()
        {
            int id = 1;
            Guid userId = Guid.NewGuid();

            var user = new User { Id = userId };

            var subCategory = new Topic
            {
                Id = id,
                CategoryId = 5,
                UserId = userId,
                IsDeleted = true
            };

            _usersRepoMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

            _topicsRepoMock
                .Setup(x => x.GetDeleteOrNotSubCategoryAsync(id))
                .ReturnsAsync(subCategory);

            _categoriesRepoMock.Setup(x => x.GetDeleteOrNotCategoryAsync(5))
                .ReturnsAsync((Category)null);

            _topicsRepoMock.Setup(x => x.UpdateAsync(subCategory))
                .Returns(Task.CompletedTask);

            var result = await _service.RestoreSubCategory(id, userId);

            Assert.IsTrue(result.Success);
            Assert.IsFalse(subCategory.IsDeleted);
            Assert.IsNull(subCategory.DeleteAt);


        }


        [Test]
        public async Task GetOrCreateSpecialSubCategory_ShouldReturnError_WhenUserIdIsEmpty()
        {
            int specialId = 1;

            Guid userId = Guid.Empty;

            Topic special = new Topic()
            {
                Id = specialId,
                Title = "Special",



            };

            _topicsRepoMock.Setup(x => x.GetSubCategoryByName(special.Title)).ReturnsAsync(special);

            var result = await _service.GetOrCreateSpecialSubCategory(userId);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Invalid user ID!", result.ErrorMessage);
            Assert.IsNull(result.Data);




        }

        [Test]
        public async Task GetOrCreateSpecialSubCategory_ShouldReturnError_WhenUserNotExists()
        {
            int specialId = 1;

            Guid userId = Guid.NewGuid();

            Topic special = new Topic()
            {
                Id = specialId,
                Title = "Special",



            };

            _topicsRepoMock.Setup(x => x.GetSubCategoryByName(special.Title)).ReturnsAsync(special);

            _usersRepoMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync((User)null);

            var result = await _service.GetOrCreateSpecialSubCategory(userId);

            Assert.IsFalse(result.Success);
            
            Assert.AreEqual("User not found!", result.ErrorMessage);
           
            Assert.IsNull(result.Data);




        }

        [Test]
        public async Task GetOrCreateSpecialSubCategory_ShouldReturnSuccess_WhenSubCategoryExists()
        {
            int specialId = 1;

            Guid userId = Guid.NewGuid();

            User user = new User()
            {
                Id = userId,
                FirstName = "Jon",
                LastName = "Snow",
                Address = "Winterfell"

            };




            Topic special = new Topic()
            {
                Id = specialId,
                Title = "Special",



            };

            _topicsRepoMock.Setup(x => x.GetSubCategoryByName(special.Title)).ReturnsAsync(special);

            _usersRepoMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

            var result = await _service.GetOrCreateSpecialSubCategory(userId);

            Assert.IsTrue(result.Success);

            Assert.IsNotNull(result.Data);




        }


        [Test]
        public async Task GetOrCreateSpecialSubCategory_ShouldFail_WhenNoCategoryExists()
        {
            Guid userId = Guid.NewGuid();

            var user = new User { Id = userId };

            _usersRepoMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            _topicsRepoMock.Setup(x => x.GetSubCategoryByName(It.IsAny<string>()))
                .ReturnsAsync((Topic)null);

            _categoriesRepoMock.Setup(x => x.LastCategoryAsync())
                .ReturnsAsync((Category)null);

            var result = await _service.GetOrCreateSpecialSubCategory(userId);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(
                "No category found to assign  the special sub category!",
                result.ErrorMessage);
        }


        [Test]
        public async Task GetOrCreateSpecialSubCategory_ShouldFail_WhenExceptionThrown()
        {
            Guid userId = Guid.NewGuid();

            var user = new User { Id = userId };

            var category = new Category { Id = 5 };

            _usersRepoMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            _topicsRepoMock.Setup(x => x.GetSubCategoryByName(It.IsAny<string>()))
                .ReturnsAsync((Topic)null);

            _categoriesRepoMock.Setup(x => x.LastCategoryAsync())
                .ReturnsAsync(category);

            _topicsRepoMock.Setup(x => x.AddAsync(It.IsAny<Topic>()))
                .ThrowsAsync(new Exception());

            var result = await _service.GetOrCreateSpecialSubCategory(userId);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Unexpected error is occurred while create special SubCategory!Please try again later. ",
                result.ErrorMessage);
        }


    }
}
