using New_Library.Data.Models.Forum;

namespace New_Library.Data.Repository.Contracts
{
    public interface ICategoryRepository:IBaseRepository
    {
        Task<List<Category>> GetAllCategoriesWithSubCategories(int? Id=null);

        IQueryable<Category> GetAllDeleteCategories();

        Task<bool> ExistByName(string name);

        Task<bool> ExistByName(string name,int Id);

        Task<Category?> GetDeleteOrNotCategoryAsync(int Id);

        Task<Category?> LastCategory();

    }
}
