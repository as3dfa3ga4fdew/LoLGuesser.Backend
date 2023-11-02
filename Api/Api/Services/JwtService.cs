using Api.Services.Interfaces;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Api.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<IJwtService> _logger;
        private readonly SymmetricSecurityKey _symmetricSecurityKey;
        private readonly SigningCredentials _signingCredentials;
        private readonly string _key;
        private readonly string _issuer;

        public JwtService(IConfiguration configuration, ILogger<IJwtService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            _key = _configuration["Jwt:Key"];
            _issuer = _configuration["Jwt:Issuer"];

            if (string.IsNullOrEmpty(_key) || string.IsNullOrEmpty(_issuer))
            {
                _logger.LogError("Missing keys in appsettings.json");
                throw new InvalidOperationException();
            }

            _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            _signingCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        }

        public string Create(List<Claim> claims)
        {
            SecurityToken token = new JwtSecurityToken(
                _issuer,
                _issuer,
                claims,
                expires: DateTime.UtcNow.AddMinutes(120),
                signingCredentials: _signingCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
