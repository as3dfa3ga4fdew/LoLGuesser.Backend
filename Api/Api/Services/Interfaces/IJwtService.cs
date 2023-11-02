using System.Security.Claims;

namespace Api.Services.Interfaces
{
    public interface IJwtService
    {
        public string Create(List<Claim> claims);
    }
}
