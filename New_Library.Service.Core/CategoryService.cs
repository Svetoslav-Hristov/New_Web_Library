using Microsoft.EntityFrameworkCore;
using New_Library.Data.Models.Forum;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Service.Core.Interfaces;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.ViewModels.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Web_Library.Service.Core
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public CategoryService(ICategoriesRepository categoriesRepository)
        {
            this._categoriesRepository = categoriesRepository;
            
        }


        public async Task<IEnumerable<IndexForumModel>> IndexPreview()
        {

           List<Category> allCategories =await _categoriesRepository.GetAllCategoriesWithSubCategories();



            IEnumerable<IndexForumModel> categories = allCategories.Select(c => new IndexForumModel()
             {
                 Id = c.Id,
                 Name = c.Name,
                 Description = c.Description,
                 Topics = c.Topics.Select(t => new TopicForumModel()
                 {

                     Id = t.Id,
                     Title = t.Title
                 }).ToArray(),
                 PostCount = c.Topics.SelectMany(t => t.Posts).Count(),
                 LastPostTitle = c.Topics.SelectMany(t => t.Posts).OrderByDescending(p => p.CreatedOn)
                .Select(p => p.Title).FirstOrDefault()


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

             string name = model.Name.Trim().ToLowerInvariant();

            bool isExist = await _categoriesRepository.ExistByName(name);

            if (isExist)
            {
                return new ServiceResult<Category> { Success = false, ErrorMessage = "A category with that name already exists." };
            }


            Category newCategory = new Category()
            {

                Name = model.Name,
                Description = model.Description,
            };

            try
            {
                await _categoriesRepository.AddAsync(newCategory);           
              

            }
            catch (Exception)
            {
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
                Id=category.Id,
                Name = category.Name,
                Description = category.Description

            };

            return new ServiceResult<CategoryFormModel> { Success = true, Data = model };

        }

        public async Task<ServiceResult<Category>> ConfirmEditCategory(CategoryFormModel model ,int Id)
        {
            var category = await _categoriesRepository.GetByIdAsync<Category>(Id);
           
            if (category == null)
            {
                return new ServiceResult<Category> { Success = false, ErrorMessage = "Category not found!" };
            }

            string name = category.Name.Trim().ToLowerInvariant();

            bool isExist = await _categoriesRepository.ExistByName(name, Id);


            if (isExist)
            {
                return new ServiceResult<Category> { Success = false, ErrorMessage = "A category with that name already exists." };
            }



            try
            {
                category.Name = model.Name;
                category.Description = model.Description;

                await _categoriesRepository.UpdateAsync(category);

            }
            catch(Exception)
            {

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
                return new ServiceResult<bool> { Success = false, ErrorMessage = "Category not found" };
            }

            try
            {
                category.IsDeleted = true;

                await _categoriesRepository.UpdateAsync(category);

            }
            catch (Exception)
            {

                return new ServiceResult<bool> 
                { 
                    Success = false,
                    ErrorMessage= "Unexpected error is occurred while delete category! Please try again later." 
                };

            }

            return new ServiceResult<bool> { Success = true };

          
        }
    }
}
