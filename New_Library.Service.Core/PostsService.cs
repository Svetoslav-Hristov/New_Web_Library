using New_Library.Data.Models.Forum;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data.Models;
using New_Web_Library.Service.Core.Interfaces;
using New_Web_Library.Services.Core.Common;
using New_Web_Library.ViewModels.Forum;
using static New_Web_Library.GCommon.EntityValidations.Posts;
using static New_Web_Library.GCommon.EntityValidations.Topics;

namespace New_Web_Library.Service.Core
{
    public class PostsService : IPostsService
    {

        private readonly IPostsRepository _postsRepository;
        private readonly ICommentsRepository _commentsRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly ITopicsRepository _topicsRepository;
        public PostsService(IPostsRepository postsRepository, ICommentsRepository commentsRepository,
            ICategoriesRepository categoriesRepository, IUsersRepository usersRepository,
            ITopicsRepository topicsRepository)
        {
            this._postsRepository = postsRepository;
            this._commentsRepository = commentsRepository;
            this._categoriesRepository = categoriesRepository;
            this._usersRepository = usersRepository;
            this._topicsRepository = topicsRepository;
        }


        public async Task<ServiceResult<PostForumPagingModel>> PostDetailModelsPreview(
            int Id, Guid? userId, int pageNumber, int pageSize)
        {

            Post? post = await _postsRepository.GetByIdAsync(Id);

            Topic? specialCategory = await _topicsRepository.GetSubCategoryByName(TopicSpecialName);

            if (post == null || (specialCategory != null && post.TopicId == specialCategory.Id))
            {
                return new ServiceResult<PostForumPagingModel>
                {
                    Success = false,
                    ErrorMessage = "Not Found!"
                };
            }



            var allComments = post.Comments
                        .Where(c => !c.IsDeleted)
                        .OrderBy(c => c.CreatedOn)
                        .ToList();

            int totalCommentCount = allComments.Count();

            var pagedComments = allComments
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(c => new ContentDetailsModel
                    {
                        Id = c.Id,
                        Title = $"Re:{post.Title}",
                        Content = c.Content,
                        CreatedOn = c.CreatedOn,
                        AuthorName = $"{c.User.FirstName} {c.User.LastName}",
                        UserId = c.UserId
                    })
                    .ToList();


            PostForumModel model = new PostForumModel()
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreatedOn = post.CreatedOn,
                AuthorName = $"{post.User.FirstName} {post.User.LastName}",
                UserId = post.UserId,
                TopicId = post.TopicId,



            };

            PostForumPagingModel pagingModel = new PostForumPagingModel
            {
                Post = model,
                Comments = pagedComments,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCommentsCount = totalCommentCount
            };


            var usersId = post.Comments.Select(p => p.UserId).Append(post.UserId).Distinct().ToList();





            var countComments = await _commentsRepository.GetAllCountComments(usersId);


            var countPosts = await _postsRepository.GetAllCountPosts(usersId);


            foreach (var user in pagingModel.Comments)
            {

                user.UserCommentCount = countComments.GetValueOrDefault(user.UserId);
                user.UserPostCount = countPosts.GetValueOrDefault(user.UserId);
            }


            model.UserPostCount = countPosts.GetValueOrDefault(post.UserId);
            model.UserCommentCount = countComments.GetValueOrDefault(post.UserId);

            // && DateTime.UtcNow - model.CreatedOn < TimeSpan.FromMinutes(CommentLifeTime)
            if (userId != null)
            {

                if (!pagingModel.Comments.Any() && model.UserId == userId)
                {
                    model.IsAuthor = true;

                }
                else
                {
                    var lastComment = pagingModel.Comments.OrderByDescending(p => p.CreatedOn).FirstOrDefault();


                    if (lastComment?.UserId == userId && DateTime.UtcNow - lastComment.CreatedOn < TimeSpan.FromMinutes(CommentLifeTime))
                    {
                        lastComment.IsAuthor = true;
                    }

                }
            }

            return new ServiceResult<PostForumPagingModel> { Success = true, Data = pagingModel };

        }



        public async Task<ServiceResult<CreateContentViewModel>> CreateNewPost(int categoryId)
        {
            var subCategory = await _topicsRepository.GetByIdAsync<Topic>(categoryId);



            if (subCategory == null)
            {
                return new ServiceResult<CreateContentViewModel> { Success = false, ErrorMessage = "SubCategory not found!" };
            }

            CreateContentViewModel model = new CreateContentViewModel()
            {
                SubCategoryId = subCategory.Id


            };


            return new ServiceResult<CreateContentViewModel> { Success = true, Data = model };

        }

        public async Task<ServiceResult<Post>> ConfirmNewPost(CreateContentViewModel model, Guid userId, int categoryId)
        {
            if (userId == Guid.Empty)
            {
                return new ServiceResult<Post> { Success = false, ErrorMessage = "Invalid userId" };
            }

            User? user = await _usersRepository.FindByIdAsync(userId);

            if (user == null)
            {
                return new ServiceResult<Post> { Success = false, ErrorMessage = "User not found!" };
            }

            Topic? subCategory = await _topicsRepository.GetByIdAsync<Topic>(categoryId);

            if (subCategory == null)
            {
                return new ServiceResult<Post> { Success = false, ErrorMessage = "SubCategory not found!" };
            }

            Post newPost = new Post()
            {

                Title = model.Title,
                Content = model.Description,
                CreatedOn = DateTime.UtcNow,
                UserId = userId,
                User = user,
                TopicId = subCategory.Id


            };

            try
            {

                await _postsRepository.AddAsync(newPost);


            }
            catch (Exception)
            {

                return new ServiceResult<Post>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while create new post! Please try again later."
                };


            }

