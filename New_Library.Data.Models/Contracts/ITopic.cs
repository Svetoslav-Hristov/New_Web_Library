using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Web_Library.Service.Core.Interfaces
{
    public interface ITopic
    {
        int Id { get; set; }

        string Title { get; set; }

        DateTime CreatedOn { get; set; }

        bool IsDeleted { get; set; }
    }
}
