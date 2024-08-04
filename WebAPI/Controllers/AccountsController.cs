using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary;
using SharedLibrary;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly ILogger<AccountsController> _logger;

    public AccountsController(ILogger<AccountsController> logger)
    {
        _logger = logger;
    }

    private static System.Timers.Timer AccountCreationTimer;

    [HttpGet]
    [Authorize]
    public IResult GetAccounts()
    {
        _logger.LogInformation("GetAccounts method called");

        try
        {
            List<Account> accounts = ServerConfig.DataAccessor.LoadAccounts();
            _logger.LogInformation("Successfully retrieved accounts");
            return Results.Ok(accounts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving accounts");
            return Results.Problem(ex.Message);
        }
    }

    [HttpPost("partial-account")]
    [Authorize]
    public IResult PostPartialAccount([FromBody] Account account)
    {
        _logger.LogInformation($"PostPartialAccount method called");

        AccountCreationTimer = new(60000);
        AccountCreationTimer.Elapsed += (sender, e) =>
        {
            _logger.LogInformation($"PartialAccount creation timer elapsed for account ID: {account.Id}");
            AccountCreationTimer.Stop();
            AccountCreationTimer.Dispose();
            ServerConfig.DataAccessor.DeleteAccountById(account.Id);
        };
        AccountCreationTimer.AutoReset = false;
        AccountCreationTimer.Start();

        try
        {
            ServerConfig.CreateAccount(account);
            _logger.LogInformation($"Created partial account ID: {account.Id}");
            return Results.Ok(account);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while creating a partial account for account ID: {account.Id}");
            return Results.Problem(ex.Message);
        }
    }

    [HttpPost("full-account")]
    [Authorize]
    public IResult PostFullAccount([FromBody] Account account)
    {
        _logger.LogInformation($"PostFullAccount method called for account ID: {account.Id}");

        AccountCreationTimer.Stop();
        AccountCreationTimer.Dispose();
        try
        {
            ServerConfig.DataAccessor.SaveAccount(account);
            _logger.LogInformation($"Successfully saved full account ID: {account.Id}");
            return Results.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while saving a full account for account ID: {account.Id}");
            return Results.Problem(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public IResult DeleteAccountById(int id)
    {
        _logger.LogInformation($"DeleteAccountById method called for account ID: {id}");

        try
        {
            ServerConfig.DataAccessor.DeleteAccountById(id);
            _logger.LogInformation($"Successfully deleted account ID: {id}");
            return Results.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while deleting account ID: {id}");
            return Results.Problem(ex.Message);
        }
    }

    [HttpPost("{id}/transaction")]
    [Authorize]
    public IResult PostTransactionById(int id, [FromBody] Transaction transaction)
    {
        _logger.LogInformation($"PostTransactionById method called for account ID: {id}");

        try
        {
            ServerConfig.AddTransactionById(id, transaction);
            _logger.LogInformation($"Successfully added transaction for account ID: {id}");
            return Results.Ok(transaction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while adding a transaction for account ID: {id}");
            return Results.Problem(ex.Message);
        }
    }

    [HttpGet("{id}/balance")]
    [Authorize]
    public IResult GetBalanceById(int id)
    {
        _logger.LogInformation($"GetBalanceById method called for account ID: {id}");

        try
        {
            MemoryStream stream = ServerConfig.GetBalanceStreamById(id);
            _logger.LogInformation($"Successfully retrieved balance for account ID: {id}");
            return Results.Stream(stream);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while retrieving the balance for account ID: {id}");
            return Results.Problem(ex.Message);
        }
    }
}
