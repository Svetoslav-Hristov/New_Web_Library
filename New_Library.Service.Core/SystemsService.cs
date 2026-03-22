using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using New_Library.Data.Models.Forum;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data.Models;
using New_Web_Library.GCommon.Enums;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels.Forum;
using New_Web_Library.ViewModels.System;
using static New_Web_Library.GCommon.EntityValidations.Topics;

namespace New_Library.Services.Core
{
    using static New_Web_Library.GCommon.EntityValidations.UsersBooks;
    public class SystemsService : ISystemsService
    {

        private readonly ISystemsRepository _systemsRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IBooksRepository _booksRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly ITopicsRepository _topicsRepository;
        private readonly IPostsRepository _postsRepository;
        private readonly ICommentsRepository _commentsRepository;

        public SystemsService(ISystemsRepository systemsRepository, IUsersRepository usersRepository,
            IBooksRepository booksRepository, ICategoriesRepository categoriesRepository,
            ITopicsRepository topicsRepository, IPostsRepository postsRepository, ICommentsRepository commentsRepository)
        {
            this._systemsRepository = systemsRepository;
            this._usersRepository = usersRepository;
            this._booksRepository = booksRepository;
            this._categoriesRepository = categoriesRepository;
            this._topicsRepository = topicsRepository;
            this._postsRepository = postsRepository;
            this._commentsRepository = commentsRepository;
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
            catch (Exception)
            {
                return new ServiceResult<UserBook> { Success = false, ErrorMessage = "Unexpected error is occurred please try again! " };

            }

            return new ServiceResult<UserBook> { Success = true };


        }

        public async Task<ServiceResult<Guid>> CreateNewReservationAsync(Guid bookId,Guid userId)
        {
            if (bookId == Guid.Empty)
            {
                return new ServiceResult<Guid> { Success = false, ErrorMessage = "Invalid book Id!" };
            }


            bool foundBook = await _booksRepository.IsExistBook(bookId);


            if (foundBook == null)
            {
                return new ServiceResult<Guid> { Success = false, ErrorMessage = "Book not found" };
            }

            bool foundUser = await _usersRepository.IsExistUser(userId);

            
            if (!foundBook || !foundUser)
            {
                var argument = !foundBook ? "Book" : "User";

                return new ServiceResult<Guid> { Success = false, ErrorMessage = $"Reservation is fail because {argument} missing! " };

            }

            bool takenOrReserve = await _systemsRepository.BookTakenOrReserve(bookId);


            if (takenOrReserve)
            {

                return new ServiceResult<Guid> { Success = false, ErrorMessage = "Book is not available." };

            }




            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);
            DateOnly expiriesPeriod = today.AddDays(ReservedExpiryPeriod);
            UserBook newReservation = new UserBook()
            {

                UserId = userId,
                BookId = bookId,
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


                return new ServiceResult<Guid>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while register new reservation! Please try again later."
                };



            }

            return new ServiceResult<Guid> { Success = true ,Data=bookId};




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

