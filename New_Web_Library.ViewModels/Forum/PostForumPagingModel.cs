using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Web_Library.ViewModels.Forum
{
    public class PostForumPagingModel
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCommentsCount { get; set; }

        public PostForumModel Post { get; set; } = null!;

        public IEnumerable<ContentDetailsModel> Comments { get; set; } = new List<ContentDetailsModel>();

    }
}
