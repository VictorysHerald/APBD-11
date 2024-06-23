namespace APBD_10.Users;

public interface IUserService
{
    Task<bool> ValidateUserAsync(string username, string password);
    Task RegisterUserAsync(string email, string username, string password);
    Task<string> GetRefreshTokenAsync(string username);
    Task SetRefreshTokenAsync(string username, string refreshToken);
}