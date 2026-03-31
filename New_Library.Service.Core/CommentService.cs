using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using New_Library.Data.Models.Forum;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data.Models;
using New_Web_Library.Data.Models.Contracts;
using New_Web_Library.Service.Core.Interfaces;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.ViewModels.Forum;
using System.Numerics;
using static New_Web_Library.GCommon.EntityValidations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace New_Web_Library.Service.Core
{
    public class CommentService : ICommentService
    {

        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _usersRepository;
        private readonly ILogger<ICommentService> _logger;

        public CommentService(ICommentRepository commentRepository, IPostRepository postRepository,
            IUserRepository usersRepository,ILogger<ICommentService> logger)

        {
            this._commentRepository = commentRepository;
            this._postRepository = postRepository;
            this._usersRepository = usersRepository;
            this._logger = logger;
        }



        public async Task<ServiceResult<CreateContentViewModel>> CreateNewComment(int Id)
        {
            var post = await _postRepository.GetByIdAsync(Id);

            if (post == null)
            {
                return new ServiceResult<CreateContentViewModel> 
                { 
                    Success = false,
                    ErrorMessage = "Post not found!" 
                };

            }


            string commentTitle = $"Re:{post.Title}";
            
            CreateContentViewModel model = new CreateContentViewModel()
            {
                Title = commentTitle,
                PostId = post.Id,


            };


            return new ServiceResult<CreateContentViewModel> { Success = true, Data = model };

        }


        public async Task<ServiceResult<Comment>> ConfirmNewComment(CreateContentViewModel model, int Id, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(model.Description))
            {
                return new ServiceResult<Comment> 
                {
                    Success = false,
                    ErrorMessage = "Content is required!" 
                };
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
                Content = model.Description.Trim(),
                UserId = userId,
                PostId = Id

            };

            try
            {

                await _commentRepository.AddAsync(newComment);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating comment create by {0} {1}", user.FirstName, user.LastName);

                return new ServiceResult<Comment>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while create new comment! Please try again later."
                };

            }

            return new ServiceResult<Comment> { Success = true,Data=newComment};

        }

        public async Task<ServiceResult<CreateContentViewModel>> EditComment(int Id, Guid userId)
        {
            var comment = await _commentRepository.GetCommentWithPostAsync(Id);

            if (comment == null)
            {

                return new ServiceResult<CreateContentViewModel> 
                { 
                    Success = false,
                    ErrorMessage = "Comment not found !" 
                };

            }

            if (comment.UserId != userId)
            {
                return new ServiceResult<CreateContentViewModel> 
                {
                    Success = false,
                    ErrorMessage = "You don't have permission over this comment." 
                };

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
            if (string.IsNullOrWhiteSpace(model.Description))
            {
                return new ServiceResult<Comment> 
                {
                    Success = false, 
                    ErrorMessage = "Content is required!" 
                };
            }

            Comment? comment = await _commentRepository.GetCommentWithUserAsync(Id);



            if (comment == null)
            {
                return new ServiceResult<Comment> 
                {
                    Success = false, 
                    ErrorMessage = "Comment not found!" 
                };
            }

            try
            {
                comment.Content = model.Description.Trim();
                comment.UpdatedAt = DateTime.UtcNow;
                await _commentRepository.UpdateAsync(comment);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing comment create by {0} {1}", comment.User.FirstName, comment.User.LastName);

                return new ServiceResult<Comment>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while edit  comment! Please try again later."
                };

            }

            return new ServiceResult<Comment> { Success = true, Data = comment };

        }

        public async Task<ServiceResult<Post>> SoftDeleteComment(int Id, int postId, Guid userId)
        {
            var comment = await _commentRepository.GetCommentWithUserAsync(Id);



            if (comment == null)
            {
                return new ServiceResult<Post> 
                {
                    Success = false,
                    ErrorMessage = "Comment not found!" 
                };
            }

            if (userId == Guid.Empty)
            {
                return new ServiceResult<Post> 
                {
                    Success = false,
                    ErrorMessage = "Invalid user Id!" 
                };
            }

            var user = await _usersRepository.FindByIdAsync(userId);

            if (user == null)
            {
                return new ServiceResult<Post> 
                {
                    Success = false,
                    ErrorMessage = "User not found!" 
                };
            }

            var isAdmin = await _usersRepository.AdminOrNotAsync(userId);

            if ( user.Id != comment.UserId && !isAdmin)
            {

                return new ServiceResult<Post> 
                {
                    Success = false, 
                    ErrorMessage = "You don't have permission over this comment." 
                };

            }

            Post? post = await _postRepository.GetByIdAsync(postId);

            if (post == null)
            {
                return new ServiceResult<Post> 
                {
                    Success = false, 
                    ErrorMessage = "Post not found!" 
                };
            }

            try
            {

                comment.IsDeleted = true;
                comment.DeleteAt = DateTime.UtcNow;
                await _commentRepository.UpdateAsync(comment);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error soft deleting comment  by user  {0} {1}", comment.User?.FirstName, comment.User?.LastName);

                return new ServiceResult<Post>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred ! Please try again later."
                };


            }


            return new ServiceResult<Post> { Success = true, Data = post };

        }

        public async Task<ServiceResult<bool>> RestoreDeleteComment(int Id)
        {
            var comment = await _commentRepository.GetSoftDeleteCommentAsync(Id);

            if (comment == null)
            {
                return new ServiceResult<bool> { Success = false, ErrorMessage = "Comment not found!" };
            }

            Post? post = await _postRepository.GetDeleteOrNotPostAsync(comment.PostId);

            if (post?.IsDeleted==true)
            {
                
                    return new ServiceResult<bool>
                    {
                        Success = false,
                        ErrorMessage = "You won't be able to return the comment because the post is also missing!"
                    };
                
            }



            try
            {

                comment.IsDeleted = false;
                comment.DeleteAt = null;
                comment.UpdatedAt = DateTime.UtcNow;
                await _commentRepository.UpdateAsync(comment);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error restoring comment delete by user  {0} {1}", comment.User?.FirstName, comment.User?.LastName);

                return new ServiceResult<bool>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while restore delete comment! Please try again later."
                };

            }


            return new ServiceResult<bool> { Success = true };
        }

        public async Task<ServiceResult<bool>> HardDeleteComment(int Id)
        {
            var comment = await _commentRepository.GetSoftDeleteCommentAsync(Id);

            if (comment == null)
            {
                return new ServiceResult<bool> { Success = false, ErrorMessage = "Comment not found!" };
            }

            
            try
            {

                await _commentRepository.DeleteAsync(comment);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error hard deleting comment  by user  {0} {1}",comment.User?.FirstName,comment.User?.LastName);

                return new ServiceResult<bool>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while hard delete comment! Please try again later."
                };

            }

            return new ServiceResult<bool> { Success = true };

        }
    }
}

