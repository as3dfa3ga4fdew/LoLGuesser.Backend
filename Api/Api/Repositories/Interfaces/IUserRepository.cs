using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        public Task<UserEntity?> GetByUsernameAsync(string username);
    }
}
