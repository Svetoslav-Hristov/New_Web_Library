using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Library.Data.Models.Contracts
{
    public interface IContent
    {
        int Id { get; set; }

        string Content { get; set; }

        DateTime CreatedOn { get; set; }

        bool IsDeleted { get; set; }


    }
}
