using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServerLibrary;
using SharedLibrary.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<UsersController> _logger;
    public AdminController(IConfiguration configuration, ILogger<UsersController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost("login")]
    public IResult AdminLogin(Credentials adminCredentials)
    {
        _logger.LogInformation($"Login attempt for admin: {adminCredentials.Username}");

        try
        {
            User admin = ServerConfig.LoadAdmin(adminCredentials);

            // create claims
            List<Claim> claims = new();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, admin.Username));
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));

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
            _logger.LogInformation($"JWT token generated successfully for admin: {admin.Username}");
            return Results.Ok(tokenString);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while processing the login for admin: {adminCredentials.Username}");
            return Results.Problem(ex.Message);
        }
    }

    [HttpPost("create")]
    [Authorize(Roles = "Admin")]
    public IResult CreateAdmin(Credentials adminCredentials)
    {
        _logger.LogInformation($"New admin creation attempt for admin: {adminCredentials.Username}");

        try
        {
            ServerConfig.CreateAdmin(adminCredentials);
            _logger.LogInformation($"Admin created successfully: {adminCredentials.Username}");
            return Results.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while processing the new admin creation for admin: {adminCredentials.Username}");
            return Results.Problem(ex.Message);
        }
    }

    [HttpPost("delete")]
    [Authorize(Roles = "Admin")]
    public IResult DeleteAdmin(Credentials adminCredentials)
    {
        _logger.LogInformation($"DeleteAdmin method called for admin: {adminCredentials.Username}");

        try
        {
            ServerConfig.DeleteAdmin(adminCredentials);
            _logger.LogInformation($"Admin deleted successfully: {adminCredentials.Username}");
            return Results.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while deleting admin: {adminCredentials.Username}");
            return Results.Problem(ex.Message);
        }
    }

    // TODO: add a user data retrieval endpoint
}
