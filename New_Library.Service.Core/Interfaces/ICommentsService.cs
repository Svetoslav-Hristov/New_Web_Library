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
    public interface ICommentsService
    {
        Task<ServiceResult<CreateContentViewModel>> CreateNewComment(int Id);
        Task<ServiceResult<Comment>> ConfirmNewComment(CreateContentViewModel model, int Id, Guid userId);
        Task<ServiceResult<CreateContentViewModel>> EditComment(int Id, Guid userId);
        Task<ServiceResult<Comment>> ConfirmEditComment(CreateContentViewModel model, int Id);
        Task<ServiceResult<Post>> SoftDeleteComment(int Id,int postId, Guid userId);
    }
}
