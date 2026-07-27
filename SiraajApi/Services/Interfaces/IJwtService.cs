using System.Security.Claims;
using SiraajApi.Models;

namespace SiraajApi.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        bool ValidateRefreshToken(string token, User user);
    }
}
