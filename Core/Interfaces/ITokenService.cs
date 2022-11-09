using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.Entities.Identity;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        JwtSecurityToken CreateToken(AppUser user);
        string CreateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}