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
        app.MapPost("/relinkeys", PostRelinKeys);
        app.MapGet("/accounts", GetAccounts);
        app.MapPost("/accounts", PostAccount);
        app.MapDelete("/accounts/{id}", DeleteAccount);
        app.MapPost("/accounts/{id}/transaction", PostTransaction);
        app.MapGet("/accounts/{id}/balance", GetBalance);
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

    private static IResult PostRelinKeys(HttpContext context)
    {
        try
        {
            using MemoryStream stream = new();
            context.Request.Body.CopyToAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);
            ServerConfig.DataAccessor.SaveRelinKeys(stream);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static IResult PostTransaction(HttpContext context, int id)
    {
        try
        {
            using MemoryStream stream = new();
            context.Request.Body.CopyToAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);
            ServerConfig.DataAccessor.AddTransaction(stream, id);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static IResult GetBalance(int id)
    {
        try
        {
            using Ciphertext? balance = ServerConfig.EncryptionHelper.GetBalance(id);
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

    private static IResult GetAccounts()
    {
        try
        {
            List<AccountModel> accounts = ServerConfig.DataAccessor.LoadAccounts();
            return Results.Ok(accounts);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static IResult PostAccount(AccountModel account)
    {
        try
        {
            ServerConfig.DataAccessor.AddAccount(account);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static IResult DeleteAccount(int id)
    {
        try
        {
            ServerConfig.DataAccessor.DeleteAccount(id);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}
