using BookApi.Models;
using BookApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)    
    {
        var username = request.Username;
        var password = request.Password;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return BadRequest(new { message = "Användarnamn och lösenord måste anges." });
        }

        var userExists = await authService.CheckUserExists(username);
        if (userExists)
        {
            return BadRequest(new { message = "Användarnamnet är redan taget." });
        }

        try
        {            
            var sucess = await authService.RegisterUserAsync(username, password);
            if (!sucess) return BadRequest(new { message = "Misslyckades med registrering av användare." });

            return Ok(new { message = "Registrering lyckades.", sucess });
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Misslyckades med registrering av användare." });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var username = request.Username;
        var password = request.Password;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return BadRequest(new { message = "Både användarnamn och lösenord måste anges." });
        }

        try
        {
            var token = await authService.LoginUserAsync(username, password);
            if (token == null)
            {
                return Unauthorized(new {message = "Kontot finns inte"});
            }

            return Ok(new {token = token}); 
        } 
        catch (Exception)
        {
            return BadRequest(new { message = "Misslyckades med inloggning." });
        }      
    }
}
