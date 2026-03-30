using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Web_Library.ViewModels.Forum
{
    public class DeletedItemsViewModel
    {
        public bool IsExistSpecialSubCategory { get; set; }


        public List<DeletedItemViewModel> DeleteItems { get; set; } = new List<DeletedItemViewModel>(); 

    }
}
