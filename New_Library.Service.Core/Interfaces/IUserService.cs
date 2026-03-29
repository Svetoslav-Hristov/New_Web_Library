using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using New_Web_Library.Data.Models;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.ViewModels.User;

namespace New_Web_Library.Services.Core.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<UserPagingViewModel>> GetAllUsersWithOrWithoutSearchCriteriaAsync(string? search, int page, int pageSize);

        Task<ServiceResult<User>> ChangeUserStatusAsync(Guid Id);

        Task<ServiceResult<UserViewModel>> GetAllUserDetailsAsync(Guid Id);

        Task<ServiceResult<User>> DeleteUserProfileAsync(Guid Id);

        

    }
}
