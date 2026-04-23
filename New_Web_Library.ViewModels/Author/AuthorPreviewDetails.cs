using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Web_Library.ViewModels.Author
{
    public class AuthorPreviewDetails
    {
        public string Name { get; set; } = null!;

        public string? Biography { get; set; }

        public string? ImageUrl { get; set; }
    }
}
