using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Web_Library.Service.Core.Interfaces
{
    public interface ICategory
    {
        int Id { get; set; }

        string Name { get; set; }

        string? Description { get; set; }

        bool IsDeleted { get; set; }


    }
}
