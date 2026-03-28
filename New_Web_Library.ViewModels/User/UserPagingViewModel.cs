

namespace New_Web_Library.ViewModels.User
{
    public class UserPagingViewModel
    {
        public string? Search { get; set; }

        public int CurrentPage { get; set; }
        
        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public IEnumerable<PreviewUserModel> Users { get; set; } = new List<PreviewUserModel>();
    }
}
