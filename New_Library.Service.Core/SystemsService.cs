using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using New_Web_Library.Data;
using New_Web_Library.Data.Models;
using New_Web_Library.GCommon.Enums;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels.System;

namespace New_Library.Services.Core
{
    using static New_Web_Library.GCommon.EntityValidations.UsersBooks;
    public class SystemsService : ISystemsService
    {
        private readonly LibraryDbContext _dbContext;

        public SystemsService(LibraryDbContext dbContext)
        {
            this._dbContext = dbContext;
        }


        public async Task<IEnumerable<RegisterModelView>> AllUserWhoHaveActiveLoanOrReservationAsync(string? search)
        {
            IQueryable<RegisterModelView> usersRegister = _dbContext.UsersBooks.
              Where(ub => ub.Status == BookStatus.PickedUp || ub.Status == BookStatus.Reserved)
               .Select(ub => new RegisterModelView()
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



            User? foundUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == model.UserId);

            Book? foundBook = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == model.BookId);


            if (foundUser == null || foundBook == null)
            {
                string argument = foundUser == null ? "User" : "Book";

                return new ServiceResult<UserBook> { ErrorMessage = $"Current {argument} not exist! " };


            }


            UserBook? isTakenBook = await _dbContext.UsersBooks.AsNoTracking()
            .FirstOrDefaultAsync(ub => ub.BookId == foundBook.Id &&
            (ub.Status == BookStatus.Reserved || ub.Status == BookStatus.PickedUp || ub.Status == BookStatus.Expired));

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

                await _dbContext.UsersBooks.AddAsync(newLoan);

                await _dbContext.SaveChangesAsync();


            }
            catch (Exception e)
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


            Book? book = await _dbContext.Books.FindAsync(bookId);


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


            bool foundBook = await _dbContext.Books.AsNoTracking().AnyAsync(b => b.Id == model.BookId);

            bool foundUser = await _dbContext.Users.AsNoTracking().AnyAsync(u => u.Id == model.UserId);

            if (!foundBook || !foundUser)
            {
                var argument = !foundBook ? "Book" : "User";

                return new ServiceResult<CreateReserveModel> { Success = false, ErrorMessage = $"Reservation is fail because {argument} missing! " };

            }



            bool takenOrReserve = await _dbContext.UsersBooks.AsNoTracking().AnyAsync(ub => ub.BookId == model.BookId &&
            (ub.Status == BookStatus.Reserved || ub.Status == BookStatus.PickedUp));

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

                await _dbContext.AddAsync(newReservation);

                await _dbContext.SaveChangesAsync();



            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

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


            UserBook? foundRecord = await _dbContext.UsersBooks.Include(ub => ub.Book).Include(ub => ub.User).FirstOrDefaultAsync(ub => ub.Id == Id);

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


            var editRecord = await _dbContext.UsersBooks.FirstOrDefaultAsync(ub => ub.Id == Id);

            if (editRecord == null)
            {
                return new ServiceResult<CreateLoanView> { Success = false, ErrorMessage = "Тhere is no information about such a record !" };

            }



            bool takenByAnotherUser = await _dbContext.UsersBooks.AsNoTracking()
            .AnyAsync(ub => ub.BookId == model.BookId && ub.UserId != model.UserId &&
            ub.Id != Id && (ub.Status == BookStatus.Reserved || ub.Status == BookStatus.PickedUp));



            if (takenByAnotherUser)
            {
                return new ServiceResult<CreateLoanView> { Success = false, ErrorMessage = "This book is currently unavailable." };

            }



            bool reservedBySameUser = await _dbContext.UsersBooks.AsNoTracking()
            .AnyAsync(ub => ub.UserId == model.UserId && ub.BookId == model.BookId && ub.Status == BookStatus.Reserved && ub.Id != Id);


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
                await _dbContext.SaveChangesAsync();


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

            var removeLoan = await _dbContext.UsersBooks.FirstOrDefaultAsync(ub => ub.Id == Id);

            if (removeLoan == null)
            {
                return new ServiceResult<UserBook> { Success = false, ErrorMessage = "Not found !" };

            }



            try
            {
                DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);

                removeLoan.Status = BookStatus.Returned;
                removeLoan.ReturnDate = today;

                var anotherBook = await _dbContext.UsersBooks.AnyAsync(ub => ub.UserId == removeLoan.UserId &&
                ub.Id != removeLoan.Id && ub.Status == BookStatus.PickedUp);

                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == removeLoan.UserId);

                if (user != null && user.IsBlocked)
                {
                    if (!anotherBook)
                    {
                        user.IsBlocked = false;
                    }
                }

                await _dbContext.SaveChangesAsync();

            }
            catch (Exception e)
            {

                return new ServiceResult<UserBook> { Success = false, ErrorMessage = "Unexpected error is occurred please try again!" };


            }

            return new ServiceResult<UserBook> { Success = true };
            ;
        }

        private async Task CheckingOverdueUsersAsync(DateOnly today, IQueryable<RegisterModelView> usersRegister)
        {

            var overdueUsers = await usersRegister.Where(ur => ur.ReturnDate.HasValue && ur.ReturnDate.Value < today &&
            ur.Status == BookStatus.PickedUp).Select(u => u.UserId).Distinct().ToListAsync();

            if (overdueUsers.Any())
            {

                var users = await _dbContext.Users.Where(u => overdueUsers.Contains(u.Id) && !u.IsBlocked).ToListAsync();


                foreach (User user in users)
                {
                    user.IsBlocked = true;

                }

                await _dbContext.SaveChangesAsync();
            }


        }

        private async Task CheckingMissReservationAsync(DateOnly today, IQueryable<RegisterModelView> usersRegister)
        {
            var missingReservation = await usersRegister.Where(ur => ur.ReservationExpiresOn.HasValue && ur.ReservationExpiresOn.Value < today &&
            ur.Status == BookStatus.Reserved).Select(ur => ur.LoanId).Distinct().ToListAsync();

            if (missingReservation.Any())
            {

                var reservations = await _dbContext.UsersBooks.Where(u => missingReservation.Contains(u.Id)).ToListAsync();

                _dbContext.UsersBooks.RemoveRange(reservations);


                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<(IEnumerable<SelectListItem> users, IEnumerable<SelectListItem> books)> FillLoanDataFormAsync()
        {

            var users = await _dbContext.Users.AsNoTracking()
           .Select(u => new SelectListItem
           {
               Text = $"{u.FirstName} {u.LastName}",
               Value = u.Id.ToString()

           }).ToListAsync();



            var books = await _dbContext.Books.AsNoTracking()
            .Select(b => new SelectListItem
            {

                Text = b.Title,
                Value = b.Id.ToString()

            }).ToListAsync();


            return (users, books);

        }

        public async Task RestoreReservationModelAsync(CreateReserveModel model)
        {
            var book = await _dbContext.Books.FindAsync(model.BookId);

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

            var foundUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == criteria || u.PhoneNumber.ToLower() == criteria);

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
