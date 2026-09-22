using BookApi.Models;

namespace BookApi.Services;

public interface IAuthService
{
    Task<bool> CheckUserExists(string username);
    Task<bool> RegisterUserAsync(string username, string password);
    Task<string?> LoginUserAsync(string username, string password);



}