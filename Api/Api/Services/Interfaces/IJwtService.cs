using System.Security.Claims;

namespace Api.Services.Interfaces
{
    public interface IJwtService
    {
        public string Create(List<Claim> claims);
        public bool TryGetClaim(HttpContext context, string type, out Claim claim);
    }
}
