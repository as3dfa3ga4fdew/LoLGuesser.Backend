using Api.Models.Entities;

namespace Api.Services.Interfaces
{
    public interface IUserService
    {
        public bool Validate<TSchema>(TSchema schema);
        public Task<UserEntity> GetByUsernameAsync(string username);
        public Task<UserEntity> GetByIdAsync(Guid id);
        public Task<bool> UpdateAsync(UserEntity entity);
        public Task<bool> CreateAsync(UserEntity userEntity);
        public Task<bool> VerifyPasswordAsync(string plain, string hash);
        public Task<string> HashPasswordAsync(string plain, int costFactor = 10);
    }
}
