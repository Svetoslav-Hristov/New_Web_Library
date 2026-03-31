using New_Web_Library.Data.Models;
using New_Web_Library.GCommon.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Library.Data.Repository.Contracts
{
    public interface ISystemRepository:IBaseRepository
    {
      
        Task<BookStatus?> ReturnStatusAsync(Guid bookId);

        Task<bool> IsTakenBookAsync(Guid bookId);

        IQueryable<UserBook> GetActiveLoans();

        Task<UserBook> GetLoanAsync(Guid bookId);

        Task<bool> BookTakenOrReserveAsync(Guid bookId);

        Task<UserBook?> ReturnRecordAsync(int Id);

        Task<bool> TakeFromAnotherUserAsync(Guid bookId, Guid userId,int Id);

        Task<bool> ReservedBySameUserAsync(Guid bookId, Guid userId, int Id);

        Task<bool> UserExtraLoanAsync( Guid userId, int Id);

        Task<bool> UserExtraLoanAsync(Guid userId);

        Task<IEnumerable<UserBook>> CheckMissingReservationAsync(List<int> recordsId );

    }
}
