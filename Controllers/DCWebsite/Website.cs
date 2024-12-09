using System.Data;

using ByteWizardApi.ApiAuth;
using ByteWizardApi.Functions.Website.User;
using ByteWizardApi.Interfaces.DB;
using ByteWizardApi.Model.Website;

using Microsoft.AspNetCore.Mvc;

using MySqlConnector;

namespace ByteWizardApi.Controllers.DCWebsite
{
    /// <summary>
    /// The Website class handles API endpoints for operations related to a web-based service,
    /// including user authentication, highscore retrieval, and news management functionalities.
    /// </summary>
    [Route("api/Website")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Discord Website")]
    public class Website(IDatabaseService databaseService) : ControllerBase
    {
        /// <summary>
        /// Represents a service for database operations used within the Website controller.
        /// It facilitates interaction with the database for various functions such as user authentication,
        /// retrieving highscores, and fetching website news.
        /// </summary>
        private readonly IDatabaseService _databaseService = databaseService;

        /// <summary>
        /// Authenticates a user based on the provided login data.
        /// </summary>
        /// <param name="dataModel">The login data model containing the username and password.</param>
        /// <returns>Returns an <see cref="IActionResult"/> indicating the outcome of the authentication. If successful, returns a 200 OK status with the authenticated user data; otherwise, returns a 401 Unauthorized status.</returns>
        [ApiKeyAuth]
        [HttpPost("LoginData")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Consumes("application/json")]
        public IActionResult AuthUser(LoginModel dataModel)
        {
            if(string.IsNullOrWhiteSpace(dataModel.Username) || string.IsNullOrWhiteSpace(dataModel.Password))
            {
                return Unauthorized();
            }

            AuthenticateUser auth = new(_databaseService);

            User? userData = auth.AuthenticateWebsiteUser(dataModel);

            if(userData is null)
            {
                return Unauthorized();
            }

            return Ok(userData);
        }

        /// <summary>
        /// Retrieves the high score data for a specified user.
        /// </summary>
        /// <param name="model">The user model containing the user's information for which high scores are to be retrieved.</param>
        /// <returns>Returns an <see cref="IActionResult"/> containing a list of <see cref="Highscore"/> if the user is authenticated successfully. If authentication fails or no data is available, returns a 401 Unauthorized status.</returns>
        [ApiKeyAuth]
        [HttpPost("Highscore")]
        [ProducesResponseType(200, Type = typeof(List<Highscore>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Consumes("application/json")]
        public IActionResult GetHighscore(User? model)
        {
            if(model is null)
            {
                return Unauthorized();
            }

            AuthenticateUser auth = new(_databaseService);
            UserHighscore highscore = new(_databaseService);

            if(!auth.AuthWithUser(model.UserID))
            {
                return Unauthorized();
            }

            List<Highscore>? hs = highscore.GenerateStatistik(model);

            if(hs is null)
            {
                return Unauthorized();
            }

            return Ok(hs);
        }

        /// <summary>
        /// Retrieves a list of news entries from the website's news view.
        /// </summary>
        /// <returns>Returns an <see cref="IActionResult"/> containing a list of <see cref="WebsiteNews"/> objects if successful. The response includes a 200 OK status with the news data.</returns>
        [HttpGet("GetNews")]
        [ProducesResponseType(200, Type = typeof(List<WebsiteNews>))]
        [Consumes("application/json")]
        public IActionResult GetNews()
        {
            DataTable table = _databaseService.ExecuteSqlQuery("SELECT * FROM `website_news_view`", []);

            List<WebsiteNews> NewsData = [];
            NewsData.AddRange(from DataRow row in table.Rows
                              select new WebsiteNews()
                              {
                                  ID = (int)row["ID"],
                                  WebsiteTehma = (string)row["website_tehma"],
                                  WebsiteText = (string)row["website_text"],
                                  DisplayName = (string)row["display_name"],
                                  TimeStamp = (DateTime)row["create_time"]
                              });
            return Ok(NewsData);
        }

        /// <summary>
        /// Adds a news item to the website using the provided request data.
        /// </summary>
        /// <param name="AddNewsRequest">The request object containing the news details and user information.</param>
        /// <returns>Returns an <see cref="IActionResult"/> indicating the status of the operation. A 200 OK status is returned if the news item is successfully added; otherwise, a relevant error status code is returned depending on the failure reason, such as 400 Bad Request, 401 Unauthorized, or 500 Internal Server Error.</returns>
        [ApiKeyAuth]
        [HttpPost("AddNews")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddNews(AddNewsRequest AddNewsRequest)
        {
            AuthenticateUser auth = new(_databaseService);

            if(!auth.AuthWithUser(AddNewsRequest.User.UserID))
            {
                return Unauthorized();
            }

            MySqlParameter[] parameters =
            [
                new MySqlParameter("newsID", DBNull.Value),
                new MySqlParameter("headline", AddNewsRequest.News.website_tehma),
                new MySqlParameter("message", AddNewsRequest.News.website_text),
                new MySqlParameter("internID", AddNewsRequest.User.ID)
            ];
            _databaseService.ExecuteStoredProcedure("AddOrUpdateWebsiteNews", parameters);
            return Ok();
        }


        /// <summary>
        /// Updates or adds news information for the website.
        /// </summary>
        /// <param name="AddNewsRequest">The request model containing the news details and user information.</param>
        /// <returns>Returns an <see cref="IActionResult"/> indicating the result of the update operation. A 200 OK status is returned if the update is successful; otherwise, it may return a 400 Bad Request, 401 Unauthorized, or 500 Internal Server Error status depending on the failure condition.</returns>
        [ApiKeyAuth]
        [HttpPut("UpdateNews")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateNews(AddNewsRequest AddNewsRequest)
        {
            AuthenticateUser auth = new(_databaseService);

            if(!auth.AuthWithUser(AddNewsRequest.User.UserID) && AddNewsRequest.News.ID is null or 0)
            {
                return Unauthorized();
            }

            MySqlParameter[] parameters =
            [
                new MySqlParameter("newsID", AddNewsRequest.News.ID),
                new MySqlParameter("headline", AddNewsRequest.News.website_tehma),
                new MySqlParameter("message", AddNewsRequest.News.website_text),
                new MySqlParameter("internID", AddNewsRequest.User.ID)
            ];
            _databaseService.ExecuteStoredProcedure("AddOrUpdateWebsiteNews", parameters);
            return Ok();
        }


        /// <summary>
        /// Deletes a news item from the website database based on the given request.
        /// </summary>
        /// <param name="AddNewsRequest">The request containing the news ID to be deleted and the user information for authentication.</param>
        /// <returns>Returns an <see cref="IActionResult"/> indicating the result of the delete operation. If successful, returns a 200 OK status; if the user is unauthorized, returns a 401 Unauthorized status; if the request is invalid, returns a 400 Bad Request status; for server errors, returns a 500 Internal Server Error status.</returns>
        [ApiKeyAuth]
        [HttpDelete("DeleteNews")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteNews(AddNewsRequest AddNewsRequest)
        {
            AuthenticateUser auth = new(_databaseService);

            if(!auth.AuthWithUser(AddNewsRequest.User.UserID) && AddNewsRequest.News.ID is null or 0)
            {
                return Unauthorized();
            }

            MySqlParameter[] parameters =
            [
                new MySqlParameter("newsID", AddNewsRequest.News.ID),
            ];
            _databaseService.ExecuteSqlQuery("DELETE FROM `website_news` WHERE ID = newsID", parameters);
            return Ok();
        }
    }
}