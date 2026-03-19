using Microsoft.EntityFrameworkCore;
using New_Library.Data.Models.Forum;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data.Models;
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
    public class TopicService : ITopicService
    {
        private readonly ITopicsRepository _topicsRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IUsersRepository _usersRepository;
        public TopicService(ITopicsRepository topicsRepository,ICategoriesRepository categoriesRepository
            ,IUsersRepository usersRepository)
        {
            this._topicsRepository = topicsRepository;
            this._categoriesRepository = categoriesRepository;
            this._usersRepository = usersRepository;
        }
        public async Task<ServiceResult<CreateSubCategoryViewModel>> CreateNewSubCategory(int Id)
        {
            var category = await _categoriesRepository.GetByIdAsync<Category>(Id);

            if (category == null)
            {
                return new ServiceResult<CreateSubCategoryViewModel>
                {
                    Success = false,
                    ErrorMessage = "Category not found!"
                };
            }

            CreateSubCategoryViewModel model = new CreateSubCategoryViewModel()
            {
                CategoryId = category.Id
            };

            return new ServiceResult<CreateSubCategoryViewModel>
            {
                Success = true,
                Data = model
            };

        }
        public async Task<ServiceResult<Topic>> ConfirmCreationNewSubcategory(CreateSubCategoryViewModel model, Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return new ServiceResult<Topic> { Success = false, ErrorMessage = "Invalid user Id!" };
            }

            User? user = await _usersRepository.FindByIdAsync(userId);

            if (user == null)
            {
                return new ServiceResult<Topic> { Success = false, ErrorMessage = "User not found!" };
            }


            Topic newTopic = new Topic()
            {
                Title = model.TopicName,
                UserId = user.Id,
                CategoryId = model.CategoryId,
                CreatedOn=DateTime.UtcNow


            };

            try
            {
                await _topicsRepository.AddAsync(newTopic);

            }
            catch (Exception)
            {

                return new ServiceResult<Topic>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while create new sub-category! Please try again later."
                };

            }

            return new ServiceResult<Topic> { Success = true };


           
        }

        public async Task<ServiceResult<CreateSubCategoryViewModel>> EditSubCategory(int Id)
        {
            var subCategory = await _topicsRepository.GetByIdAsync<Topic>(Id);

            if (subCategory == null)
            {
                return new ServiceResult<CreateSubCategoryViewModel> { Success = false, ErrorMessage = "Sub category not found!" };
            }

            CreateSubCategoryViewModel model = new CreateSubCategoryViewModel()
            {
                TopicName = subCategory.Title,
                CategoryId = subCategory.CategoryId,
                SubCategoryId=Id,
               


            };

            return new ServiceResult<CreateSubCategoryViewModel> { Success = true, Data = model };


        }

        public async Task<ServiceResult<Topic>> ConfirmEditSubCategory(CreateSubCategoryViewModel model ,int Id)
        {

            Topic? subCategory = await _topicsRepository.GetByIdAsync<Topic>(Id);

            if (subCategory == null)
            {
                return new ServiceResult<Topic> { Success = false, ErrorMessage = "Invalid Sub Category!" };
            }

            
            if (string.IsNullOrEmpty(model.TopicName))
            {
                return new ServiceResult<Topic> { Success = false, ErrorMessage = "Invalid data!" };
            }

            try
            {
                subCategory.Title = model.TopicName;


                await _topicsRepository.UpdateAsync(subCategory);

            }
            catch (Exception)
            {
                return new ServiceResult<Topic> 
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while edit sub-category! Please try again later." 
                };

            }

            return new ServiceResult<Topic> { Success = true ,Data=subCategory };

        }

        public async Task<ServiceResult<SubCategoryViewModel>> SubCategoryIndexPreview(int Id)
        {

            List<Topic> subCategories = await _topicsRepository.GetAllSubCategoryWithComments(Id);
 
            Topic? subCategory = subCategories.FirstOrDefault(c => c.Id == Id); 
           
            if (subCategory == null)
            {
                return new ServiceResult<SubCategoryViewModel> { Success = false, ErrorMessage = "SubCategory not found!" };

            }



            SubCategoryViewModel model =  new SubCategoryViewModel()
            {

                CategoryName = subCategory.Title,
                CategoryId = subCategory.Id,
                Posts = subCategory.Posts.Select(p => new SubCategoryForumModel()
                {
                    Id = p.Id,
                    PostTitle = p.Title,
                    PostAuthor = $"{p.User.FirstName} {p.User.LastName}",
                    CreatedOn = p.CreatedOn,
                    CommentCount = p.Comments.Count(),

                }).ToList()

            };




            if (subCategory==null)
            {

                return new ServiceResult<SubCategoryViewModel> { Success = false, ErrorMessage = "Not found!" };
            }



            return new ServiceResult<SubCategoryViewModel> { Success = true, Data = model};

        }

        public async Task<ServiceResult<bool>> SoftDeleteSubCategory(int Id)
        {
            var subCategory = await _topicsRepository.GetByIdAsync<Topic>(Id);

            if (subCategory == null)
            {
                return new ServiceResult<bool> { Success = false, ErrorMessage = "SubCategory not exist!" };
            }

            try
            {
                subCategory.IsDeleted = true;
                await _topicsRepository.UpdateAsync(subCategory);

            }
            catch (Exception)
            {
                return new ServiceResult<bool> 
                {
                    Success=false,
                    ErrorMessage= "Unexpected error is occurred while delete sub category! Please try again later."
                };                

            }

            return new ServiceResult<bool> { Success = true };
           
        }
    }
}
