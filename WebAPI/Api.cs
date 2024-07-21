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

    static IResult PostRelinKeys()
    {
        try
        {
            // TODO: implement PostRelinKeys. write relinKeys to a file
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    static IResult PostTransaction()
    {
        try
        {
            // TODO: implement PostTransaction. call context.Request.Body.CopyToAsync(FileStream);
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
                return Results.Stream(new MemoryStream());
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
