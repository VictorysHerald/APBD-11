using System.Security.Claims;

namespace APBD_10.Auth;

public interface IAuthService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}