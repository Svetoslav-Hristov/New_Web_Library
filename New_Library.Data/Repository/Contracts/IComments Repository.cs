using New_Library.Data.Models.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Library.Data.Repository.Contracts
{
    public interface ICommentsRepository:IBaseRepository

    {
        Task<Comment?> GetCommentWithPostAsync(int id);

        Task<Dictionary<Guid, int>> GetAllCountComments(List<Guid> usersId);

    }
}
