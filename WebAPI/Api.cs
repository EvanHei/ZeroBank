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
            // TODO: call EncryptionParameters.Save(stream);
            return Results.Stream(new MemoryStream());
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
            // TODO: write relinKeys to a file
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
            // TODO: call context.Request.Body.CopyToAsync(FileStream);
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
            // TODO: sum all ciphertexts (from the blockchain) and call ciphertextSum.Save(stream);
            return Results.Stream(new MemoryStream());
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}
