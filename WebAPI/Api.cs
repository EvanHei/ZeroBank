using Microsoft.Research.SEAL;
using ServerLibrary;
using SharedLibrary;
using System.IO;

namespace WebAPI;

public static class Api
{
    public static void ConfigureApi(this WebApplication app)
    {
        app.MapGet("/parms", GetParms);
        app.MapGet("/accounts", GetAccounts);
        app.MapPost("/accounts", PostAccount);
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

    private static IResult PostAccount(Account account)
    {
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

    private static IResult PostTransactionById(Transaction transaction, int id)
    {
        try
        {
            ServerConfig.DataAccessor.AddTransactionById(transaction, id);
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
            using Ciphertext? balance = ServerConfig.EncryptionHelper.GetBalance(transactions, id, relinKeys);
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
