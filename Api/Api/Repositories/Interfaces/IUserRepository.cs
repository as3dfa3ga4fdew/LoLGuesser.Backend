using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<UserEntity?> GetByUsernameAsync(string username);
        public Task<bool> CreateAsync(UserEntity user);
    }
}
