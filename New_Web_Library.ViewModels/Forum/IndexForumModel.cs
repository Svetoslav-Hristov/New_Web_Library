using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Web_Library.ViewModels.Forum
{
    public class IndexForumModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }
       
        public TopicForumModel[]? Topics { get; set; }

        public int PostCount { get; set; }

        public string? LastPostTitle { get; set; }

    }
}