                await _usersRepository.UpdateRangeAsync(users);
            }


        }

        private async Task CheckingMissReservationAsync(DateOnly today, IQueryable<RegisterModelView> usersRegister)
        {
            var missingReservation = await usersRegister.Where(ur => ur.ReservationExpiresOn.HasValue && ur.ReservationExpiresOn.Value < today &&
            ur.Status == BookStatus.Reserved).Select(ur => ur.LoanId).Distinct().ToListAsync();

            if (missingReservation.Any())
            {

                var reservations = await _systemsRepository.CheckMissingReservation(missingReservation);

                foreach (var reservation in reservations)
                {

                    reservation.Status = BookStatus.Expired;
                    reservation.ReservationExpiresOn = today;


                }


                await _systemsRepository.UpdateRangeAsync(reservations);

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

        public async Task<IEnumerable<DeletedItemViewModel>> GetAllDeleteItems()
        {

            List<DeletedItemViewModel> deleteItems = new List<DeletedItemViewModel>();

            var allDeleteCategories = _categoriesRepository.GetAllDeleteCategories();

            var categories =await allDeleteCategories
                  .Select(c => new DeletedItemViewModel
                  {
                      Id = c.Id,
                      Name = c.Name,
                      Type = "Category",
                      Description = c.Description,
                      DeleteAt=c.DeleteAt,
                      

                  })
                  .ToListAsync();

            var allDeleteSubCategories = _topicsRepository.GetAllDeleteSubCategories();

            var subCategories =await allDeleteSubCategories
                   .Select(t => new DeletedItemViewModel
                   {
                       Id = t.Id,
                       Name = t.Title,
                       Type = "SubCategory",
                       Description = null,
                       DeleteAt=t.DeleteAt,
                       ParentId = t.CategoryId,
                       ParentName = t.Category.Name
                   })
                   .ToListAsync();


            var allCoveredSubCategories =await _topicsRepository.GetAllCoveredSubCategories().ToListAsync();

            var coverSubCategories =  allCoveredSubCategories
                   .Select(t => new DeletedItemViewModel
                   {
                       Id = t.Id,
                       Name = t.Title,
                       Type = "Covered/SubCategory",
                       Description = null,
                       ParentId = t.CategoryId,
                       ParentName = t.Category?.Name ?? "Deleted category"
                   })
                   .ToList();




            var allDeletePosts = _postsRepository.AllDeletePost();


            var posts =await  allDeletePosts
            .Select(p => new DeletedItemViewModel
            {
                Id = p.Id,
                Name = p.Title,
                Type = "Post",
                Description = p.Content,
                DeleteAt=p.DeleteAt,
                ParentId = p.TopicId,
                ParentName = p.Topic.Title
            })
            .ToListAsync();

            List<int> coveredParentSub =  allCoveredSubCategories.Select(s => s.Id).Distinct().ToList();

            var allCoveredPost = _postsRepository.CoveredPosts(coveredParentSub);

            var coveredPosts = await allCoveredPost
            .Select(p => new DeletedItemViewModel
            {
                Id = p.Id,
                Name = p.Title,
                Type = "Covered/Post",
                Description = p.Content,
                DeleteAt = p.DeleteAt,
                ParentId = p.TopicId,
                ParentName = p.Topic.Title
            })
            .ToListAsync();

           




            IQueryable<Comment> allDeleteComments = _commentsRepository.GetAllDeleteComments();

            var comments = await allDeleteComments
                .Select(c => new DeletedItemViewModel
                {
                    Id = c.Id,
                    Name = "Comment",
                    Type = "Comment",
                    Description = c.Content,
                    DeleteAt=c.DeleteAt,
                    ParentId = c.PostId,
                    ParentName = c.Post.Title
                })
                .ToListAsync();


            deleteItems.AddRange(categories);
            deleteItems.AddRange(subCategories);
            deleteItems.AddRange(coverSubCategories);
            deleteItems.AddRange(posts);
            deleteItems.AddRange(coveredPosts);
            deleteItems.AddRange(comments);


            return deleteItems;

        }

        public async Task<ServiceResult<SubCategoryViewModel>> GetSpecialArea()
        {
            var specialSubCategory = await _topicsRepository.GetSubCategoryByName(TopicSpecialName);

            if (specialSubCategory == null)
            {
                return new ServiceResult<SubCategoryViewModel> { Success = false, ErrorMessage = "SubCategory is not created or found!" };
            }

            SubCategoryViewModel model = new SubCategoryViewModel()
            {
                CategoryId = specialSubCategory.Id,
                CategoryName = specialSubCategory.Title,
                Posts = specialSubCategory.Posts.Select(p => new SubCategoryForumModel()
                {
                    Id = p.Id,
                    PostTitle = p.Title,
                    PostAuthor = $"{p.User.FirstName} {p.User.LastName}",
                    CreatedOn = p.CreatedOn,
                    CommentCount = p.Comments.Count(),

                }).ToList()


            };


            return new ServiceResult<SubCategoryViewModel> { Success = true, Data = model };
            
        }
    }
}
