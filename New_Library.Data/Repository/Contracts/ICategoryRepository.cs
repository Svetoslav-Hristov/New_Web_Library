using New_Library.Data.Models.Forum;

namespace New_Library.Data.Repository.Contracts
{
    public interface ICategoryRepository:IBaseRepository
    {
        Task<List<Category>> GetAllCategoriesWithSubCategoriesAsync(int? Id=null);

        IQueryable<Category> GetAllDeleteCategories();

        Task<bool> ExistByNameAsync(string name);

        Task<bool> ExistByNameAsync(string name,int Id);

        
        Task<Category?> GetDeleteOrNotCategoryAsync(int Id);

        Task<Category?> LastCategoryAsync();

    }
}
