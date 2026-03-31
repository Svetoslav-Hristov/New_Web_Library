using New_Library.Data.Models.Forum;

namespace New_Library.Data.Repository.Contracts
{
    public interface ICommentRepository:IBaseRepository

    {
        Task<Comment?> GetCommentWithPostAsync(int id);

        Task<Dictionary<Guid, int>> GetAllCountCommentsAsync(List<Guid> usersId);

        IQueryable<Comment> GetAllDeleteComments();

        Task<Comment?> GetSoftDeleteCommentAsync(int Id);

        Task<int> GetAllCommentsCountAsync(Guid userId);

        Task<Comment?> GetCommentWithUserAsync (int Id);

    }
}
