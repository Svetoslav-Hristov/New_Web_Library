using Microsoft.Extensions.Logging;
using Moq;
using New_Library.Data.Models.Forum;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Service.Core;
using New_Web_Library.Service.Core.Interfaces;
using New_Web_Library.ViewModels.Forum;

namespace New_Web_Library.Services.Core.Tests
{
    [TestFixture]
    public class CategoryServiceTest
    {
        private Mock<ICategoryRepository> _categoriesRepoMock;
        private Mock<ITopicRepository> _topicsRepoMock;
        private Mock<IUserRepository> _userRepoMock;
        private Mock<ILogger<ICategoryService>> _loggerMock;

        private CategoryService _service;

        [SetUp]
        public void SetUp()
        {
            _categoriesRepoMock = new Mock<ICategoryRepository>();
            _topicsRepoMock = new Mock<ITopicRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _loggerMock = new Mock<ILogger<ICategoryService>>();

            _service = new CategoryService(_categoriesRepoMock.Object, _topicsRepoMock.Object
                , _userRepoMock.Object, _loggerMock.Object);

        }


        [Test]
        public async Task IndexPreview_ShouldReturnCategories_WhenNoSpecialCategory()
        {

            var categories = new List<Category>
            {
               new Category
               {
                 Id = 1,
                 Name = "Cat1",
                 Topics = new List<Topic>
                 {new Topic
                 {
                    Id = 1,
                    Title = "Topic1",
                    Posts = new List<Post>
                    { new Post
                       {
                          Title = "Post1",
                          CreatedOn = DateTime.UtcNow

                        }
                     }
                   }
                 }
               }
            };


            _categoriesRepoMock.Setup(x => x.GetAllCategoriesWithSubCategoriesAsync(It.IsAny<int?>()))
              .ReturnsAsync(categories);

            _topicsRepoMock.Setup(x => x.GetSubCategoryByName(It.IsAny<string>()))
              .ReturnsAsync((Topic)null);

            var result = await _service.IndexPreview();


            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());


        }


