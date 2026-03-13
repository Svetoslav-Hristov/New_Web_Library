using New_Web_Library.Services.Core.Common;
using New_Web_Library.ViewModels.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Web_Library.Service.Core.Interfaces
{
    public interface ITopicService
    {
        Task<ServiceResult<IEnumerable<SubCategoryForumModel>>> SubCategoryIndexPreview(int Id);
    }
}
