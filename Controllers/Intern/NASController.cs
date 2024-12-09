using ByteWizardApi.ApiAuth;
using ByteWizardApi.Functions.Intern;
using ByteWizardApi.Model.Intern;

using Microsoft.AspNetCore.Mvc;

namespace ByteWizardApi.Controllers.Intern;

/// <summary>
/// Controller for handling NAS-related operations within the ByteWizard internal API.
/// </summary>
/// <remarks>
/// This controller provides endpoints to check the NAS API status and to retrieve picture lists and files.
/// Access to the endpoints may require authentication through an API key.
/// </remarks>
[Route("api/nas")]
[ApiController]
[ApiExplorerSettings(GroupName = "internal")]
public class NasController(IConfiguration configuration) : ControllerBase
{
    
    /// <summary>
    /// Represents the configuration settings for the NASController.
    /// </summary>
    /// <remarks>
    /// This IConfiguration instance is used to access configuration settings required for the NAS operations,
    /// such as connection strings or API keys. It is passed to dependent services such as FtpNas to facilitate
    /// NAS-related functionalities.
    /// </remarks>
    private readonly IConfiguration _configuration = configuration;

    /// <summary>
    /// Checks the status of the API and returns PONG if successful.
    /// </summary>
    /// <returns>Returns a string "PONG" if the API is successful, or a problem detail if there's an issue.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("status")]
    public IActionResult Status()
    {
        const string host = @"http://********/api/v2.0/core/ping";
        const string apiKey = @"***************";
        using HttpClient client = new();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        HttpResponseMessage response = client.GetAsync(host).GetAwaiter().GetResult();

        if(response.IsSuccessStatusCode)
        {
            string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return Ok(responseBody);
        }
        else
        {
            return Problem(response.StatusCode.ToString());
        }
    }

    /// <summary>
    /// Gets a list of pictures using the API key for authentication.
    /// </summary>
    /// <remarks>
    /// This endpoint allows you to retrieve a list of picture names using an API key for authentication.
    /// </remarks>
    /// <returns>
    /// Returns a list of picture names if found.
    /// If no pictures are found, it returns a "Not Found" response with the message "No Pics found."
    /// If there's an issue with the authentication, it may return an "Unauthorized" response.
    /// </returns>
    [ApiKeyAuth]
    [HttpGet("pics")]
    [ProducesResponseType(200, Type = typeof(List<string>))]
    [ProducesResponseType(404, Type = typeof(string))]
    [ProducesResponseType(401)]
    public IActionResult GetPicList()
    {
        FtpNas ftp = new(_configuration);
        List<string> result = ftp.GetPicList();

        if(result.Count > 0)
        {
            return Ok(result);
        }
        return NotFound("No Pics found");
    }


    /// <summary>
    /// Retrieves a list of picture files from the NAS based on the provided list of picture names.
    /// </summary>
    /// <param name="Piclist">A list of picture names to retrieve from the NAS.</param>
    /// <returns>Returns a list of <c>FtpDataModel</c> objects representing the picture files if found; otherwise, returns a 404 status code with a "No Files Found" message.</returns>
    [ApiKeyAuth]
    [HttpPost("getFiles")]
    [ProducesResponseType(200, Type = typeof(List<FtpDataModel>))]
    [ProducesResponseType(404, Type = typeof(string))]
    [Consumes("application/json")]
    public IActionResult GetPicture(List<string> Piclist)
    {
        FtpNas ftp = new(_configuration);
        List<FtpDataModel> result = ftp.GetFilel(Piclist);

        if(result.Count > 0)
        {
            return Ok(result);
        }
        return NotFound("No Files Found");
    }
}