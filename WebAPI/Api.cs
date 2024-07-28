using Microsoft.Research.SEAL;
using ServerLibrary;
using SharedLibrary;
using System.IO;
using System.Security.Principal;
using System.Timers;

namespace WebAPI;

public static class Api
{
    private static System.Timers.Timer Timer;

    public static void ConfigureApi(this WebApplication app)
    {
        app.MapGet("/parms", GetParms);
        app.MapGet("/accounts", GetAccounts);
        app.MapPost("/accounts/partial-account", PostPartialAccount);
        app.MapPost("/accounts/full-account", PostFullAccount);
        app.MapDelete("/accounts/{id}", DeleteAccountById);
        app.MapPost("/accounts/{id}/transaction", PostTransactionById);
        app.MapGet("/accounts/{id}/balance", GetBalanceById);
    }

    private static IResult GetParms()
    {
        try
        {
            MemoryStream stream = new();
            ServerConfig.EncryptionHelper.Parms.Save(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return Results.Stream(stream);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static IResult GetAccounts()
    {
        try
        {
            List<Account> accounts = ServerConfig.DataAccessor.LoadAccounts();
            return Results.Ok(accounts);
        }
        catch (Exception ex)
        {
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
    private static IResult PostPartialAccount(Account account)
    {
        // 60 second timer will delete partial account after time has elapsed
        Timer = new(60000);
        ElapsedEventHandler handler = (sender, e) =>
        {
            Timer.Stop();
            Timer.Dispose();
            ServerConfig.DataAccessor.DeleteAccountById(account.Id);
        };
        Timer.Elapsed += handler;
        Timer.AutoReset = false;
        Timer.Start();

        try
        {
            ServerConfig.DataAccessor.CreateAccount(account);
            return Results.Ok(account);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static IResult PostFullAccount(Account account)
    {
        // stop the timer
        Timer.Stop();
        Timer.Dispose();
        try
        {
            ServerConfig.DataAccessor.SaveAccount(account);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static IResult DeleteAccountById(int id)
    {
        try
        {
            ServerConfig.DataAccessor.DeleteAccountById(id);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static IResult PostTransactionById(int id, Transaction transaction)
    {
        try
        {
            ServerConfig.DataAccessor.AddTransactionById(id, transaction);
            return Results.Ok(transaction);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static IResult GetBalanceById(int id)
    {
        try
        {
            List<Ciphertext> transactions = ServerConfig.DataAccessor.LoadTransactionsById(id);
            using RelinKeys? relinKeys = ServerConfig.DataAccessor.LoadRelinKeysById(id);
            using Ciphertext? balance = ServerConfig.EncryptionHelper.GetBalance(transactions, relinKeys);
            if (balance == null)
            {
                return Results.Problem("There are no transactions.");
            }

            MemoryStream stream = new();
            balance.Save(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return Results.Stream(stream);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}
