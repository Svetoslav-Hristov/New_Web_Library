using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data;
using New_Web_Library.Data.Models;
using New_Web_Library.GCommon.Enums;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels.System;

namespace New_Library.Services.Core
{
    using static New_Web_Library.GCommon.EntityValidations;
    using static New_Web_Library.GCommon.EntityValidations.UsersBooks;
    public class SystemsService : ISystemsService
    {

        private readonly ISystemsRepository _systemsRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IBooksRepository _booksRepository;

        public SystemsService(ISystemsRepository systemsRepository ,IUsersRepository usersRepository,
            IBooksRepository booksRepository)
        {
            this._systemsRepository = systemsRepository;
            this. _usersRepository = usersRepository;
            this._booksRepository = booksRepository;
        }


        public async Task<IEnumerable<RegisterModelView>> AllUserWhoHaveActiveLoanOrReservationAsync(string? search)
        {
            IQueryable<UserBook> activeLoans = _systemsRepository.GetActiveLoans();

            IQueryable<RegisterModelView> usersRegister = activeLoans.Select(ub => new RegisterModelView()
            {
                LoanId = ub.Id,
                UserId = ub.UserId,
                UserFirstName = ub.User.FirstName,
                UserLastName = ub.User.LastName,
                BookId = ub.BookId,
                BookTitle = ub.Book.Title,
                PickUpDate = ub.PickUpDate,
                ReturnDate = ub.ReturnDate,
                ReservedOn = ub.ReservedOn,
                ReservationExpiresOn = ub.ReservationExpiresOn,
                Status = ub.Status
            }).OrderByDescending(ub => ub.PickUpDate.HasValue);

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            await CheckingOverdueUsersAsync(today, usersRegister);

            await CheckingMissReservationAsync(today, usersRegister);


            if (search != null)
            {
                string criteria = search.Trim().ToLower();

                var foundRecords = await usersRegister.Where(ur => ur.UserFirstName.ToLower().Contains(criteria) ||
                ur.UserLastName.ToLower().Contains(criteria)).OrderBy(ur => ur.UserFirstName).ThenBy(ur => ur.UserLastName).ToArrayAsync();

                return (foundRecords);

            }

            var currentRecords = await usersRegister.OrderBy(ur => ur.UserFirstName).ThenBy(ur => ur.UserLastName).ToArrayAsync();



            return (currentRecords);



        }
        public async Task<CreateLoanView> CreateNewLoanAsync()
        {

            var (users, books) = await FillLoanDataFormAsync();


            if (!users.Any() || !books.Any())
            {
                string argument = !users.Any() ? "Users" : "Books";

                throw new InvalidOperationException($"Cannot create loan with empty {argument} collection! ");
            }


            CreateLoanView model = new CreateLoanView()
            {

                UsersList = users,

                BookList = books

            };


            return model;


        }
        public async Task<ServiceResult<UserBook>> ConfirmNewLoanAsync(CreateLoanView model)
        {



            User? foundUser = await _usersRepository.FindByIdAsync(model.UserId);

            Book? foundBook = await _booksRepository.GetByIdAsync(model.BookId);   


            if (foundUser == null || foundBook == null)
            {
                string argument = foundUser == null ? "User" : "Book";

                return new ServiceResult<UserBook> { ErrorMessage = $"Current {argument} not exist! " };


            }


            UserBook? isTakenBook = await _systemsRepository.GetLoan(model.BookId);
            
            if (isTakenBook != null)
            {
                string status = isTakenBook.Status.ToString();



                return new ServiceResult<UserBook> { Success = false, ErrorMessage = $"The book is currently {status}" };

            }


            if (foundUser.IsBlocked)
            {

                return new ServiceResult<UserBook> { Success = false, ErrorMessage = "The user is temporarily unable to rent a book due to an unclear status!" };
            }


            DateOnly loanDate = DateOnly.FromDateTime(DateTime.UtcNow);
            DateOnly returnDate = loanDate.AddDays(BorrowingExpiryPeriod);


            UserBook newLoan = new UserBook()
            {

                UserId = model.UserId,
                BookId = model.BookId,
                PickUpDate = loanDate,
                ReturnDate = returnDate,
                Status = BookStatus.PickedUp

            };

            try
            {

                await _systemsRepository.AddAsync(newLoan);

            }
            catch (Exception )
            {
                return new ServiceResult<UserBook> { Success = false, ErrorMessage = "Unexpected error is occurred please try again! " };

            }

            return new ServiceResult<UserBook> { Success = true };


        }

