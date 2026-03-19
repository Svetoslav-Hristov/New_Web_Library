using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Web_Library.ViewModels.Forum
{
    public class DeletedItemViewModel
    {

        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Type { get; set; } = null!;


        public string? Description { get; set; }

       
        public int? ParentId { get; set; }

        public string? ParentName { get; set; }


    }
}
