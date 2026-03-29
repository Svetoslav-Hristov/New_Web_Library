using Microsoft.Extensions.Logging;
using New_Library.Data.Models.Forum;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Service.Core.Interfaces;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.ViewModels.Forum;
using static New_Web_Library.GCommon.EntityValidations.Topics;

namespace New_Web_Library.Service.Core
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoriesRepository;
        private readonly ITopicRepository _topicsRepository;
        private readonly ILogger<ICategoryService> _logger;

        public CategoryService(ICategoryRepository categoriesRepository, ITopicRepository topicsRepository
            , ILogger<ICategoryService> logger)
        {
            this._categoriesRepository = categoriesRepository;
            this._topicsRepository = topicsRepository;
            this._logger = logger;

        }


        public async Task<IEnumerable<IndexForumModel>> IndexPreview()
        {

            List<Category> allCategories = await _categoriesRepository.GetAllCategoriesWithSubCategories();

            int specialCategoryId = 0;

            var specialCategory = await _topicsRepository.GetSubCategoryByName(TopicSpecialName);

            if (specialCategory != null)
            {
                specialCategoryId = specialCategory.Id;
            }

            IEnumerable<IndexForumModel> categories = allCategories.Select(c => new IndexForumModel()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Topics = c.Topics.Where(t => t.Title != TopicSpecialName).Select(t => new TopicForumModel()
                {

                    Id = t.Id,
                    Title = t.Title
                }).ToArray(),
                PostCount = c.Topics.SelectMany(t => t.Posts).Count(),
                LastPostTitle = c.Topics.SelectMany(t => t.Posts).OrderByDescending(p => p.CreatedOn)
                .Where(p => p.TopicId != specialCategoryId).Select(p => p.Title).FirstOrDefault(),
                LastActive = c.Topics.SelectMany(t => t.Posts).OrderByDescending(p => p.CreatedOn).
                 Where(p => p.TopicId != specialCategoryId).Select(p => (DateTime?)p.CreatedOn).FirstOrDefault()


            }).ToList();

            return categories;

        }
        public ServiceResult<CategoryFormModel> CreateNewCategory()
        {
            CategoryFormModel model = new CategoryFormModel();

            return new ServiceResult<CategoryFormModel> { Success = true, Data = model };

        }

        public async Task<ServiceResult<Category>> ConfirmNewCategory(CategoryFormModel model)
        {
            if (model == null)
            {
                return new ServiceResult<Category> { Success = false, ErrorMessage = "Invalid data!" };
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {

                return new ServiceResult<Category>
                {
                    Success = false,
                    ErrorMessage = "Category name is required."
                };

            }


            string name = model.Name.Trim();

            bool isExist = await _categoriesRepository.ExistByName(name.ToLowerInvariant());

            if (isExist)
            {
                return new ServiceResult<Category> { Success = false, ErrorMessage = "A category with that name already exists." };
            }


            Category newCategory = new Category()
            {

                Name = name,
                Description = model.Description,

            };

            try
            {
                await _categoriesRepository.AddAsync(newCategory);


            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error create category with title {Title}", model.Name);

                return new ServiceResult<Category>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while create new category! Please try again later."
                };

            }

            return new ServiceResult<Category> { Success = true };

        }

        public async Task<ServiceResult<CategoryFormModel>> EditCategory(int Id)
        {
            var category = await _categoriesRepository.GetByIdAsync<Category>(Id);

            if (category == null)
            {
                return new ServiceResult<CategoryFormModel> { Success = false, ErrorMessage = "Category not found!" };
            }

            CategoryFormModel model = new CategoryFormModel()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description

            };

            return new ServiceResult<CategoryFormModel> { Success = true, Data = model };

        }

        public async Task<ServiceResult<Category>> ConfirmEditCategory(CategoryFormModel model, int Id)
        {

            if (model == null)
            {

                return new ServiceResult<Category>
                {
                    Success = false,
                    ErrorMessage = "Invalid data!"
                };

            }


            if (string.IsNullOrEmpty(model.Name))
            {
                return new ServiceResult<Category>
                {
                    Success = false,
                    ErrorMessage = "Category name is required."
                };

            }

            var category = await _categoriesRepository.GetByIdAsync<Category>(Id);

            if (category == null)
            {
                return new ServiceResult<Category>
                {
                    Success = false,
                    ErrorMessage = "Category not found!"
                };
            }

            string name = model.Name.Trim();

            bool isExist = await _categoriesRepository.ExistByName(name.ToLowerInvariant(), Id);


            if (isExist)
            {
                return new ServiceResult<Category>
                {
                    Success = false,
                    ErrorMessage = "A category with that name already exists."
                };
            }



            try
            {
                category.Name = model.Name;
                category.Description = model.Description;
                category.UpdatedAt = DateTime.UtcNow;
                await _categoriesRepository.UpdateAsync(category);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing category with title {Title}", model.Name);

                return new ServiceResult<Category>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while edit category! Please try again later."
                };


            }

            return new ServiceResult<Category> { Success = true };

        }

        public async Task<ServiceResult<bool>> SoftDeleteCategory(int Id)
        {
            var category = await _categoriesRepository.GetByIdAsync<Category>(Id);

            if (category == null)
            {
                return new ServiceResult<bool>
                {
                    Success = false,
                    ErrorMessage = "Category not found!"
                };
            }

            if (category.IsDeleted)
            {
                return new ServiceResult<bool> 
                { 
                    Success = false,
                    ErrorMessage = "Category is already deleted." 
                };
            }


            try
            {
                category.IsDeleted = true;
                category.DeleteAt = DateTime.UtcNow;

                await _categoriesRepository.UpdateAsync(category);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error deleting category with title {Title}", category.Name);

                return new ServiceResult<bool>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while delete category! Please try again later."
                };

            }

            return new ServiceResult<bool> { Success = true };


        }

        public async Task<ServiceResult<bool>> HardDeleteCategory(int Id)
        {
            var category = await _categoriesRepository.GetDeleteOrNotCategoryAsync(Id);

            if (category == null)
            {
                return new ServiceResult<bool> 
                { 
                    Success = false,
                    ErrorMessage = "Category not found!" 
                };
            }

            
            if (category.Topics.Any())
            {
                return new ServiceResult<bool> 
                { 
                    Success = false, 
                    ErrorMessage = "Category is not empty!" 
                };
            }




            try
            {

                await _categoriesRepository.DeleteAsync(category);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error hard deleting category with title {Title}", category.Name);

                return new ServiceResult<bool>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while hard delete category! Please try again later."
                };
            }

            return new ServiceResult<bool> { Success = true };

        }

        public async Task<ServiceResult<bool>> RestoreSoftDeleteCategory(int Id)
        {
           
            
            var category = await _categoriesRepository.GetDeleteOrNotCategoryAsync(Id);

            if (category == null)
            {
                return new ServiceResult<bool> { Success = false, ErrorMessage = "Category not found!" };
            }

            try
            {
                category.IsDeleted = false;
                category.DeleteAt = null;
                category.UpdatedAt = DateTime.UtcNow;

                await _categoriesRepository.UpdateAsync(category);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error restoring category with title {Title}", category.Name);

                return new ServiceResult<bool>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while restore category! Please try again later."
                };

            }

            return new ServiceResult<bool> { Success = true };

        }
    }
}
