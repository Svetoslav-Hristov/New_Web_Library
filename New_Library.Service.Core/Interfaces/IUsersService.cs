using New_Web_Library.Data.Models;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.ViewModels.User;

namespace New_Web_Library.Services.Core.Interfaces
{
    public interface IUsersService
    {
        Task<ServiceResult<IEnumerable<User>>> GetAllUsersWithOrWithoutSearchCriteriaAsync(string? search);

        Task<ServiceResult<User>> ChangeUserStatusAsync(Guid Id);

        Task<ServiceResult<UserViewModel>> GetAllUserDetailsAsync(Guid Id);


        Task<ServiceResult<User>> DeleteUserProfileAsync(Guid Id);

    }
}
