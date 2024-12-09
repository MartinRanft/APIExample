using System.Data;

using ByteWizardApi.Interfaces.DB;
using ByteWizardApi.Model.Website;

using MySqlConnector;

namespace ByteWizardApi.Functions.Website.User
{
    /// <summary>
    /// The UserHighscore class is responsible for generating a highscore list for a user,
    /// utilizing data retrieved from a database.
    /// </summary>
    public class UserHighscore(IDatabaseService database)
    {
        /// <summary>
        /// Represents a database service instance used for executing database operations,
        /// such as stored procedures and queries, within the UserHighscore class.
        /// This instance is primarily utilized to interface with the database
        /// and retrieve or manipulate data needed to generate a user's highscore list.
        /// </summary>
        private readonly IDatabaseService _databaseService = database;
        
        /// <summary>
        /// Generates a list of high scores for a user based on their data.
        /// Retrieves the highscore details for the specified user and
        /// returns it as a list of Highscore objects.
        /// </summary>
        /// <param name="data">The User object containing the user's data for which the high scores are to be retrieved.</param>
        /// <returns>A List of Highscore objects containing the high score details for the specified user, or null if no data is retrieved.</returns>
        public List<Highscore>? GenerateStatistik(Model.Website.User data)
        {
            MySqlParameter[] param =
            [
                new MySqlParameter("user_id", data.ID)
            ];

            DataTable resulTable = _databaseService.ExecuteStoredProcedure("GetUserHighscore", param);

            List<Highscore> highscore = [];
            highscore.AddRange(from DataRow row in resulTable.Rows
                               select new Highscore
                               {
                                   InternID = (uint)row["ID"],
                                   DiscordUserID = Convert.ToUInt32(row["UserID"]),
                                   Avatar = (byte[])row["DiscordAvatar"],
                                   InternServerID = (uint)row["DiscordServer"],
                                   WordCount = (int)row["WordCount"],
                                   PicCount = (int)row["PicCount"],
                                   UrlCount = (int)row["UrlCount"],
                                   Birthday = row["Birthday"] != DBNull.Value ? row["Birthday"] as DateTime? : null,
                                   UserName = (string)row["UserName"],
                                   ServerName = (string)row["ServerName"]
                               });

            return highscore;
        }
    }
}