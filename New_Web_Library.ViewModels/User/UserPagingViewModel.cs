

namespace New_Web_Library.ViewModels.User
{
    public class UserPagingViewModel
    {
        public string? Search { get; set; }

        public int CurrentPage { get; set; }
        
        public int TotalPages { get; set; }

        public IEnumerable<User> Users { get; set; }
    }
}
