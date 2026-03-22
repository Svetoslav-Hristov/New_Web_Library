using New_Library.Data.Models.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Library.Data.Repository.Contracts
{
    public interface ITopicsRepository:IBaseRepository
    {
        Task<Topic?> GetAllSubCategoryWithComments(int topicId);

        Task<bool> IsExistWithSameName(string title, int topicId);

        IQueryable<Topic> GetAllDeleteSubCategories();

        Task<Topic?> GetDeleteOrNotSubCategory(int Id);

        IQueryable<Topic>? GetAllCoveredSubCategories();

        Task<Topic?> GetSubCategoryByName(string name);

    }
}
