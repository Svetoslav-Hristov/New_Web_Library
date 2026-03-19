using New_Library.Data.Models.Forum;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data;
using Microsoft.EntityFrameworkCore;

namespace New_Library.Data.Repository
{
    public class CommentsRepository : BaseRepository, ICommentsRepository
    {
        public CommentsRepository(LibraryDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<Dictionary<Guid, int>> GetAllCountComments(List<Guid> usersId)
        {

            var countComments = await _dbContext.Comments.AsNoTracking().Where(c => usersId.Contains(c.UserId) && !c.IsDeleted)
                .GroupBy(c => c.UserId).Select(c => new
                {
                    UserId = c.Key,
                    Count = c.Count()

                }).ToDictionaryAsync(x => x.UserId, x => x.Count);

            return countComments;
        }

        public IQueryable<Comment> GetAllDeleteComments()
        {
            IQueryable<Comment> allDelete = _dbContext.Comments.IgnoreQueryFilters().Where(c => c.IsDeleted);


            return allDelete;
            
        }

        public async Task<Comment?> GetCommentWithPostAsync(int id)
        {
            return await  _dbContext.Comments.Include(c => c.Post)
              .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Comment?> GetDeleteComment(int Id)
        {
            return await _dbContext.Comments.IgnoreQueryFilters().Where(c => c.Id == Id).FirstOrDefaultAsync();
           
        }
    }
}
