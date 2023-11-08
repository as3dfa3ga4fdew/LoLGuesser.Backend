using Api.Services.Interfaces;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Api.Exceptions;

namespace Api.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Create(List<Claim> claims)
        {
            string key = _configuration["Jwt:Key"];
            if (key == null)
                throw new MissingPropertyException(nameof(IConfiguration) + " " + nameof(key));

            string issuer = _configuration["Jwt:Issuer"];
            if(issuer == null)
                throw new MissingPropertyException(nameof(IConfiguration) + " " + nameof(issuer));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer,
              issuer,
              claims,
              expires: DateTime.Now.AddMinutes(1200),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool TryGetClaim(HttpContext context, string type, out Claim claim)
        {
            claim = null;

            //Parse token
            string jwt = context.Request.Headers.Authorization;

            if (jwt == null) 
                return false;

            string[] jwtItems = jwt.Split("Bearer ");

            if (jwtItems.Length != 2)
                return false;

            //Decode jwt token
            JwtSecurityToken decodedToken = null;
            try
            {
                decodedToken = new JwtSecurityToken(jwtItems[1]);
            }
            catch (Exception e)
            {
                return false;
            }

            claim = decodedToken.Claims.FirstOrDefault(c => c.Type == type);

            return claim != null;
        }
    }
}