        public async Task<ServiceResult<CreateReserveModel>> CreateNewReservationAsync(Guid bookId)
        {
            if (bookId == Guid.Empty)
            {
                return new ServiceResult<CreateReserveModel> { Success = false, ErrorMessage = "Invalid book Id!" };
            }


            Book? book = await _booksRepository.GetByIdAsync(bookId);


            if (book == null)
            {
                return new ServiceResult<CreateReserveModel> { Success = false, ErrorMessage = "Book not found" };
            }



            CreateReserveModel model = new CreateReserveModel()
            {
                BookId = bookId,
                BookTitle = book.Title,
            };



            return new ServiceResult<CreateReserveModel> { Success = true, Data = model };

        }

        public async Task<ServiceResult<CreateReserveModel>> ConfirmNewReservationAsync(CreateReserveModel model)
        {

            if (model == null)
            {

                await RestoreReservationModelAsync(model);

                return new ServiceResult<CreateReserveModel>
                {
                    Success = false,
                    Data = model,
                    ErrorMessage = "Invalid user use the search engine and field correctly"
                };

            }


            bool foundBook = await _booksRepository.IsExistBook(model.BookId);

            bool foundUser = await _usersRepository.IsExistUser(model.UserId);

            if (!foundBook || !foundUser)
            {
                var argument = !foundBook ? "Book" : "User";

                return new ServiceResult<CreateReserveModel> { Success = false, ErrorMessage = $"Reservation is fail because {argument} missing! " };

            }



            bool takenOrReserve = await _systemsRepository.BookTakenOrReserve(model.BookId);
            

            if (takenOrReserve)
            {

                return new ServiceResult<CreateReserveModel> { Success = false, ErrorMessage = "Book is not available." };

            }




            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);
            DateOnly expiriesPeriod = today.AddDays(ReservedExpiryPeriod);

            UserBook newReservation = new UserBook()
            {

                UserId = model.UserId,
                BookId = model.BookId,
                ReservedOn = today,
                ReservationExpiresOn = expiriesPeriod,
                Status = BookStatus.Reserved



            };

            try
            {

                await _systemsRepository.AddAsync(newReservation);

            }
            catch (Exception)
            {
               

                return new ServiceResult<CreateReserveModel>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while register new reservation! Please try again later."
                };



            }