            return new ServiceResult<Post> { Success = true, Data = newPost };


        }

        public async Task<ServiceResult<CreateContentViewModel>> EditPost(int Id)
        {
            var post = await _postsRepository.GetByIdAsync<Post>(Id);

            if (post == null)
            {
                return new ServiceResult<CreateContentViewModel> { Success = false, ErrorMessage = "Post not found" };
            }



            CreateContentViewModel model = new CreateContentViewModel()
            {
                Title = post.Title,
                Description = post.Content,
                PostId = post.Id


            };

            return new ServiceResult<CreateContentViewModel> { Success = true, Data = model };


        }

        public async Task<ServiceResult<Post>> ConfirmEditPost(CreateContentViewModel model, Guid userId, int Id)
        {
            var user = await _usersRepository.FindByIdAsync(userId);

            if (user == null)
            {
                return new ServiceResult<Post> { Success = false, ErrorMessage = "User not found!" };

            }

            var post = await _postsRepository.GetByIdAsync<Post>(Id);


            if (post == null)
            {
                return new ServiceResult<Post> { Success = false, ErrorMessage = "Post not found!" };
            }



            if (post.UserId != user.Id)
            {
                return new ServiceResult<Post> { Success = false, ErrorMessage = "Тhis post belongs to another user!" };
            }


            try
            {
                post.Title = model.Title;
                post.Content = model.Description;
                post.UpdatedAt = DateTime.UtcNow;
                await _postsRepository.UpdateAsync(post);

            }
            catch (Exception)
            {

                return new ServiceResult<Post>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while edit  post! Please try again later."
                };
            }


            return new ServiceResult<Post> { Success = true, Data = post };

        }

        public async Task<ServiceResult<int>> SoftDeletePost(int Id, Guid userId)
        {
            var post = await _postsRepository.GetByIdAsync(Id);

            if (post == null)
            {
                return new ServiceResult<int> { Success = false, ErrorMessage = "Post not found!" };
            }

            var user = await _usersRepository.FindByIdAsync(userId);

            if (user == null)
            {
                return new ServiceResult<int> { Success = false, ErrorMessage = "User not found!" };
            }


            bool isAdmin = await _usersRepository.AdminOrNot(userId);

            if (post.UserId != userId && !isAdmin )
            {
                return new ServiceResult<int> { Success = false, ErrorMessage = "You don't have permission over this post." };

            }

            try
            {

                post.IsDeleted = true;
                post.DeleteAt = DateTime.UtcNow;
                await _postsRepository.UpdateAsync(post);


            }
            catch (Exception)
            {
                return new ServiceResult<int>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while delete post! Please try again later."
                };

            }

            return new ServiceResult<int> { Success = true, Data = post.TopicId };


        }

        public async Task<ServiceResult<bool>> RestoreDeletePost(int Id)
        {
            var post = await _postsRepository.GetDeleteOrNotPost(Id);

            if (post == null)
            {
                return new ServiceResult<bool> { Success = false, ErrorMessage = "Post not found!" };
            }

            Topic? subCategory = await _topicsRepository.GetDeleteOrNotSubCategory(post.TopicId);

            if (subCategory != null)
            {
                if (subCategory.IsDeleted)
                {
                    return new ServiceResult<bool>
                    {
                        Success = false,
                        ErrorMessage = "You won't be able to return the Post because the SubCategory is also missing!"
                    };
                }
            }



            try
            {

                post.IsDeleted = false;
                post.DeleteAt = null;
                post.UpdatedAt = DateTime.UtcNow;
                await _postsRepository.UpdateAsync(post);

            }
            catch (Exception)
            {

                return new ServiceResult<bool>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while restore delete Post! Please try again later."
                };

            }


            return new ServiceResult<bool> { Success = true };
        }

        public async Task<ServiceResult<bool>> HardDeletePost(int Id)
        {
            var post = await _postsRepository.GetDeleteOrNotPost(Id);

            if (post == null)
            {
                return new ServiceResult<bool> { Success = false, ErrorMessage = "Post not found!" };
            }

            try
            {

                await _postsRepository.DeleteAsync(post);

            }
            catch (Exception)
            {
                return new ServiceResult<bool>
                {
                    Success = false,
                    ErrorMessage = "Unexpected error is occurred while hard delete Post! Please try again later."
                };

            }

            return new ServiceResult<bool> { Success = true };
        }

        public async Task<ServiceResult<ContentDetailsModel>> GetUserComplaint(int Id)
        {
            var post = await _postsRepository.GetByIdAsync(Id);

            if (post == null)
            {
                return new ServiceResult<ContentDetailsModel> { Success = false, ErrorMessage = "Post not found!" };
            }

            ContentDetailsModel model = new ContentDetailsModel()
            {

                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreatedOn = post.CreatedOn,
                AuthorName = $"{post.User.FirstName} {post.User.LastName}",
                UserId = post.UserId,
                TopicId = post.TopicId
            };


            model.UserPostCount = await _postsRepository.GetAllPostCount(post.UserId);

            model.UserCommentCount = await _commentsRepository.GetAllCommentsCount(post.UserId);

            return new ServiceResult<ContentDetailsModel> { Success = true, Data = model };


        }
    }
}

