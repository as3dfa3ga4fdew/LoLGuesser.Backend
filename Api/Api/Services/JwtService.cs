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

        public JwtService(IConfiguration configuration, ILogger<IJwtService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string Create(List<Claim> claims)
        {
            string key = _configuration["Jwt:Key"];
            string issuer = _configuration["Jwt:Issuer"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer,
              issuer,
              claims,
              expires: DateTime.Now.AddMinutes(1200),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