            return new ServiceResult<CreateReserveModel> { Success = true };


        }

        public async Task<ServiceResult<CreateLoanView>> EditCurrentLoanModelAsync(int Id)
        {

            if (Id <= 0)
            {
                return new ServiceResult<CreateLoanView> { Success = false, ErrorMessage = "Not found !" };
            }


            UserBook? foundRecord = await _systemsRepository.ReturnRecord(Id);

            if (foundRecord == null)
            {
                return new ServiceResult<CreateLoanView> { Success = false, ErrorMessage = "Тhere is no information about such a record !" };
            }


            var (users, books) = await FillLoanDataFormAsync();


            CreateLoanView editLoanModel = new CreateLoanView()
            {

                UserId = foundRecord.UserId,
                UsersList = users,
                BookId = foundRecord.BookId,
                BookList = books



            };

            return new ServiceResult<CreateLoanView> { Success = true, Data = editLoanModel };
            ;
        }

        public async Task<ServiceResult<CreateLoanView>> ConfirmEditLoanModelAsync(int Id, CreateLoanView model)
        {

            if (Id <= 0)
            {
                return new ServiceResult<CreateLoanView> { Success = false, ErrorMessage = "Not found !" };

            }


            
            UserBook? editRecord = await _systemsRepository.ReturnRecord(Id);

            if (editRecord == null)
            {
                return new ServiceResult<CreateLoanView> { Success = false, ErrorMessage = "Тhere is no information about such a record !" };

            }

            

            bool takenByAnotherUser = await _systemsRepository.TakeFromAnotherUser(model.BookId, model.UserId, Id);


            if (takenByAnotherUser)
            {
                return new ServiceResult<CreateLoanView> { Success = false, ErrorMessage = "This book is currently unavailable." };

            }



            bool reservedBySameUser = await _systemsRepository.ReservedBySameUser(model.BookId, model.UserId, Id);
            

            if (reservedBySameUser)
            {
                return new ServiceResult<CreateLoanView> { Success = false, ErrorMessage = "This book has already been reserved by the user." };

            }




            DateOnly pickUpDate = model.PickUpDate;

            DateOnly returnDate = pickUpDate.AddDays(BorrowingExpiryPeriod);

            try
            {
                editRecord.UserId = model.UserId;
                editRecord.BookId = model.BookId;
                editRecord.PickUpDate = pickUpDate;
                editRecord.ReturnDate = returnDate;
                editRecord.Status = BookStatus.PickedUp;

                await _systemsRepository.UpdateAsync(editRecord);

            }
            catch (Exception e)
            {
                return new ServiceResult<CreateLoanView> { Success = false, ErrorMessage = "An unexpected problem has occurred that prevents editing!" };


            }



            return new ServiceResult<CreateLoanView> { Success = true };



        }

        public async Task<ServiceResult<UserBook>> DeleteLoanAsync(int Id)
        {
            if (Id <= 0)
            {
                return new ServiceResult<UserBook> { Success = false, ErrorMessage = "Invalid record !" };

            }

            var removeLoan = await _systemsRepository.GetByIdAsync<UserBook>(Id);

            if (removeLoan == null)
            {
                return new ServiceResult<UserBook> { Success = false, ErrorMessage = "Not found !" };

            }



            try
            {
                DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);

                removeLoan.Status = BookStatus.Returned;
                removeLoan.ReturnDate = today;

                var user = await _usersRepository.FindByIdAsync(removeLoan.UserId);

                var anotherBook = await _systemsRepository.UserExtraLoan(removeLoan.UserId, removeLoan.Id);
               

                if (user != null && user.IsBlocked)
                {
                    if (!anotherBook)
                    {
                        user.IsBlocked = false;
                    }
                }

                await _systemsRepository.UpdateAsync(removeLoan);

            }
            catch (Exception)
            {

                return new ServiceResult<UserBook> { Success = false, ErrorMessage = "Unexpected error is occurred please try again!" };


            }

            return new ServiceResult<UserBook> { Success = true };
            
        }

        private async Task CheckingOverdueUsersAsync(DateOnly today, IQueryable<RegisterModelView> usersRegister)
        {

            var overdueUsers = await usersRegister.Where(ur => ur.ReturnDate.HasValue && ur.ReturnDate.Value < today &&
            ur.Status == BookStatus.PickedUp).Select(u => u.UserId).Distinct().ToListAsync();

            if (overdueUsers.Any())
            {

                var users = await _usersRepository.CheckOverdueUsers(overdueUsers);


                foreach (User user in users)
                {
                    user.IsBlocked = true;

                }

                await _usersRepository.UpdateAsync(users);
            }


        }

        private async Task CheckingMissReservationAsync(DateOnly today, IQueryable<RegisterModelView> usersRegister)
        {
            var missingReservation = await usersRegister.Where(ur => ur.ReservationExpiresOn.HasValue && ur.ReservationExpiresOn.Value < today &&
            ur.Status == BookStatus.Reserved).Select(ur => ur.LoanId).Distinct().ToListAsync();

            if (missingReservation.Any())
            {

                var reservations = await _systemsRepository.CheckMissingReservation(missingReservation);

                await _systemsRepository.DeleteAsync(reservations);

            }
        }

        public async Task<(IEnumerable<SelectListItem> users, IEnumerable<SelectListItem> books)> FillLoanDataFormAsync()
        {

            IQueryable<User> allUsers = _usersRepository.GetAllUsers();

            var users = await allUsers
           .Select(u => new SelectListItem
           {
               Text = $"{u.FirstName} {u.LastName}",
               Value = u.Id.ToString()

           }).ToListAsync();


            IQueryable<Book> allBooks = _booksRepository.GetAllBooks();

            var books = await allBooks
            .Select(b => new SelectListItem
            {

                Text = b.Title,
                Value = b.Id.ToString()

            }).ToListAsync();


            return (users, books);

        }

        public async Task RestoreReservationModelAsync(CreateReserveModel model)
        {
            var book = await _booksRepository.GetByIdAsync(model.BookId);

            if (book != null)
            {
                model.BookId = book.Id;
                model.BookTitle = book.Title;

            }
        }

        public async Task<ServiceResult<CreateReserveModel>> FindUserByCriteriaAsync(CreateReserveModel model)
        {
            if (string.IsNullOrEmpty(model.SearchingCriteria))
            {


                return new ServiceResult<CreateReserveModel>
                {
                    Success = false,
                    ErrorMessage = "User with this email or phone number was not found!",
                    Data = model
                };


            }


            string criteria = model.SearchingCriteria.Trim().ToLower();

            var foundUser = await _usersRepository.SearchByPhoneOrEmail(criteria);
            
            if (foundUser == null)
            {
                return new ServiceResult<CreateReserveModel>
                {
                    Success = false,
                    ErrorMessage = "User with this email or phone number was not found!",
                    Data = model

                };
            }

            model.SearchingCriteria = criteria;
            model.UserId = foundUser.Id;
            model.UserName = $"{foundUser.FirstName} {foundUser.LastName}";




            return new ServiceResult<CreateReserveModel> { Success = true, Data = model };


        }
    }
}
