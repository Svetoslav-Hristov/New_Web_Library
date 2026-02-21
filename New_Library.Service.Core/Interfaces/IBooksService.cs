using New_Web_Library.Data.Models;
using New_Web_Library.GCommon.Enums;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.ViewModels.Book;

namespace New_Web_Library.Services.Core.Interfaces
{
    public interface IBooksService
    {
        Task<IEnumerable<FullPreviewModelBook>> GetAllBooksOrderedByTitleThanByAuthorAscAsync(string? search, Genre? genre);

        Task<ServiceResult<FullPreviewModelBook>> GetCurrentModelAsync(Guid Id);


        Task<ServiceResult< BookFormModel>> GetEmptyModelBookFormWithLoadedTypesAsync();

        Task <ServiceResult<Book>> CreateNewBookUsingBookFormModelAsync(BookFormModel model);

        Task<ServiceResult<BookFormModel>> EditBookUsingBookFormModelAsync(Guid Id);

        Task<ServiceResult<Book>> ConfirmEditChangesUsingBookFormModelAsync(Guid Id, BookFormModel model);

        Task<ServiceResult<bool>> DeleteCurrentBookAsync(Guid Id);

        Task BookModelDataFillingAsync(BookFormModel model);


    }
}
