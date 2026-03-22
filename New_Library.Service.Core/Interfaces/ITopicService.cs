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
    public interface ITopicService
    {
        Task<ServiceResult<SubCategoryViewModel>> SubCategoryIndexPreview(int Id);

        Task<ServiceResult<CreateSubCategoryViewModel>> CreateNewSubCategory(int Id);

        Task<ServiceResult<Topic>> ConfirmCreationNewSubcategory(CreateSubCategoryViewModel model,Guid userId);

        Task<ServiceResult<CreateSubCategoryViewModel>> EditSubCategory(int Id);

        Task<ServiceResult<Topic>> ConfirmEditSubCategory(CreateSubCategoryViewModel model,int Id);

        Task<ServiceResult<bool>> SoftDeleteSubCategory(int Id);

        Task<ServiceResult<bool>> HardDeleteSubCategory(int Id);

        Task<ServiceResult<bool>> RestoreSubCategory(int Id);

        Task<ServiceResult<Topic>> GetOrCreateSpecialSubCategory(Guid userId);



    }
}
