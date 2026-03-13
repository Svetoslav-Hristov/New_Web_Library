using New_Library.Data.Models.Forum;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data;
using Microsoft.EntityFrameworkCore;

namespace New_Library.Data.Repository
{
    public class PostsRepository : BaseRepository,IPostsRepository
    {
        

        public PostsRepository(LibraryDbContext dbContext) 
            : base(dbContext)
        {

        }

        public async Task<Dictionary<Guid, int>> GetAllCountPosts(List<Guid> usersId)
        {

            var countPosts = await _dbContext.Posts.AsNoTracking().Where(p => usersId.Contains(p.UserId) && !p.IsDeleted)
                .GroupBy(p => p.UserId).Select(p => new
                {

                    UserId = p.Key,
                    Count = p.Count(),

                }).ToDictionaryAsync(x => x.UserId, x => x.Count);

            return countPosts;

        }

        public async Task<Post?> GetByIdAsync(int Id)
        {
             var post = await _dbContext.Posts.Include(p=>p.Comments).
               Include(p=>p.Topic).ThenInclude(p=>p.User)
               .FirstOrDefaultAsync(p => p.Id == Id && !p.IsDeleted);


            return post;

        }
    
    
    
    }
}
