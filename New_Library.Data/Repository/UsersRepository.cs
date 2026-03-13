using Microsoft.EntityFrameworkCore;
using New_Library.Data.Repository.Contracts;
using New_Web_Library.Data;
using New_Web_Library.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Library.Data.Repository
{
    public class UsersRepository : BaseRepository, IUsersRepository
    {
        public UsersRepository(LibraryDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<IEnumerable<User>> CheckOverdueUsers(List<Guid> users)
        {
            var overdueUsers = await _dbContext.Users.Where(u => users.Contains(u.Id) && !u.IsBlocked).ToListAsync();

            return overdueUsers;

        }

        public async Task<User?> FindByIdAsync(Guid Id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == Id);

        }

        public IQueryable<User> GetAllUsers()
        {
            IQueryable<User> allUsers = _dbContext.Users.AsNoTracking().
                OrderBy(u => u.FirstName).ThenBy(u => u.LastName);


            return  allUsers;
        }

        public async Task<bool> IsExistUser(Guid userId)
        {
            return await _dbContext.Users.AsNoTracking().AnyAsync(u => u.Id == userId);
        }

        public async Task<User?> SearchByPhoneOrEmail(string criteria)
        {
            var foundUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == criteria || u.PhoneNumber.ToLower() == criteria);

            return foundUser;

        }

        public async Task<User?> UserFullDetailsAndHistory(Guid userId)
        {
            var foundUser = await _dbContext.Users.Include(u => u.UserBooks).ThenInclude(ub => ub.Book)
                .AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);

            return  foundUser;
        }
    }
}
