using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Library.Data.Repository
{
    public class TopicsRepository : BaseRepository, ITopicsRepository
    {
        public TopicsRepository(LibraryDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
