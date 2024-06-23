using System.Security.Claims;
using APBD_10.Auth.RequestModels;
using APBD_10.Users;
using APBD_10.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

namespace APBD_10.Auth;

public static class Configuration
{
       public static void RegisterEndpointsForAuth(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/register", [AllowAnonymous] async (IUserService userService, RegisterRequest credentials) =>
        {
            await userService.RegisterUserAsync(credentials.Email, credentials.Login, credentials.Password);
            return Results.Ok();
        });

        app.MapPost("/api/login", [AllowAnonymous] async (IUserService userService, IAuthService authService, LoginRequest credentials) =>
        {
            if (!await userService.ValidateUserAsync(credentials.Username, credentials.Password))
                return Results.Unauthorized();
            
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, credentials.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var accessToken = authService.GenerateAccessToken(claims);
            
            var refreshToken = SecurityUtils.GenerateRefreshToken();
            
            await userService.SetRefreshTokenAsync(credentials.Username, refreshToken);
            
            return Results.Ok(new { accessToken, refreshToken });
        });

        app.MapPost("/api/refresh", [Authorize(AuthenticationSchemes = "IgnoreTokenExpirationScheme")] async (HttpContext httpContext, IUserService userService, IAuthService authService, RefreshTokenRequest refreshTokenRequest) =>
        {
            var username = authService.GetPrincipalFromExpiredToken(refreshTokenRequest.RefreshToken)?.Identity?.Name;

            if (username is null || refreshTokenRequest.RefreshToken != await userService.GetRefreshTokenAsync(username))
                return Results.Unauthorized();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var newAccessToken = authService.GenerateAccessToken(claims);
            var newRefreshToken = SecurityUtils.GenerateRefreshToken();
            await userService.SetRefreshTokenAsync(username, newRefreshToken);

            return Results.Ok(new { accessToken = newAccessToken, refreshToken = newRefreshToken });
        });
    }
}