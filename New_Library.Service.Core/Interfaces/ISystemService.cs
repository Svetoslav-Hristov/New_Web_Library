using New_Web_Library.Data.Models;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.ViewModels.Forum;
using New_Web_Library.ViewModels.System;

namespace New_Web_Library.Services.Core.Interfaces
{
    public interface ISystemService
    {
        Task<IEnumerable<RegisterModelView>> AllUserWhoHaveActiveLoanOrReservationAsync(string? search);

        Task<CreateLoanView> CreateNewLoanAsync();

        Task<ServiceResult<UserBook>> ConfirmNewLoanAsync(CreateLoanView model);

        Task<ServiceResult<Guid>> CreateNewReservationAsync(Guid bookId ,Guid userId);

        Task<ServiceResult<CreateLoanView>> EditCurrentLoanModelAsync(int Id);

        Task<ServiceResult<CreateLoanView>> ConfirmEditLoanModelAsync(int Id,CreateLoanView model);

        Task<ServiceResult<UserBook>> DeleteLoanAsync(int Id);

        Task<ServiceResult<CreateReserveModel>> FindUserByCriteriaAsync(CreateReserveModel model);

        Task RestoreReservationModelAsync(CreateReserveModel model);

        Task<IEnumerable<DeletedItemViewModel>> GetAllDeleteItems();

        Task<ServiceResult<SubCategoryViewModel>> GetSpecialArea();

    }
        
}
