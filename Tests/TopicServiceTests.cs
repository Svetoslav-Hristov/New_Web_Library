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

namespace AspNetCoreArchTemplate.Services.Core.Tests
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
                CategoryId=1
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
        public async Task EditSubCategory_ShouldBeCorrect_WhenExist()
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

                SubCategoryId=1,
                TopicName = " "


            };

            var result = await _service.ConfirmEditSubCategory(subCategory,1,userId);

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
        public async Task ConfirmEditSubCategory_ShouldReturnError_WhenUserNotExist()
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
        public async Task ConfirmEditSubCategory_ShouldReturnError_WhenSubCategoryNotExist()
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

            var result = await _service.ConfirmEditSubCategory(model,subCategoryId, userId);

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
                Id=1,
               Title="Test",
               UserId=Guid.NewGuid()


            };



            _topicsRepoMock.Setup(x => x.GetByIdAsync<Topic>(subCategory.Id))
              .ReturnsAsync(subCategory);

            var result = await _service.ConfirmEditSubCategory(model,subCategory.Id, user.Id);

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





    }
}
