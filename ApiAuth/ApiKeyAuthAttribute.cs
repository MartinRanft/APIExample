using System.Data;

using ByteWizardApi.Interfaces.DB;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using MySqlConnector;

namespace ByteWizardApi.ApiAuth
{
    /// <summary>
    /// Represents an attribute used to enforce API key authentication on controllers or actions.
    /// </summary>
    /// <remarks>
    /// This attribute applies a filter that verifies the presence of a valid API key, ensuring that
    /// the specified endpoints are accessible only to authorized requests with the correct API key.
    /// </remarks>
    public class ApiKeyAuthAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Represents an attribute used to enforce API key authentication on controllers or actions.
        /// </summary>
        /// <remarks>
        /// This attribute applies a filter that verifies the presence of a valid API key, ensuring that
        /// the specified endpoints are accessible only to authorized requests with the correct API key.
        /// </remarks>
        public ApiKeyAuthAttribute() : base(typeof(ApiKeyAuthorizationFilter))
        {
        }
    }


    /// <summary>
    /// Implements the authorization filter to enforce API key validation for incoming requests.
    /// </summary>
    /// <remarks>
    /// This filter checks for the presence and validity of an API key in the request headers and denies
    /// access if the API key is missing or invalid. It relies on the provided IDatabaseService to verify
    /// the API key against stored values.
    /// </remarks>
    public class ApiKeyAuthorizationFilter(IDatabaseService databaseService) : IAuthorizationFilter
    {
        /// <summary>
        /// An instance of the <see cref="IDatabaseService"/> that provides database-related operations
        /// such as executing stored procedures, functions, and SQL queries.
        /// Utilized within the authorization filter to verify API keys against stored values.
        /// </summary>
        private readonly IDatabaseService _databaseService = databaseService;
        
        /// <summary>
        /// Handles the authorization logic to ensure that incoming requests contain a valid API key.
        /// </summary>
        /// <param name="context">The authorization filter context which contains information about the current request and allows manipulation of the HTTP response.</param>
        /// <remarks>
        /// This method checks the request headers for an API key, verifies its presence and validity, and prevents unauthorized access if the API key is found to be missing or invalid.
        /// </remarks>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string? apiKey = context.HttpContext.Request.Headers["ApiKey"];

            if(string.IsNullOrWhiteSpace(apiKey))
            {
                context.Result = new UnauthorizedResult();
            }

            if(IsValidApiKey(apiKey!))
            {
                return;
            }
            
            context.Result = new UnauthorizedResult();
        }

        /// <summary>
        /// Validates the provided API key against stored values in the database.
        /// </summary>
        /// <param name="apiKey">The API key to be validated.</param>
        /// <returns>
        /// Returns true if the API key is valid; otherwise, returns false.
        /// </returns>
        private bool IsValidApiKey(string apiKey)
        {
            MySqlParameter[] sqlParameters =
            [
                new MySqlParameter("@p_api_key", apiKey),
                new MySqlParameter("VerifyApiKey", MySqlDbType.Int32) { Direction = ParameterDirection.ReturnValue }
            ];

            object? apiCheck = _databaseService.ExecuteScalarFunction("VerifyApiKey", sqlParameters);

            if(apiCheck == null)
            {
                return false;
            }
            if(int.TryParse(apiCheck.ToString(), out int checkResult))
            {
                return checkResult == 1;
            }

            return false;
        }
    }
}