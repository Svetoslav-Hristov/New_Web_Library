using New_Library.Data.Models.Forum;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.ViewModels.Forum;

namespace New_Web_Library.Service.Core.Interfaces
{
    public interface IPostsService
    {
        Task<ServiceResult<PostForumModel>> PostDetailModelsPreview(int Id, Guid? userId);

        Task<ServiceResult<CreateContentViewModel>> CreateNewPost(int categoryId);

        Task<ServiceResult<int>> ConfirmNewPost(CreateContentViewModel model,Guid userId ,int categoryId);

        Task<ServiceResult<CreateContentViewModel>> EditPost(int Id);

        Task<ServiceResult<Post>> ConfirmEditPost(CreateContentViewModel model, Guid userId,int Id);

        Task<ServiceResult<int>> SoftDeletePost(int Id,Guid userId);

        Task<ServiceResult<bool>> RestoreDeletePost(int Id);

        Task<ServiceResult<bool>> HardDeletePost(int Id);

    }
}
