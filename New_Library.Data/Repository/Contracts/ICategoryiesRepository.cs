using New_Library.Data.Models.Forum;

namespace New_Library.Data.Repository.Contracts
{
    public interface ICategoriesRepository:IBaseRepository
    {
        Task<List<Category>> GetAllCategoriesWithSubCategories(int? Id=null);

    }
}
