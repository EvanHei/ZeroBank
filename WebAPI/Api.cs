using Microsoft.Research.SEAL;
using ServerLibrary;
using System.IO;

namespace WebAPI;

public static class Api
{
    public static void ConfigureApi(this WebApplication app)
    {
        app.MapGet("/parms", GetParms);
        app.MapPost("/relinkeys", PostRelinKeys);
        app.MapPost("/transaction", PostTransaction);
        app.MapGet("/balance", GetBalance);
    }

    static IResult GetParms()
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

    static IResult PostRelinKeys(HttpContext context)
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

    static IResult PostTransaction(HttpContext context)
    {
        try
        {
            using MemoryStream stream = new();
            context.Request.Body.CopyToAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);
            ServerConfig.DataAccessor.SaveTransaction(stream);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    static IResult GetBalance()
    {
        try
        {
            Ciphertext? balance = ServerConfig.EncryptionHelper.GetBalance();
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
