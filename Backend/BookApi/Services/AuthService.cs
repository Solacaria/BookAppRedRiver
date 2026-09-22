using BookApi.Models;
using BookApi.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BookApi.Services;
public class AuthService : IAuthService
{
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext dbcontext, IConfiguration configuration)
    {
        _dbContext = dbcontext;
        _configuration = configuration;
    }

    public async Task<bool> CheckUserExists(string username)
    {
        return await _dbContext.Users.AnyAsync(u => u.UserName.ToLower() == username.ToLower());
    }

    public async Task<bool> RegisterUserAsync(string username, string password)
    {
        var user = new User
        {
            UserName = username
        };

        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, password);

        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<string?> LoginUserAsync(string username, string password)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());
        if (user == null) return null;

        var hasher = new PasswordHasher<User>();
        var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);

        if (result == PasswordVerificationResult.Failed) return null;

        var token = CreateToken(user);
        return token;

    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim> {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };
    
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );
    
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(3),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}