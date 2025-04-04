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
    private readonly ILogger<UserController> _logger;
    public AdminController(IConfiguration configuration, ILogger<UserController> logger)
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

    [HttpGet("admins")]
    [Authorize(Roles = "Admin")]
    public IResult GetAdmins()
    {
        _logger.LogInformation("GetAdmins method called");

        try
        {
            List<User> admins = ServerConfig.DataAccessor.LoadAdmins();
            _logger.LogInformation("Successfully retrieved admins");
            return Results.Ok(admins);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving admins");
            return Results.Problem(ex.Message);
        }
    }

    [HttpGet("accounts")]
    [Authorize(Roles = "Admin")]
    public IResult GetAccounts()
    {
        _logger.LogInformation("GetAccounts method called");

        try
        {
            List<Account> accounts = ServerConfig.DataAccessor.LoadAllAccounts();
            _logger.LogInformation("Successfully retrieved accounts");
            return Results.Ok(accounts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving accounts");
            return Results.Problem(ex.Message);
        }
    }

    [HttpGet("users")]
    [Authorize(Roles = "Admin")]
    public IResult GetUsers()
    {
        _logger.LogInformation("GetUsers method called");

        try
        {
            List<User> users = ServerConfig.DataAccessor.LoadUsers();
            _logger.LogInformation("Successfully retrieved users");
            return Results.Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving users");
            return Results.Problem(ex.Message);
        }
    }

    [HttpGet("keys")]
    [Authorize(Roles = "Admin")]
    public IResult GetKeys()
    {
        _logger.LogInformation("GetKeys method called");

        try
        {
            Dictionary<int, string> keys = ServerConfig.DataAccessor.LoadUserPrivateKeys();
            _logger.LogInformation("Successfully retrieved keys");
            return Results.Ok(keys);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving keys");
            return Results.Problem(ex.Message);
        }
    }
}
