using Microsoft.EntityFrameworkCore;
using New_Web_Library.Data;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels.Book;

namespace New_Library.Services.Core
{
    public class WelcomeService : IWelcomeService
    {
        private readonly LibraryDbContext _dbContext;

        public WelcomeService(LibraryDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<IEnumerable<PreviewBookModel>> GetLatestTitlesPreviewAsync()
        {

            IEnumerable<PreviewBookModel> bookCollection = await _dbContext.Books.AsNoTracking().
               Where(b => b.CoverImageUrl != null).OrderByDescending(b => b.Id).Select(b => new PreviewBookModel()
               {

                   Id = b.Id,
                   CoverImageUrl = b.CoverImageUrl,
                   Title = b.Title

               }).Take(5).OrderBy(pm => pm.Title).ToArrayAsync();


            return bookCollection;
        }
    }
}
