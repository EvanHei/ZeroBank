using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary;
using SharedLibrary.Models;
using System.Security.Claims;

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

    [HttpGet]
    [Authorize]
    public IResult GetAccounts()
    {
        _logger.LogInformation("GetAccounts method called");

        try
        {
            string userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdClaim);
            List<Account> accounts = ServerConfig.DataAccessor.LoadUserAccounts(userId);
            _logger.LogInformation("Successfully retrieved accounts");
            return Results.Ok(accounts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving accounts");
            return Results.Problem(ex.Message);
        }
    }

    /* Steps in creating a new account:
     * 
     * 1. Client sends a partial account without the server's keys
     * 2. Server generates keys, and adds them to the account, and signs
     * 3. Client signs account and sends back full account
     * 
     * If the cleint does not complete step 3 within 60 seconds, the server will delete all resources associated with the partial account.
     */

    private static readonly Dictionary<int, (int AccountId, System.Timers.Timer Timer)> UserAccountStates = new();

    [HttpPost("partial-account")]
    [Authorize]
    public IResult PostPartialAccount([FromBody] Account account)
    {
        _logger.LogInformation($"PostPartialAccount method called");

        string userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = int.Parse(userIdClaim);

        // check if the user is already in the process of creating an account
        if (UserAccountStates.ContainsKey(userId))
        {
            return Results.BadRequest("You already have a pending account creation.");
        }

        // configure timer
        System.Timers.Timer accountCreationTimer = new(60000);
        accountCreationTimer.Elapsed += (sender, e) =>
        {
            _logger.LogInformation($"PartialAccount creation timer elapsed for account ID: {account.Id}");
            accountCreationTimer.Stop();
            accountCreationTimer.Dispose();
            UserAccountStates.Remove(userId);
            ServerConfig.DataAccessor.DeleteAccountById(account.Id);
        };
        accountCreationTimer.AutoReset = false;
        accountCreationTimer.Start();

        try
        {
            ServerConfig.CreatePartialAccount(account, userId);

            // store the account creation state
            UserAccountStates[userId] = (account.Id, accountCreationTimer);

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

        string userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = int.Parse(userIdClaim);

        // check if the user has a pending partial account creation
        if (!UserAccountStates.TryGetValue(userId, out var state) || state.AccountId != account.Id)
        {
            return Results.BadRequest("Cannot call /full-account before /partial-account or wrong account ID.");
        }

        try
        {
            // stop and dispose of the timer
            state.Timer.Stop();
            state.Timer.Dispose();
            UserAccountStates.Remove(userId);
            
            ServerConfig.CreateFullAccount(userId, account);
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
    public IResult Delete(int id)
    {
        _logger.LogInformation($"Delete method called for account ID: {id}");

        try
        {
            string userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdClaim);
            ServerConfig.DeleteAccount(id, userId);
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
    public IResult PostTransactionById([FromBody] CiphertextTransaction transaction)
    {
        _logger.LogInformation($"PostTransactionById method called for account ID: {transaction.AccountId}");

        try
        {
            string userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdClaim);
            ServerConfig.AddTransaction(userId, transaction);
            _logger.LogInformation($"Successfully added transaction for account ID: {transaction.AccountId}");
            return Results.Ok(transaction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while adding a transaction for account ID: {transaction.AccountId}");
            return Results.Problem(ex.Message);
        }
    }

    [HttpGet("{id}/balance")]
    [Authorize]
    public IResult GetBalance(int id)
    {
        _logger.LogInformation($"GetBalance method called for account ID: {id}");

        try
        {
            string userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdClaim);
            MemoryStream stream = ServerConfig.GetBalanceStream(id, userId);

            if (stream == null)
            {
                _logger.LogInformation($"No balance found for account ID: {id}");
                return Results.NoContent();
            }

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
