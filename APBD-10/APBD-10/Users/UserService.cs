using APBD_10.Context;
using APBD_10.Users.Models;
using APBD_10.Utils;


namespace APBD_10.Users;

public class UserService : IUserService
{
    private readonly HospitalDbContext _context;

    public UserService(HospitalDbContext context)
    {
        _context = context;
    }


    public async Task<bool> ValidateUserAsync(string login, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        string hashedPassword = SecurityUtils.GetHashedPasswordWithSalt(password, user.Salt);
        
        return user.Password == hashedPassword;
    }

    public async Task RegisterUserAsync(string email, string username, string password)
    {
        var hashedPasswordWithSalt = SecurityUtils.GetHashedPasswordAndSalt(password);
        var refreshToken = SecurityUtils.GenerateRefreshToken();
        
        var user = new User
        {
            Email = email,
            Login = username,
            Password = hashedPasswordWithSalt.Item1,
            Salt = hashedPasswordWithSalt.Item2,
        };
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<string> GetRefreshTokenAsync(string username)
    {
        var res = await _context.Users.FirstOrDefaultAsync(u => u.Login == username);
        return res.RefreshToken;
    }

    public async Task SetRefreshTokenAsync(string login, string refreshToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        user.RefreshToken = refreshToken;
        user.RefreshTokenExp = DateTime.Now.AddDays(1);

        await _context.SaveChangesAsync();
    }
}