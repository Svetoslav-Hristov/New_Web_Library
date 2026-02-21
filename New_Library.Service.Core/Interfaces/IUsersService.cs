using New_Web_Library.Data.Models;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.ViewModels.User;

namespace New_Web_Library.Services.Core.Interfaces
{
    public interface IUsersService
    {
        Task<ServiceResult<IEnumerable<User>>> GetAllUsersWithOrWithoutSearchCriteriaAsync(string? search);

        Task<ServiceResult<User>> ChangeUserStatusAsync(Guid Id);

        ServiceResult<UserFormModel> CreateNewUserUsingFormModel();

        Task<ServiceResult<UserFormModel>> ConfirmRegistrationNewUserAsync(UserFormModel model);

        Task<ServiceResult<UserFormModel>> EditUserRegistrationAsync(Guid Id);

        Task<ServiceResult<UserFormModel>> ConfirmEditChangesAsync(Guid Id,UserFormModel model);

        Task<ServiceResult<UserViewModel>> GetAllUserDetailsAsync(Guid Id);


        Task<ServiceResult<User>> DeleteUserProfileAsync(Guid Id);

    }
}
