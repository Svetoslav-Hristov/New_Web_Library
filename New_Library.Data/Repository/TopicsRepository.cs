using Microsoft.EntityFrameworkCore;
using New_Library.Data.Models.Forum;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Library.Data.Repository
{
    public class TopicsRepository : BaseRepository, ITopicsRepository
    {
        public TopicsRepository(LibraryDbContext dbContext) 
            : base(dbContext)
        {
        }

       

        public  IQueryable<Topic>? GetAllCoveredSubCategories()
        {
            var subCategories =  _dbContext.Topics.IgnoreQueryFilters().Include(t=>t.Category)
                .Where(t => !t.IsDeleted && t.Category.IsDeleted)
                .OrderBy(t => t.CategoryId);


            return subCategories;
        }

        public IQueryable<Topic> GetAllDeleteSubCategories()
        {
            var deleteSubCategories = _dbContext.Topics.IgnoreQueryFilters().Where(c => c.IsDeleted);

            return deleteSubCategories;
        }

        public async Task<Topic?> GetAllSubCategoryWithComments(int topicId)
        {
            var topic =await  _dbContext.Topics.Where(t => t.Id == topicId)
            .Include(t => t.User).Include(t => t.Posts).ThenInclude(p=>p.User).Include(t=>t.Posts).ThenInclude(p=>p.Comments)
            .ThenInclude(c => c.User)
            .Include(t => t.Category).FirstOrDefaultAsync();


            return topic;
        }

        public async Task<Topic?> GetDeleteOrNotSubCategory(int Id)
        {
            return await _dbContext.Topics.IgnoreQueryFilters().Include(c=>c.Category).Include(c=>c.User)
                .Include(t=>t.Posts).Where(t => t.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<Topic?> GetSubCategoryByName(string name)
        {
            var subCategory = await _dbContext.Topics.Where(t => t.Title == name)
            .Include(t => t.User).Include(t => t.Posts).ThenInclude(p => p.User).Include(t => t.Posts).ThenInclude(p => p.Comments)
            .ThenInclude(c => c.User)
            .Include(t => t.Category).FirstOrDefaultAsync();

            return subCategory;
          
        }

        public async Task<bool> IsExistWithSameName(string title, int topicId)
        {
            return await _dbContext.Topics.AnyAsync(t => t.Title == title && t.Id != topicId);
        }
    }
}
