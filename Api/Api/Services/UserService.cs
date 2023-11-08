using Api.Models.Entities;
using Api.Models.Schemas;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";
        private const string AlphabetUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Numbers = "0123456789";
        private const string Special = "~!@#$%^&*()-_+={}][|\\`,./?;:'\"<>";
        public bool ValidateUsername(string username)
        {
            if (username == null) return false;

            if (username.Length <= 5) return false;

            if (username.Length >= 16) return false;

            if (username.All(x => (Alphabet + AlphabetUpper + Numbers).Contains(x)) == false) return false;

            return true;
        }
        public bool ValidatePassword(string password)
        {
            if (password == null) return false;

            if (password.Length <= 7) return false;

            if (password.Length >= 50) return false;

            if (password.All(x => (Alphabet + AlphabetUpper + Numbers + Special).Contains(x)) == false) return false;

            if (password.Any(x => Alphabet.Contains(x)) == false ||
                password.Any(x => AlphabetUpper.Contains(x)) == false ||
                password.Any(x => Numbers.Contains(x)) == false ||
                password.Any(x => Special.Contains(x)) == false)
                return false;

            return true;
        }
       
        public bool Validate<TSchema>(TSchema schema)
        {
            if (schema == null)
                return false;

            switch (schema)
            {
                case LoginSchema loginSchema:
                    if(ValidateUsername(loginSchema.Username) && ValidatePassword(loginSchema.Password))
                        return true;
                    break;
                case RegisterSchema registerSchema:
                    if (ValidateUsername(registerSchema.Username) && ValidatePassword(registerSchema.Password))
                        return true;
                    break;
                default:
                    throw new Exception(nameof(Validate));
            }

            return false;
        }

        public async Task<UserEntity> GetByUsernameAsync(string username)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username));

            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<UserEntity> GetByIdAsync(Guid id)
        {
            return await _userRepository.GetAsync(x => x.Id == id);
        }
        
        public async Task<bool> UpdateAsync(UserEntity entity)
        {
            if(entity == null)
                throw new ArgumentNullException(nameof(entity));

            return await _userRepository.UpdateAsync(entity);
        }

        public async Task<bool> CreateAsync(UserEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return await _userRepository.CreateAsync(entity);
        }

        public async Task<bool> VerifyPasswordAsync(string plain, string hash)
        {
            return await Task.Run(() => BCrypt.Net.BCrypt.Verify(plain, hash));
        }

        public async Task<string> HashPasswordAsync(string plain, int costFactor = 10)
        {
            return await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(plain, costFactor));
        }
    }
}
