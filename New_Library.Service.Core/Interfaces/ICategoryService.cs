using New_Library.Data.Models.Forum;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.ViewModels.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Web_Library.Service.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<IndexForumModel>> IndexPreview();

        ServiceResult<CategoryFormModel> CreateNewCategory();

        Task<ServiceResult<Category>> ConfirmNewCategory(CategoryFormModel model);

        Task<ServiceResult<CategoryFormModel>> EditCategory(int Id);

        Task<ServiceResult<Category>> ConfirmEditCategory(CategoryFormModel model, int Id);

        Task<ServiceResult<bool>> SoftDeleteCategory(int Id);

        Task<ServiceResult<bool>> HardDeleteCategory(int Id);

        Task<ServiceResult<bool>> RestoreSoftDeleteCategory(int Id);

    }
}
