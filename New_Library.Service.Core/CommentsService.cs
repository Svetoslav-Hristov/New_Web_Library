using Microsoft.EntityFrameworkCore;
using New_Library.Data.Models.Forum;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data.Models;
using New_Web_Library.Service.Core.Interfaces;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.ViewModels.Forum;

namespace New_Web_Library.Service.Core
{
    public class CommentsService : ICommentsService
    {

        private readonly ICommentsRepository _commentRepository;
        private readonly IPostsRepository _postRepository;
        private readonly IUsersRepository _usersRepository;

        public CommentsService(ICommentsRepository commentRepository,IPostsRepository postRepository ,
            IUsersRepository usersRepository)
            
        {
            this._commentRepository = commentRepository;
            this._postRepository = postRepository;
            this._usersRepository = usersRepository;
        }



        public async Task<ServiceResult<CreateContentViewModel>> CreateNewComment(int Id)
        {
            var post = await _postRepository.GetByIdAsync(Id);

            if (post == null)
            {
                return new ServiceResult<CreateContentViewModel> { Success = false, ErrorMessage = "Post not found!" };

            }



            string commentTitle = $"Re:{post.Title}";
            CreateContentViewModel model = new CreateContentViewModel()
            {
                Title = commentTitle,
                PostId = post.Id

            };


            return new ServiceResult<CreateContentViewModel> { Success = true, Data = model };

        }


        public async Task<ServiceResult<Comment>> ConfirmNewComment(CreateContentViewModel model, int Id, Guid userId)
        {
            if (model == null)
            {
                return new ServiceResult<Comment> { Success = false, ErrorMessage = "Invalid data!" };
            }

            var post = await _postRepository.GetByIdAsync(Id);

            if (post == null)
            {

                return new ServiceResult<Comment> { Success = false, ErrorMessage = "Post not found!" };

            }

            var user = await _usersRepository.FindByIdAsync(userId);

            if (user == null)
            {

                return new ServiceResult<Comment> { Success = false, ErrorMessage = "User not found!" };

            }





            Comment newComment = new Comment()
            {

                CreatedOn = DateTime.UtcNow,
                Content = model.Description,
                UserId = userId,
                PostId = Id

            };

            try
            {
                await _commentRepository.AddAsync(newComment);
                
            }
            catch (Exception)
            {

                return new ServiceResult<Comment>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while create new comment! Please try again later."
                };

            }

            return new ServiceResult<Comment> { Success = true };

        }

        public async Task<ServiceResult<CreateContentViewModel>> EditComment(int Id, Guid userId)
        {
            var comment = await _commentRepository.GetCommentWithPostAsync(Id);                    

            if (comment == null)
            {

                return new ServiceResult<CreateContentViewModel> { Success = false, ErrorMessage = "Comment not found !" };

            }

            if (comment.UserId != userId)
            {
                return new ServiceResult<CreateContentViewModel> { Success = false, ErrorMessage = "Invalid request." };

            }

            CreateContentViewModel model = new CreateContentViewModel()
            {
                Title = $"Re:{comment.Post.Title}",
                Description = comment.Content,
                UserId = userId,
                PostId = comment.PostId
            };

            return new ServiceResult<CreateContentViewModel> { Success = true, Data = model };

        }

        public async Task<ServiceResult<Comment>> ConfirmEditComment(CreateContentViewModel model, int Id)
        {
            if (model == null)
            {
                return new ServiceResult<Comment> { Success = false, ErrorMessage = "Invalid data!" };
            }

            Comment? comment = await _commentRepository.GetByIdAsync<Comment>(Id);



            if (comment == null)
            {
                return new ServiceResult<Comment> { Success = false, ErrorMessage = "Comment not found!" };
            }

            try
            {

                comment.Content = model.Description;
                await _commentRepository.UpdateAsync(comment);

            }
            catch (Exception)
            {
                return new ServiceResult<Comment>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while edit  comment! Please try again later."
                };

            }

            return new ServiceResult<Comment> { Success = true, Data = comment };

        }

        public async Task<ServiceResult<Post>> SoftDeleteComment(int Id,int postId, Guid userId)
        {
            var comment = await _commentRepository.GetByIdAsync<Comment>(Id);      



            if (comment == null)
            {
                return new ServiceResult<Post> { Success = false, ErrorMessage = "Comment not found!" };
            }

            if (userId == Guid.Empty)
            {
                return new ServiceResult<Post> { Success = false, ErrorMessage = "Not found!" };
            }

            var user = await _usersRepository.FindByIdAsync(userId);          

            if (user == null || user.Id != comment.UserId)
            {

                return new ServiceResult<Post> { Success = false, ErrorMessage = "Invalid data!" };

            }

            Post? post = await _postRepository.GetByIdAsync(postId);
           
            try
            {

                comment.IsDeleted = true;

                await _commentRepository.UpdateAsync(comment);

            }
            catch (Exception)
            {

                return new ServiceResult<Post>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred ! Please try again later."
                };


            }


            return new ServiceResult<Post> { Success = true, Data = post };

        }

    }
}

