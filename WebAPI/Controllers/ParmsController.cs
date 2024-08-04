using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ServerLibrary;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ParmsController : ControllerBase
{
    private readonly ILogger<ParmsController> _logger;

    public ParmsController(ILogger<ParmsController> logger)
    {
        _logger = logger;
    }

    [Authorize]
    [HttpGet]
    public IResult GetParms()
    {
        _logger.LogInformation("GetParms method called");

        try
        {
            MemoryStream stream = new();
            ServerConfig.EncryptionHelper.Parms.Save(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return Results.Stream(stream);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving encryption parameters");
            return Results.Problem(ex.Message);
        }
    }
}
