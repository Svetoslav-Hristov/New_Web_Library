using New_Web_Library.ViewModels.Book;

namespace New_Web_Library.Services.Core.Interfaces
{
    public interface IWelcomeService
    {
        Task <IEnumerable<PreviewBookModel>> GetLatestTitlesPreviewAsync();
    }
}