        [Test]
        public async Task IndexPreview_ShouldIgnoreSpecialCategory_WhenExists()
        {

            var special = new Topic
            {
                Id = 99,
                Title = "Special"
            };

            var categories = new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Cat1",
                    Topics = new List<Topic>
                {
               new Topic
                {
                    Id = 99,
                    Title = "Special",
                    Posts = new List<Post>()
                },
               new Topic
                {
                    Id = 1,
                    Title = "Normal",
                    Posts = new List<Post>
                {
                  new Post
                  {
                    Title = "Post1",
                    CreatedOn = DateTime.UtcNow,
                    TopicId = 1
                  }
                }

               }
              }
             }
            };



            _categoriesRepoMock.Setup(x => x.GetAllCategoriesWithSubCategoriesAsync(It.IsAny<int?>()))
              .ReturnsAsync(categories);

            _topicsRepoMock.Setup(x => x.GetSubCategoryByName(It.IsAny<string>()))
               .ReturnsAsync(special);


            var result = await _service.IndexPreview();

            Assert.IsTrue(result.Any());



        }


        [Test]
        public async Task ConfirmCategory_ShouldReturnError_WhenModelIsNull()
        {

            var result = await _service.ConfirmNewCategory(null);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Invalid data!", result.ErrorMessage);

        }


        [Test]
        public async Task ConfirmCategory_ShouldReturnError_WhenNameIsEmpty()
        {

            var model = new CategoryFormModel
            {
                Name = "   "
            };



            var result = await _service.ConfirmNewCategory(model);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Category name is required.", result.ErrorMessage);


        }


        [Test]
        public async Task ConfirmCategory_ShouldReturnError_WhenCategoryExists()
        {

            CategoryFormModel model = new CategoryFormModel()
            {

                Name = "Test"


            };


            _categoriesRepoMock.Setup(x => x.ExistByNameAsync("Test")).ReturnsAsync(true);

            var result = await _service.ConfirmNewCategory(model);


            Assert.IsFalse(result.Success);
            Assert.AreEqual("A category with that name already exists.", result.ErrorMessage);



        }


        [Test]
        public async Task ConfirmCategory_ShouldReturnError_WhenAddFails()
        {
            CategoryFormModel model = new CategoryFormModel()
            {

                Name = "Test"

            };

            _categoriesRepoMock.Setup(x => x.ExistByNameAsync(It.IsAny<string>()))
              .ReturnsAsync(false);

            _categoriesRepoMock.Setup(x => x.AddAsync(It.IsAny<Category>()))
                .ThrowsAsync(new Exception());


            var result = await _service.ConfirmNewCategory(model);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Unexpected error is occurred while create new category! Please try again later."
                , result.ErrorMessage);


        }


        [Test]
        public async Task ConfirmCategory_ShouldCreateCategory_WhenValid()
        {

            CategoryFormModel model = new CategoryFormModel()
            {

                Name = " Test ",
                Description = "Some Description"

            };

            _categoriesRepoMock.Setup(x => x.ExistByNameAsync("test")).ReturnsAsync(false);

            var result = await _service.ConfirmNewCategory(model);

            Assert.IsTrue(result.Success);

            _categoriesRepoMock.Verify(x => x.AddAsync(It.IsAny<Category>()), Times.Once);
        }



        [Test]

        public async Task EditCategory_ShouldReturnError_WhenCategoryNotFound()
        {

            _categoriesRepoMock.Setup(x => x.GetByIdAsync<Category>(It.IsAny<int>()))
               .ReturnsAsync((Category)null);


            var result = await _service.EditCategory(1);

            Assert.IsFalse(result.Success);

            Assert.IsNull(result.Data);

            Assert.AreEqual("Category not found!", result.ErrorMessage);

        }

        [Test]
        public async Task EditCategory_ShouldReturnModel_WhenCategoryExists()
        {

            Category category = new Category()
            {
                Id = 1,
                Name = "Test",
                Description = "Some description"


            };

            _categoriesRepoMock.Setup(x => x.GetByIdAsync<Category>(1)).ReturnsAsync(category);


            var result = await _service.EditCategory(1);

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);


            Assert.AreEqual(category.Name, result.Data.Name);
            Assert.AreEqual(category.Description, result.Data.Description);

        }


        [Test]
        public async Task ConfirmEditCategory_ShouldReturnError_WhenModelIsNull()
        {

            var result = await _service.ConfirmEditCategory(null, 1);


            Assert.IsFalse(result.Success);

            Assert.IsNull(result.Data);

            Assert.AreEqual("Invalid data!", result.ErrorMessage);

        }

        [Test]
        public async Task ConfirmEditCategory_ShouldReturnError_WhenNameIsEmpty()
        {

            CategoryFormModel model = new CategoryFormModel()
            {
                Id = 1,
                Name = "",


            };


            var result = await _service.ConfirmEditCategory(model, 1);


            Assert.IsFalse(result.Success);

            Assert.IsNull(result.Data);

            Assert.AreEqual("Category name is required.", result.ErrorMessage);

        }




        [Test]
        public async Task ConfirmEditCategory_ShouldReturnError_WhenCategoryNotExist()

        {
            CategoryFormModel model = new CategoryFormModel

            {
                Name = "Test"
            };



            _categoriesRepoMock.Setup(x => x.GetByIdAsync<Category>(1))
              .ReturnsAsync((Category)null);


            var result = await _service.ConfirmEditCategory(model, 1);


            Assert.IsFalse(result.Success);

            Assert.AreEqual("Category not found!", result.ErrorMessage);


        }


        [Test]
        public async Task ConfirmEditCategory_ShouldReturnError_WhenNameExists()
        {

            CategoryFormModel model = new CategoryFormModel()
            {

                Name = "Test"

            };


            _categoriesRepoMock.Setup(x => x.GetByIdAsync<Category>(1)).ReturnsAsync(new Category());

            _categoriesRepoMock.Setup(x => x.ExistByNameAsync("test", 1)).ReturnsAsync(true);

            var result = await _service.ConfirmEditCategory(model, 1);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("A category with that name already exists.", result.ErrorMessage);

        }


        [Test]
        public async Task ConfirmEditCategory_ShouldReturnError_WhenUpdateFails()
        {

            CategoryFormModel model = new CategoryFormModel()
            {
                Name = "Test"
            };

            Category category = new Category()
            {
                Id = 1
            };

            _categoriesRepoMock.Setup(x => x.GetByIdAsync<Category>(1))
                .ReturnsAsync(category);

            _categoriesRepoMock.Setup(x => x.ExistByNameAsync(It.IsAny<string>(), 1))
                .ReturnsAsync(false);

            _categoriesRepoMock.Setup(x => x.UpdateAsync(It.IsAny<Category>()))
              .ThrowsAsync(new Exception());

            var result = await _service.ConfirmEditCategory(model, 1);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Unexpected error is occurred while edit category! Please try again later.", result.ErrorMessage);

        }


        [Test]
        public async Task ConfirmEditCategory_ShouldUpdateCategory_WhenValid()
        {

            CategoryFormModel model = new CategoryFormModel
            {
                Name = "Test",
                Description = "Desc"
            };

            var category = new Category { Id = 1 };

            _categoriesRepoMock.Setup(x => x.GetByIdAsync<Category>(1))
              .ReturnsAsync(category);

            _categoriesRepoMock.Setup(x => x.ExistByNameAsync("test", 1))
              .ReturnsAsync(false);

            var result = await _service.ConfirmEditCategory(model, 1);

            Assert.IsTrue(result.Success);

            _categoriesRepoMock.Verify(x => x.UpdateAsync(category), Times.Once);




        }


        [Test]
        public async Task SoftDeleteCategory_ShouldReturnError_WhenCategoryNotExist()
        {

            var result = await _service.SoftDeleteCategory(1);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Category not found!", result.ErrorMessage);



        }


        [Test]
        public async Task SoftDeleteCategory_ShouldReturnError_WhenCategoryIsAllReadyDelete()
        {
            Category category = new Category()
            {
                Id = 1,
                Name = "Test",
                IsDeleted = true

            };

            _categoriesRepoMock.Setup(x => x.GetByIdAsync<Category>(1)).ReturnsAsync(category);

            var result = await _service.SoftDeleteCategory(1);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Category is already deleted.", result.ErrorMessage);




        }

        [Test]
        public async Task SoftDeleteCategory_ShouldReturnError_WhenUpdateThrowsException()
        {
            Category category = new Category
            {
                Id = 1,
                Name = "Test",
                IsDeleted = false
            };

            _categoriesRepoMock.Setup(x => x.GetByIdAsync<Category>(1))
              .ReturnsAsync(category);

            _categoriesRepoMock.Setup(x => x.UpdateAsync(It.IsAny<Category>()))
               .ThrowsAsync(new Exception());

            var result = await _service.SoftDeleteCategory(1);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Unexpected error is occurred while delete category! Please try again later.", result.ErrorMessage);


        }



        [Test]
        public async Task SoftDeleteCategory_ShouldDeleteCorrectly_WhenCategoryExistAndNotDelete()
        {
            Category category = new Category
            {
                Id = 1,
                Name = "Test",
                IsDeleted = false
            };

            _categoriesRepoMock.Setup(x => x.GetByIdAsync<Category>(1)).ReturnsAsync(category);


            var result = await _service.SoftDeleteCategory(1);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(category.IsDeleted);
            Assert.IsNotNull(category.DeleteAt);

        }



        [Test]
        public async Task HardDeleteCategory_ShouldReturnError_WhenCategoryNotExist()
        {

            _categoriesRepoMock.Setup(x => x.GetByIdAsync<Category>(1))
              .ReturnsAsync((Category)null);

            var result = await _service.HardDeleteCategory(1);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Category not found!", result.ErrorMessage);



        }


        [Test]
        public async Task HardDeleteCategory_ShouldReturnError_WhenCategoryIsNotEmpty()
        {

            Category category = new Category()
            {
                Id = 1,
                Name = "Test",
                Topics = new List<Topic>
                {
                    new Topic()
                }

            };

            _categoriesRepoMock.Setup(x => x.GetDeleteOrNotCategoryAsync(1))
             .ReturnsAsync(category);

            var result = await _service.HardDeleteCategory(1);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Category is not empty!", result.ErrorMessage);
        }



        [Test]
        public async Task HardDeleteCategory_ShouldReturnError_WhenDeleteFails()
        {
            Category category = new Category
            {
                Id = 1,
                Name = "Test",
                Topics = new List<Topic>()
            };

            _categoriesRepoMock.Setup(x => x.GetDeleteOrNotCategoryAsync(1))
              .ReturnsAsync(category);

            _categoriesRepoMock.Setup(x => x.DeleteAsync(It.IsAny<Category>()))
               .ThrowsAsync(new Exception());

            var result = await _service.HardDeleteCategory(1);

            Assert.IsFalse(result.Success);
        
            Assert.AreEqual("Unexpected error is occurred while hard delete category! Please try again later.", result.ErrorMessage);
        
        
        
        }

        [Test]
        public async Task HardDeleteCategory_ShouldDelete_WhenValid()
        {
            Category category = new Category
            {
                Id = 1,
                Name = "Test",
                Topics = new List<Topic>()
            };

            _categoriesRepoMock.Setup(x => x.GetDeleteOrNotCategoryAsync(1))
              .ReturnsAsync(category);

            var result = await _service.HardDeleteCategory(1);

            Assert.IsTrue(result.Success);

            _categoriesRepoMock.Verify(x => x.DeleteAsync(category),Times.Once);
        
        
        
        }


        [Test]
        public async Task RestoreSofDeleteCategory_ShouldReturnError_WhenCategoryNotExist()
        {

            _categoriesRepoMock.Setup(x => x.GetByIdAsync<Category>(1))
                .ReturnsAsync((Category)null);


            var result = await _service.RestoreSoftDeleteCategory(1);

            Assert.IsFalse(result.Success);

            Assert.AreEqual("Category not found!", result.ErrorMessage);
        
        }



        [Test]
        public async Task RestoreSofDeleteCategory_ShouldRestoreCorrectly_WhenCategoryExist()
        {
            Category category = new Category()
            {
                Id = 1,
                Name = "Test",
                IsDeleted = true


            };



            _categoriesRepoMock.Setup(x => x.GetDeleteOrNotCategoryAsync(1))
                .ReturnsAsync(category);


            var result = await _service.RestoreSoftDeleteCategory(1);

            Assert.IsTrue(result.Success);

            Assert.IsFalse(category.IsDeleted);

            Assert.IsNull(category.DeleteAt);

        }


        [Test]
        public async Task RestoreSoftDeleteCategory_ShouldReturnError_WhenUpdateFails()
        {
            Category category = new Category 
            {
                Id = 1 
                
            };

          
            
            
            _categoriesRepoMock.Setup(x => x.GetDeleteOrNotCategoryAsync(1))
              .ReturnsAsync(category);

            _categoriesRepoMock.Setup(x => x.UpdateAsync(It.IsAny<Category>()))
                .ThrowsAsync(new Exception());

            var result = await _service.RestoreSoftDeleteCategory(1);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Unexpected error is occurred while restore category! Please try again later.", result.ErrorMessage);
        }






    }




}




