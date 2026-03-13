using New_Web_Library.Data.Models;
using New_Web_Library.GCommon.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Library.Data.Repository.Contracts
{
    public interface ISystemsRepository:IBaseRepository
    {
      
        Task<BookStatus?> ReturnStatus (Guid bookId);

        Task<bool> IsTakenBook(Guid bookId);

        IQueryable<UserBook> GetActiveLoans();

        Task<UserBook> GetLoan(Guid bookId);

        Task<bool> BookTakenOrReserve(Guid bookId);

        Task<UserBook?> ReturnRecord(int Id);

        Task<bool> TakeFromAnotherUser(Guid bookId, Guid userId,int Id);

        Task<bool> ReservedBySameUser(Guid bookId, Guid userId, int Id);

        Task<bool> UserExtraLoan( Guid userId, int Id);

        Task<bool> UserExtraLoan(Guid userId);

        Task<IEnumerable<UserBook>> CheckMissingReservation(List<int> recordsId );

    }
}
