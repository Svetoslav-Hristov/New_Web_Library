using New_Web_Library.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Library.Data.Repository.Contracts
{
    public interface IUsersRepository : IBaseRepository
    {
        Task<User?> FindByIdAsync(Guid Id);

        Task<bool> IsExistUser(Guid userId);

        IQueryable<User> GetAllUsers();

        Task<IEnumerable<User>> CheckOverdueUsers(List<Guid> users);

        Task<User?> SearchByPhoneOrEmail(string criteria);

        Task<User?> UserFullDetailsAndHistory(Guid userId);


    }
}
