using New_Library.Data.Models.Forum;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data.Models;
using New_Web_Library.Service.Core.Interfaces;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.ViewModels.Forum;
using static New_Web_Library.GCommon.EntityValidations.Topics;

namespace New_Web_Library.Service.Core
{
    public class TopicService : ITopicService
    {
        private readonly ITopicsRepository _topicsRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IUsersRepository _usersRepository;
        public TopicService(ITopicsRepository topicsRepository, ICategoriesRepository categoriesRepository
            , IUsersRepository usersRepository)
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
                CreatedOn = DateTime.UtcNow


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
                return new ServiceResult<CreateSubCategoryViewModel> { Success = false, ErrorMessage = "SubCategory not found!" };
            }

            CreateSubCategoryViewModel model = new CreateSubCategoryViewModel()
            {
                TopicName = subCategory.Title,
                CategoryId = subCategory.CategoryId,
                SubCategoryId = Id,



            };

            return new ServiceResult<CreateSubCategoryViewModel> { Success = true, Data = model };


        }

        public async Task<ServiceResult<Topic>> ConfirmEditSubCategory(CreateSubCategoryViewModel model, int Id)
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
                subCategory.UpdatedAt = DateTime.UtcNow;
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

            return new ServiceResult<Topic> { Success = true, Data = subCategory };

        }

        public async Task<ServiceResult<SubCategoryViewModel>> SubCategoryIndexPreview(int Id)
        {

            Topic? subCategory = await _topicsRepository.GetAllSubCategoryWithComments(Id);


            if (subCategory == null)
            {
                return new ServiceResult<SubCategoryViewModel> { Success = false, ErrorMessage = "SubCategory not found!" };

            }



            SubCategoryViewModel model = new SubCategoryViewModel()
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






            return new ServiceResult<SubCategoryViewModel> { Success = true, Data = model };

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
                subCategory.DeleteAt = DateTime.UtcNow;
                await _topicsRepository.UpdateAsync(subCategory);

            }
            catch (Exception)
            {
                return new ServiceResult<bool>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while delete sub category! Please try again later."
                };

            }

            return new ServiceResult<bool> { Success = true };

        }

        public async Task<ServiceResult<bool>> HardDeleteSubCategory(int Id)
        {
            var subCategory = await _topicsRepository.GetDeleteOrNotSubCategory(Id);

            if (subCategory == null)
            {
                return new ServiceResult<bool> { Success = false, ErrorMessage = "Sub Category not found!" };
            }

            if (subCategory.Posts.Any())
            {

                return new ServiceResult<bool> { Success = false, ErrorMessage = "Sub Category has posts!" };

            }


            try
            {
                await _topicsRepository.DeleteAsync(subCategory);

            }
            catch (Exception)
            {

                return new ServiceResult<bool>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while hard delete SubCategory! Please try again later."
                };

            }

            return new ServiceResult<bool> { Success = true };

        }

        public async Task<ServiceResult<bool>> RestoreSubCategory(int Id)
        {
            var subCategory = await _topicsRepository.GetDeleteOrNotSubCategory(Id);

            if (subCategory == null)
            {
                return new ServiceResult<bool> { Success = false, ErrorMessage = "Sub Category not found!" };
            }

            var category = await _categoriesRepository.GetDeleteOrNotCategory(Id);

            if (category != null)
            {
                if (!category.IsDeleted)
                {
                    return new ServiceResult<bool>
                    {
                        Success = false,
                        ErrorMessage = "You won't be able to return the SubCategory because the Category is also missing!"
                    };
                }

            }


            try
            {
                subCategory.IsDeleted = false;
                subCategory.DeleteAt = null;
                subCategory.UpdatedAt = DateTime.UtcNow;

                await _topicsRepository.UpdateAsync(subCategory);

            }
            catch (Exception)
            {
                return new ServiceResult<bool>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while restore SubCategory! Please try again later."
                };

            }

            return new ServiceResult<bool> { Success = true };


        }

        public async Task<ServiceResult<Topic>> GetOrCreateSpecialSubCategory(Guid userId)
        {
           

            var subCategory = await _topicsRepository.GetSubCategoryByName(TopicSpecialName);

            if (subCategory != null)
            {

                return new ServiceResult<Topic> { Success = true, Data = subCategory };
            }


            var lastCategory = await _categoriesRepository.LastCategory();

            var user = await _usersRepository.FindByIdAsync(userId);

            Topic special = new Topic()
            {

                Title = TopicSpecialName,
                UserId = userId,
                User = user,
                Category = lastCategory,
                CategoryId = lastCategory.Id,
                CreatedOn = DateTime.UtcNow,
            };


            try
            {
                await _topicsRepository.AddAsync(special);

            }
            catch
            {

                return new ServiceResult<Topic>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while create special SubCategory!Please try again later. "
                };

               

            }

            return new ServiceResult<Topic> { Success = true, Data = special };

        }

        
    }
}
