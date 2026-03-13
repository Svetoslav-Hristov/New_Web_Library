using Microsoft.EntityFrameworkCore;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data;
using New_Web_Library.Services.Core.Interfaces;
using New_Web_Library.ViewModels.Book;

namespace New_Library.Services.Core
{
    public class WelcomeService : IWelcomeService
    {
        private readonly IBooksRepository _booksRepository;

        public WelcomeService(IBooksRepository booksRepository)
        {
            this._booksRepository = booksRepository;
        }

        public async Task<IEnumerable<PreviewBookModel>> GetLatestTitlesPreviewAsync()
        {

            var allBooks = _booksRepository.GetAllBooks();

            IEnumerable<PreviewBookModel> bookCollection = await allBooks.
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
