using Microsoft.EntityFrameworkCore;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data;
using New_Web_Library.Data.Models;
using New_Web_Library.GCommon.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Library.Data.Repository
{
    public class SystemsRepository : BaseRepository, ISystemsRepository
    {
        public SystemsRepository(LibraryDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<bool> BookTakenOrReserve(Guid bookId)
        {
            return await _dbContext.UsersBooks.AsNoTracking().AnyAsync(ub => ub.BookId == bookId &&
             (ub.Status == BookStatus.Reserved || ub.Status == BookStatus.PickedUp));
        }

        public async Task<IEnumerable<UserBook>> CheckMissingReservation(List<int> recordsId)
        {
            var reservations = await _dbContext.UsersBooks.Where(u => recordsId.Contains(u.Id)).ToListAsync();

            return reservations;
        }

        public IQueryable<UserBook> GetActiveLoans()
        {
            var usersRegister = _dbContext.UsersBooks.AsNoTracking().
               Where(ub => ub.Status == BookStatus.PickedUp || ub.Status == BookStatus.Reserved);

            return usersRegister;
        }

       
        public async Task<UserBook?> GetLoan(Guid bookId)
        {
            UserBook? isTakenBook = await _dbContext.UsersBooks.AsNoTracking()
            .FirstOrDefaultAsync(ub => ub.BookId == bookId &&
            (ub.Status == BookStatus.Reserved || ub.Status == BookStatus.PickedUp || ub.Status == BookStatus.Expired));

            return isTakenBook;

        }

        public async Task<bool> IsTakenBook(Guid bookId)
        {
            var isTaken = await _dbContext.UsersBooks.AnyAsync(ub => ub.BookId == bookId && ub.Status == BookStatus.PickedUp);

            return isTaken;
        }

        public async Task<bool> ReservedBySameUser(Guid bookId, Guid userId, int Id)
        {
            bool reservedBySameUser = await _dbContext.UsersBooks.AsNoTracking()
            .AnyAsync(ub => ub.UserId == userId && ub.BookId == bookId && ub.Status == BookStatus.Reserved && ub.Id != Id);

            return reservedBySameUser;

        }

        public async Task<UserBook?> ReturnRecord(int Id)
        {
            UserBook? foundRecord = await _dbContext.UsersBooks.Include(ub => ub.Book).Include(ub => ub.User).FirstOrDefaultAsync(ub => ub.Id == Id);

            return foundRecord;
        }

        public async Task<BookStatus?> ReturnStatus(Guid bookId)
        {
            BookStatus? bookStatus = await _dbContext.UsersBooks.AsNoTracking().Where(ub => ub.BookId == bookId)
                .OrderByDescending(ub => ub.Id).Select(ub => (BookStatus?)ub.Status).FirstOrDefaultAsync();

            return bookStatus;
        }

        public async Task<bool> TakeFromAnotherUser(Guid bookId, Guid userId,int Id)
        {
            bool takenByAnotherUser = await _dbContext.UsersBooks.AsNoTracking()
            .AnyAsync(ub => ub.BookId == bookId && ub.UserId != userId &&
            ub.Id != Id && (ub.Status == BookStatus.Reserved || ub.Status == BookStatus.PickedUp));

            return takenByAnotherUser;

        }

        public async Task<bool> UserExtraLoan( Guid userId, int Id)
        {
            var anotherBook = await _dbContext.UsersBooks.AnyAsync(ub => ub.UserId == userId &&
                ub.Id != Id && ub.Status == BookStatus.PickedUp);

            return anotherBook;
        }

        public async Task<bool> UserExtraLoan(Guid userId)
        {
            bool notReturnedBook = await _dbContext.UsersBooks.AnyAsync(ub => ub.UserId == userId && ub.Status == BookStatus.PickedUp);

            return notReturnedBook;
        }
    }
}
