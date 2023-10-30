using Api.Contexts;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<IUserRepository> _logger;

        public UserRepository(DataContext dataContext, ILogger<IUserRepository> logger)
        {
            _context = dataContext;            _logger = logger;
        }

        public async Task<UserEntity?> GetByUsernameAsync(string username)
        {
            if(username == null)
                throw new ArgumentNullException(nameof(username));

            return await _context.Users.Where(x => x.Username == username).FirstOrDefaultAsync();
        }
        public async Task<bool> CreateAsync(UserEntity user)
        {
            if (await GetByUsernameAsync(user.Username) != null)
                return false;

            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                _logger.LogError(e, "CreateAsync");
                return false;
            }
            
            return true;
        }
    }
}
