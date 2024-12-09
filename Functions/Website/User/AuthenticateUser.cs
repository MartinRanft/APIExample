using System.Data;

using ByteWizardApi.Interfaces.DB;
using ByteWizardApi.Model.Website;

using MySqlConnector;

namespace ByteWizardApi.Functions.Website.User
{
    /// <summary>
    /// Represents the functionality to authenticate users within the ByteWizard website system.
    /// </summary>
    internal sealed class AuthenticateUser(IDatabaseService database)
    {
        /// <summary>
        /// Service used to interact with the database for executing stored procedures,
        /// functions, and queries in the user authentication process within the
        /// ByteWizard website system.
        /// </summary>
        private readonly IDatabaseService _databaseService = database;


        /// <summary>
        /// Authenticates a website user based on their login details.
        /// </summary>
        /// <remarks>
        /// This method validates the provided username and password against stored credentials in the database.
        /// If the credentials are verified, it retrieves user details.
        /// </remarks>
        /// <param name="loginModel">The login model containing the user's credentials.</param>
        /// <returns>
        /// Returns a User object with the user's details if authentication is successful, otherwise null.
        /// </returns>
        public Model.Website.User? AuthenticateWebsiteUser(LoginModel loginModel)
        {
            MySqlParameter[] param =
            [
                new MySqlParameter("user_name", loginModel.Username)
            ];

            DataTable getHash = _databaseService.ExecuteStoredProcedure("GetUserPasswordByUsername", param);

            try
            {
                if(!BCrypt.Net.BCrypt.Verify(loginModel.Password, getHash.Rows[0]["Password"].ToString()))
                {
                    return null;
                }

                Array.Clear(param);
                param =
                [
                    new MySqlParameter("user_id", getHash.Rows[0]["UserID"].ToString())
                ];

                DataTable UserResult = _databaseService.ExecuteStoredProcedure("GetDiscordUserDetails", param);

                Model.Website.User UserData = new()
                {
                    ID = (UInt32)UserResult.Rows[0]["ID"],
                    UserID = (long)UserResult.Rows[0]["UserID"],
                    UserName = (string)UserResult.Rows[0]["UserName"],
                    DiscordName = (string)UserResult.Rows[0]["DiscordName"],
                    DiscordAvatar = (byte[])UserResult.Rows[0]["DiscordAvatar"],
                    Password = (string)UserResult.Rows[0]["Password"],
                    isAdmin = (bool)UserResult.Rows[0]["isAdmin"],
                    isActive = (bool)UserResult.Rows[0]["isActive"],
                    IsServerMod = (bool)UserResult.Rows[0]["IsServerMod"],
                    apiUser = (UInt32)UserResult.Rows[0]["apiUser"],
                    created = (DateTime)UserResult.Rows[0]["created"]
                };

                return UserData;
            }
            catch(Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Authenticates a user based on their Discord ID.
        /// </summary>
        /// <remarks>
        /// This method executes a stored procedure named 'GetDiscordUserDetails' with the given Discord ID
        /// as a parameter. It checks if the user exists in the database with that Discord ID.
        /// </remarks>
        /// <param name="discordId">The Discord ID of the user to authenticate.</param>
        /// <returns>
        /// Returns true if a user with the specified Discord ID exists, otherwise false.
        /// </returns>
        public bool AuthWithUser(long discordId)
        {
            MySqlParameter[] param =
            [
                new MySqlParameter("user_id", discordId)
            ];

            DataTable table = _databaseService.ExecuteStoredProcedure("GetDiscordUserDetails", param);

            return table.Rows.OfType<DataRow>().Any();
        }
    }
}