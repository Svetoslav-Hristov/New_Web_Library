using New_Library.Data.Models.Forum;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace New_Library.Data.Repository
{
    public class PostRepository : BaseRepository,IPostRepository
    {
        

        public PostRepository(LibraryDbContext dbContext) 
            : base(dbContext)
        {

        }

        public IQueryable<Post> AllDeletePost()
        {
            var allDelete = _dbContext.Posts.IgnoreQueryFilters().Where(p => p.IsDeleted);

            return allDelete;
           
        }

        public IQueryable<Post> CoveredPosts(List<int> coveredParentSub)
        {
            var coveredPost = _dbContext.Posts.IgnoreQueryFilters()
                .Include(p => p.Topic).Where(p => !p.IsDeleted && p.Topic != null)
                .Where(p => p.Topic.IsDeleted || coveredParentSub.Contains(p.TopicId))
                .OrderBy(p => p.TopicId);



            return coveredPost;
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

        public async Task<int> GetAllPostCount(Guid userId)
        {
            return  await _dbContext.Posts.AsNoTracking().Where(p => p.UserId == userId).CountAsync();
        }

        public async Task<Post?> GetByIdAsync(int Id)
        {
             var post = await _dbContext.Posts.Include(p=>p.User).Include(p=>p.Comments).ThenInclude(c=>c.User).
               Include(p=>p.Topic).ThenInclude(p=>p.User) 
               .FirstOrDefaultAsync(p => p.Id == Id && !p.IsDeleted);


            return post;

        }

        public async Task<Post?> GetDeleteOrNotPostAsync(int Id)
        {
            return await _dbContext.Posts.IgnoreQueryFilters().Include(p=>p.User).FirstOrDefaultAsync(p=>p.Id==Id);
        }
    }
}
