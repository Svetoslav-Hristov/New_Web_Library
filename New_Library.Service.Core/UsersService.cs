using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data;
using New_Web_Library.Data.Models;
using New_Web_Library.GCommon.Enums;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels.User;

namespace New_Library.Services.Core
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUsersRepository _usersRepository;
        private readonly ISystemsRepository _systemsRepository;
        public UsersService(UserManager<User> userManager, SignInManager<User> signInManager,
            IUsersRepository usersRepository ,ISystemsRepository systemsRepository)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._usersRepository = usersRepository;
            this._systemsRepository = systemsRepository;
        }


        public async Task<ServiceResult<IEnumerable<User>>> GetAllUsersWithOrWithoutSearchCriteriaAsync(string? search)
        {

            IQueryable<User> allUsers = _usersRepository.GetAllUsers();



            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower().Trim();

                bool isValidAge = int.TryParse(search, out int age);

                IEnumerable<User> foundUsers = await allUsers.AsNoTracking()
                    .Where(u => !u.IsDeleted && (u.FirstName.ToLower().Contains(search) || u.LastName.ToLower()
                    .Contains(search) || (isValidAge && u.Age == age))).ToArrayAsync();

                if (!foundUsers.Any())
                {


                    return new ServiceResult<IEnumerable<User>> { Success = false, ErrorMessage = "User/s not found!", Data = foundUsers };
                }

                return new ServiceResult<IEnumerable<User>> { Success = true, Data = foundUsers };

            }


            IEnumerable<User> users = await allUsers.AsNoTracking().Where(u => !u.IsDeleted).ToArrayAsync();

            if (!users.Any())
            {

                return new ServiceResult<IEnumerable<User>> { Success = false, ErrorMessage = "There no added users in data base!", Data = users };


            }

            return new ServiceResult<IEnumerable<User>> { Success = true, Data = users };
        }

        public async Task<ServiceResult<User>> ChangeUserStatusAsync(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return new ServiceResult<User> { Success = false, ErrorMessage = "Not found!" };
            }


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
            catch (Exception)
            {

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
            if (Id == Guid.Empty)
            {

                return new ServiceResult<UserViewModel> { Success = false, ErrorMessage = "Not found !" };
            }



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
            if (Id == Guid.Empty)
            {

                return new ServiceResult<User> { Success = false, ErrorMessage = "Not found !" };

            }


            User? removedUser = await _usersRepository.FindByIdAsync(Id);

            if (removedUser == null)
            {
                return new ServiceResult<User> { Success = false, ErrorMessage = "Тhe user does not exist" };
            }



            bool notReturnedBook = await _systemsRepository.UserExtraLoan(Id);

            if (notReturnedBook || removedUser.IsBlocked)
            {
                return new ServiceResult<User> { Success = false, ErrorMessage = "The user cannot be deleted due to unspecified obligations !!" };

            }


            try
            {
                removedUser.IsDeleted = true;

                await _usersRepository.UpdateAsync(removedUser);

            }
            catch (Exception)
            {

                return new ServiceResult<User> { Success = false, ErrorMessage = "Unexpected error is occurred! Please try again later." };

            }

            return new ServiceResult<User> { Success = true };

        }

        
    }
}


