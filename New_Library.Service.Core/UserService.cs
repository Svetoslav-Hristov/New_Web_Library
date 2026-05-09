using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data.Models;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels.User;

namespace New_Library.Services.Core
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserRepository _usersRepository;
        private readonly ISystemRepository _systemsRepository;
        private readonly ILogger<IUserService> _logger;
        public UserService(UserManager<User> userManager, SignInManager<User> signInManager,
            IUserRepository usersRepository, ISystemRepository systemsRepository, ILogger<IUserService> logger)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._usersRepository = usersRepository;
            this._systemsRepository = systemsRepository;
            this._logger = logger;

        }


        public async Task<ServiceResult<UserPagingViewModel>> GetAllUsersWithOrWithoutSearchCriteriaAsync(string? search, int page, int pageSize)
        {

            IQueryable<User> allUsers = _usersRepository.GetAllUsers();



            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower().Trim();

                bool isValidAge = int.TryParse(search, out int age);

                allUsers = allUsers.AsNoTracking()
                    .Where(u => !u.IsDeleted && (u.FirstName.ToLower().Contains(search) || u.LastName.ToLower()
                    .Contains(search) || (isValidAge && u.Age == age)));

            }


            int totalCount = await allUsers.CountAsync();

            if (totalCount == 0)
            {
                var emptyModel = new UserPagingViewModel
                {
                    Users = new List<PreviewUserModel>(),
                    CurrentPage = page,
                    TotalPages = 0,
                    Search = search
                };

                return new ServiceResult<UserPagingViewModel>
                {
                    Success = false,
                    ErrorMessage = "There are no added users in database!",
                    Data = emptyModel
                };
            }


            List<PreviewUserModel>? users = await allUsers.AsNoTracking().Skip((page - 1) * pageSize).Take(pageSize).Select(u => new PreviewUserModel()
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Age = u.Age,
                Address = u.Address,
                TelephoneNumber = u.PhoneNumber,
                Email = u.Email,
                IsBlocked = u.IsBlocked


            }).ToListAsync();


            UserPagingViewModel? model = new UserPagingViewModel()
            {

                Users = users,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Search = search

            };



            return new ServiceResult<UserPagingViewModel> { Success = true, Data = model };
        }

        public async Task<ServiceResult<User>> ChangeUserStatusAsync(Guid Id)
        {

            User? blockedUser = await _usersRepository.FindByIdAsync(Id);

            if (blockedUser == null)
            {

                return new ServiceResult<User> { Success = false, ErrorMessage = "Not found!" };
            }

            if (!blockedUser.IsBlocked)
            {

                return new ServiceResult<User> { Success = false, ErrorMessage = "Тhe user is not blocked !" };


            }

            try
            {

                blockedUser.IsBlocked = false;
                await _usersRepository.UpdateAsync(blockedUser);


            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error  changing status  user Id {0} ", Id);

                return new ServiceResult<User>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while change status of this user! Please try again later."
                };

            }

            return new ServiceResult<User> { Success = true };
        }

        public async Task<ServiceResult<UserViewModel>> GetAllUserDetailsAsync(Guid Id)
        {

            var foundUser = await _usersRepository.UserFullDetailsAndHistory(Id);



            if (foundUser == null)
            {
                return new ServiceResult<UserViewModel> { Success = false, ErrorMessage = "Тhe user does not exist" };
            }



            UserViewModel model = new UserViewModel()
            {
                Id = foundUser.Id,
                FirstName = foundUser.FirstName,
                LastName = foundUser.LastName,
                Age = foundUser.Age,
                Address = foundUser.Address,
                PhoneNumber = foundUser.PhoneNumber,
                Email = foundUser.Email,
                IsBlocked = foundUser.IsBlocked,
                UserHistory = foundUser.UserBooks.Select(ub => new UserBookHistoryModel()
                {
                    BookId = ub.BookId,
                    Title = ub.Book.Title,
                    PickUpDate = ub.PickUpDate,
                    ReturnDate = ub.ReturnDate


                }).OrderByDescending(ub => ub.PickUpDate).ToArray()

            };



            return new ServiceResult<UserViewModel> { Success = true, Data = model };

        }

        public async Task<ServiceResult<User>> DeleteUserProfileAsync(Guid Id)
        {

            User? removedUser = await _usersRepository.FindByIdAsync(Id);

            if (removedUser == null)
            {
                return new ServiceResult<User> { Success = false, ErrorMessage = "Тhe user does not exist" };
            }



            bool notReturnedBook = await _systemsRepository.UserExtraLoanAsync(Id);

            if (notReturnedBook || removedUser.IsBlocked)
            {
                return new ServiceResult<User> { Success = false, ErrorMessage = "The user cannot be deleted due to unspecified obligations !!" };

            }


            try
            {
                removedUser.IsDeleted = true;

                await _usersRepository.UpdateAsync(removedUser);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with Id {}", Id);

                return new ServiceResult<User> { Success = false, ErrorMessage = "Unexpected error is occurred! Please try again later." };

            }

            return new ServiceResult<User> { Success = true };

        }

        public async Task<ServiceResult<UserFormModel>> EditUserProfileAsync(Guid Id, Guid changerId)
        {
            if (Id == Guid.Empty)
            {
                return new ServiceResult<UserFormModel> { Success = false, ErrorMessage = "Invalid user Id!" };
            }

            var user = await _usersRepository.FindByIdAsync(Id);

            if (user == null)
            {
                return new ServiceResult<UserFormModel> { Success = false, ErrorMessage = "User not found!" };
            }

            var isAdmin = await _usersRepository.AdminOrNotAsync(changerId);

            if (!isAdmin || changerId == Guid.Empty)
            {
                return new ServiceResult<UserFormModel>
                {
                    Success = false,
                    ErrorMessage = "You don't have permission to make changes on user's profile! "
                };

            }

            UserFormModel model = new UserFormModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName=user.UserName,
                Age = user.Age,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email


            };

            return new ServiceResult<UserFormModel> { Success = true, Data = model };


        }

        public async Task<ServiceResult<bool>> ConfirmEditUserProfileAsync(UserFormModel model, Guid Id, Guid changerId)
        {
            var isAdmin = await _usersRepository.AdminOrNotAsync(changerId);

            if (changerId == Guid.Empty || !isAdmin)
            {
                return new ServiceResult<bool>
                {
                    Success = false,
                    ErrorMessage = "You don't have access to make changes!"
                };
            }

            var user = await _usersRepository.FindByIdAsync(Id);

            if (user == null)
            {
                return new ServiceResult<bool>
                {
                    Success = false,
                    ErrorMessage = "User not found!"
                };
            }


            if (string.IsNullOrWhiteSpace(model.FirstName))
            {
                return new ServiceResult<bool> { Success = false, ErrorMessage = "First name is required." };
            }


            if (string.IsNullOrWhiteSpace(model.LastName))
            {
                return new ServiceResult<bool> { Success = false, ErrorMessage = "Last name is required." };
            }


            if (string.IsNullOrWhiteSpace(model.Address))
            {
                return new ServiceResult<bool> { Success = false, ErrorMessage = "Address is required." };
            }

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                return new ServiceResult<bool> { Success = false, ErrorMessage = "Email address is required." };
            }

            var isExistEmail = await _userManager.FindByEmailAsync(model.Email);

            if (isExistEmail != null && isExistEmail.Id != Id)
            {

                return new ServiceResult<bool> { Success = false, ErrorMessage = "This email belongs to another user." };
            }


            if (string.IsNullOrWhiteSpace(model.PhoneNumber))
            {
                return new ServiceResult<bool> { Success = false, ErrorMessage = "Phone number is required." };
            }


            var isExistPhoneNumber = await _usersRepository.SearchByPhoneOrEmail(model.PhoneNumber);

            if (isExistPhoneNumber != null && isExistPhoneNumber.Id != Id)
            {
                return new ServiceResult<bool> { Success = false, ErrorMessage = "This phone number belongs to another user." };

            }

            if (model.Age < 5 || model.Age > 120)
            {
                return new ServiceResult<bool> { Success = false, ErrorMessage = "Age must be in range 5-120 ." };
            }

            bool isChanged = model.FirstName.Trim() != user.FirstName || model.LastName.Trim() != user.LastName || model.Age != user.Age
                || model.PhoneNumber.Trim() != user.PhoneNumber || model.Address.Trim() != user.Address || 
                model.Email.Trim().ToLower() != user.Email.ToLower();

            try
            {
                if (isChanged)
                {
                    user.FirstName = model.FirstName.Trim();
                    user.LastName = model.LastName.Trim();
                    user.Age = model.Age;
                    user.Address = model.Address.Trim();
                    user.PhoneNumber = model.PhoneNumber.Trim();
                    user.Email = model.Email.Trim();

                    await _usersRepository.UpdateAsync(user);

                }
                else
                {
                    return new ServiceResult<bool> { Success = false, ErrorMessage = "User profile was not changed." };
                }


            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error editing user's profile with id {userId}", Id);

                return new ServiceResult<bool> 
                { 
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while edit user's profile !Please try again later ." 
                };



            }


            return new ServiceResult<bool> { Success = true };

        }
    }
}


