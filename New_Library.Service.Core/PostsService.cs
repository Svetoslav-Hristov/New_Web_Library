using New_Library.Data.Models.Forum;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Service.Core.Interfaces;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.ViewModels.Forum;
using static New_Web_Library.GCommon.EntityValidations.Posts;

namespace New_Web_Library.Service.Core
{
    public class PostsService : IPostsService
    {

        private readonly IPostsRepository _postsRepository;
        private readonly ICommentsRepository _commentsRepository;

        public PostsService(IPostsRepository postsRepository ,ICommentsRepository commentsRepository)
        {
            this._postsRepository = postsRepository;
            this._commentsRepository = commentsRepository;
        }


        public async Task<ServiceResult<PostForumModel>> PostDetailModelsPreview(int Id, Guid? userId)
        {

            Post? post = await _postsRepository.GetByIdAsync(Id);


            if (post == null)
            {
                return new ServiceResult<PostForumModel> { Success = false, ErrorMessage = "Not Found!" };
            }


            PostForumModel model = new PostForumModel()
            {
                Id = post.Id,
                Title =post.Title,
                Content = post.Content,
                CreatedOn = post.CreatedOn,
                AuthorName = $"{post.User.FirstName} {post.User.LastName}",
                UserId = post.UserId,
                Comments = post.Comments.Where(c => !c.IsDeleted).Select(c => new ContentDetailsModel()
                {
                    Id = c.Id,
                    Title = $"Re:{post.Title}",
                    Content = c.Content,
                    CreatedOn = c.CreatedOn,
                    AuthorName = $"{c.User.FirstName} {c.User.LastName}",
                    UserId = c.UserId,

                }).ToArray()

            };


            

            var usersId = post.Comments.Select(p => p.UserId).Append(post.UserId).Distinct().ToList();





            var countComments = await _commentsRepository.GetAllCountComments(usersId);


            var countPosts = await _postsRepository.GetAllCountPosts(usersId);
                

            foreach (var user in model.Comments)
            {
                
                user.UserCommentCount = countComments.GetValueOrDefault(user.UserId);
                user.UserPostCount = countPosts.GetValueOrDefault(user.UserId);
            }


            model.UserPostCount = countPosts.GetValueOrDefault(post.UserId);
            model.UserCommentCount = countComments.GetValueOrDefault(post.UserId);


            if (userId != null)
            {

                if (!model.Comments.Any() && model.UserId == userId)
                {
                    model.IsAuthor = true;

                }
                else
                {
                    var lastComment = model.Comments.OrderByDescending(p => p.CreatedOn).FirstOrDefault();


                    if (lastComment?.UserId == userId && DateTime.UtcNow - lastComment.CreatedOn < TimeSpan.FromMinutes(CommentLifeTime))
                    {
                        lastComment.IsAuthor = true;
                    }

                }
            }

            return new ServiceResult<PostForumModel> { Success = true, Data = model };

        }
    }
}
