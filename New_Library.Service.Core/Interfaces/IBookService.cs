using New_Web_Library.Data.Models;
using New_Web_Library.GCommon.Enums;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.ViewModels.Book;

namespace New_Web_Library.Services.Core.Interfaces
{
    public interface IBookService
    {
        Task<BookPagingPreview> GetAllBooksOrderedByTitleThanByAuthorAscAsync(string? search, Genre? genre, int page, int pageSize);

        Task<ServiceResult<FullPreviewModelBook>> GetCurrentModelAsync(Guid Id);


        Task<ServiceResult< BookFormModel>> GetEmptyModelBookFormWithLoadedTypesAsync();

        Task <ServiceResult<Book>> CreateNewBookUsingBookFormModelAsync(BookFormModel model);

        Task<ServiceResult<BookFormModel>> EditBookUsingBookFormModelAsync(Guid Id);

        Task<ServiceResult<Book>> ConfirmEditChangesUsingBookFormModelAsync(Guid Id, BookFormModel model);

        Task<ServiceResult<bool>> DeleteCurrentBookAsync(Guid Id);

        Task BookModelDataFillingAsync(BookFormModel model);


    }
}
