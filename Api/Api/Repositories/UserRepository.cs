using Api.Contexts;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class UserRepository : Repository<UserEntity>, IUserRepository
    {
        public UserRepository(DataContext context, ILogger<IRepository<UserEntity>> logger) : base(context, logger)
        {
        }

        public async Task<UserEntity?> GetByUsernameAsync(string username)
        {
            if(username == null)
                throw new ArgumentNullException(nameof(username));

            return await _context.Users.Where(x => x.Username == username).FirstOrDefaultAsync();
        }
    }
}
