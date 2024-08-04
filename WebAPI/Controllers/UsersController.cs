using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServerLibrary;
using SharedLibrary;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IConfiguration configuration, ILogger<UsersController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost("login")]
    public IResult Login(UserLogin userLogin)
    {
        _logger.LogInformation($"Login attempt for user: {userLogin.Username}");

        try
        {
            if (string.IsNullOrEmpty(userLogin.Username) || string.IsNullOrEmpty(userLogin.Password))
            {
                _logger.LogWarning($"User not found: {userLogin.Username}");
                return Results.BadRequest("Invalid user credentials");
            }

            // get user from database
            User user = ServerConfig.DataAccessor.LoadUser(userLogin);
            if (user == null)
            {
                return Results.NotFound("User not found");
            }

            // create claims
            List<Claim> claims = new();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Username));

            // create the new JWT token
            JwtSecurityToken token = new
            (
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                notBefore: DateTime.Now,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256
                )
            );

            // return JWT token
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            _logger.LogInformation($"JWT token generated successfully for user: {user.Username}");
            return Results.Ok(tokenString);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while processing the login for user: {userLogin.Username}");
            return Results.Problem(ex.Message);
        }    
    }
}
