using Microsoft.EntityFrameworkCore;
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
    public class TopicService : ITopicService
    {
        private readonly ITopicsRepository _topicsRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        public TopicService(ITopicsRepository topicsRepository,ICategoriesRepository categoriesRepository)
        {
            this._topicsRepository = topicsRepository;
            this._categoriesRepository = categoriesRepository;
        }

        public async Task<ServiceResult<IEnumerable<SubCategoryForumModel>>> SubCategoryIndexPreview(int Id)
        {

            var categories = await _categoriesRepository.GetAllCategoriesWithSubCategories(Id);


            var posts = categories.SelectMany(c => c.Topics).SelectMany(c => c.Posts)
                 .Select(p => new SubCategoryForumModel()
                 {
                     Id = p.Id,
                     PostTitle = p.Title,
                     PostCategory = p.Topic.Category.Name,
                     PostAuthor = $"{p.User.FirstName} {p.User.LastName}",
                     CreatedOn = p.CreatedOn,
                     CommentCount = p.Comments.Count()

                 }).ToList();



            if (!posts.Any())
            {

                return new ServiceResult<IEnumerable<SubCategoryForumModel>> { Success = false, ErrorMessage = "Not found!" };
            }



            return new ServiceResult<IEnumerable<SubCategoryForumModel>> { Success = true, Data = posts };

        }
    }
}
