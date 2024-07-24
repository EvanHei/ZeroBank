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
            List<Blockchain> accounts = ServerConfig.DataAccessor.LoadAccounts();
            return Results.Ok(accounts);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static IResult PostAccount(GenesisBlockData genesisBlockData)
    {
        try
        {
            GenesisBlockData signedData = ServerConfig.DataAccessor.CreateAccount(genesisBlockData);
            return Results.Ok(signedData);
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

    private static IResult PostTransactionById(TransactionBlockData transactionBlockData, int id)
    {
        try
        {
            TransactionBlockData signedData = ServerConfig.DataAccessor.AddTransactionById(transactionBlockData, id);
            return Results.Ok(signedData);
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
            using Ciphertext? balance = ServerConfig.EncryptionHelper.GetBalance(transactions, id);
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
