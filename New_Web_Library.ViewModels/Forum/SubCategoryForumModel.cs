namespace New_Web_Library.ViewModels.Forum
{
    public class SubCategoryForumModel
    {
        public int Id { get; set; }

        public string PostTitle { get; set; } = null!;

        public string PostAuthor { get; set; } = null!;

        public string PostCategory { get; set; } = null!;

        public  DateTime CreatedOn { get; set; }

        public int CommentCount { get; set; }

        
    }
}
