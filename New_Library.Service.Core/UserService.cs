using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data;
using New_Web_Library.Data.Models;
using New_Web_Library.GCommon.Enums;
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


        public async Task<ServiceResult<UserPagingViewModel>> GetAllUsersWithOrWithoutSearchCriteriaAsync(string? search ,int page,int pageSize)
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


    }
}


