using New_Library.Data.Models.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Library.Data.Repository.Contracts
{
    public interface IPostsRepository:IBaseRepository
    {
        Task<Post?> GetByIdAsync(int id);

        Task<Dictionary<Guid, int>> GetAllCountPosts(List<Guid> usersId);

        IQueryable<Post> AllDeletePost();

        Task<Post?> GetDeleteOrNotPost(int Id);

        IQueryable<Post> CoveredPosts(List<int> coveredParentSub);

    }
}
