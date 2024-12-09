using ByteWizardApi.Functions.Intern;
using ByteWizardApi.Model.Intern;

using Microsoft.AspNetCore.Mvc;

namespace ByteWizardApi.Controllers.Intern
{
    /// <summary>
    /// This controller handles the asynchronous image generation requests.
    /// It provides endpoints for generating images and VTT tokens based on provided data.
    /// </summary>
    [Route("api/bildgen")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "internal")]
    public class BildGenerierung() : Controller
    {
        /// <summary>
        /// Asynchronously generates images based on provided generation data.
        /// </summary>
        /// <param name="data">The data containing user settings and preferences for image generation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IActionResult, returning a list of base64 encoded images if successful; otherwise, a status code indicating the error.</returns>
        /// <response code="200">Returns a list of base64 encoded images.</response>
        /// <response code="401">Occurs when the user ID is null or 0, indicating unauthorized access due to invalid user identification.</response>
        /// <response code="503">Occurs if the image generation fails, indicating the service is unavailable.</response>
        [HttpPost("GeneratePic")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<byte[]>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GeneratePicAsync(GeneratData data)
        {
            if(data.UserSettings.UserId is null or 0)
            {
                return StatusCode(401);
            }

            AiPicGen aiGenerator = new();
            List<byte[]>? result = await aiGenerator.GeneratePicAsync(data);

            if(result is null || result.Count == 0)
            {
                return StatusCode(503); 
            }

            List<string> base64Images = result.Select(bytes => Convert.ToBase64String(bytes)).ToList();

            return Ok(base64Images);
        }

        /// <summary>
        /// Generates token images based on the provided token data.
        /// </summary>
        /// <param name="data">The token data used for image generation.</param>
        /// <returns>A list of base64 encoded images if successful; otherwise, a status code indicating the error.</returns>
        /// <response code="200">Returns a list of base64 encoded images.</response>
        /// <response code="503">Occurs when the user ID is null or 0, or if the image generation fails.</response>
        [HttpPost("GenerateVTTToken")]
        [ProducesResponseType(typeof(List<string>), 200)]
        [ProducesResponseType(503)]
        public async Task<IActionResult> TokenCreature(TokenData data)
        {
            if(data.UserId is null or 0)
            {
                return StatusCode(503);
            }
            
            AiPicGen aiGenerator = new();
            List<byte[]>? result = await aiGenerator.TokenGenerator(data);

            if(result is null || result.Count == 0)
            {
                return StatusCode(503);
            }

            List<string> based64Images = result.Select(bytes => Convert.ToBase64String(bytes)).ToList();
            return Ok(based64Images);
        }
    }
}