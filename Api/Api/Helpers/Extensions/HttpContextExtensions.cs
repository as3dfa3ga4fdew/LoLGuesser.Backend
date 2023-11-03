using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.Helpers.Extensions
{
    public static class HttpContextExtensions
    {
        public static Claim GetClaim(this HttpContext context, string type)
        {
            //Parse token
            string jwt = context.Request.Headers.Authorization.ToString();

            if (jwt == null) return null;

            string[] jwtItems = jwt.Split("Bearer ");

            if (jwtItems.Length != 2) return null;

            //Decode jwt token
            JwtSecurityToken decodedToken = null;

            try
            {
                decodedToken = new JwtSecurityToken(jwtItems[1]);
            }
            catch(Exception e)
            {
                return null;
            }

            return decodedToken.Claims.FirstOrDefault(c => c.Type == type);
        }
    }
}
