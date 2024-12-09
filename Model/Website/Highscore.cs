namespace ByteWizardApi.Model.Website
{
#pragma warning disable
    
    /// <summary>
    /// Represents the highscore data for a user within the system.
    /// </summary>
    public class Highscore
    {
        
        /// <summary>
        /// Gets or sets the internal identifier for the highscore entry.
        /// This ID is unique within the system for each user's highscore record.
        /// </summary>
        public uint InternID { get; set; }


        /// <summary>
        /// Gets or sets the identifier for the user on Discord.
        /// This ID uniquely identifies a user within the Discord platform.
        /// </summary>
        public long DiscordUserID { get; set; }


        /// <summary>
        /// Gets or sets the avatar image data associated with the user.
        /// This is represented as a byte array and typically holds the user's profile picture.
        /// </summary>
        public byte[]? Avatar { get; set; }


        /// <summary>
        /// Gets or sets the internal server identifier associated with the highscore entry.
        /// This server ID uniquely identifies the server for a user's highscore within the system.
        /// </summary>
        public uint InternServerID { get; set; }


        /// <summary>
        /// Gets or sets the name of the server associated with the highscore entry.
        /// The server name is typically used to identify which server the highscore was achieved on.
        /// </summary>
        public string ServerName { get; set; }


        /// <summary>
        /// Gets or sets the total word count accumulated by the user within the highscore context.
        /// This represents the number of words associated with the user's activities or contributions.
        /// </summary>
        public int WordCount { get; set; }


        /// <summary>
        /// Gets or sets the count of pictures associated with the highscore entry.
        /// This property keeps track of the total number of images linked to the user's highscore data.
        /// </summary>
        public int PicCount { get; set; }


        /// <summary>
        /// Gets or sets the count of URLs associated with the highscore entry.
        /// It represents the number of URLs linked to the user's highscore data.
        /// </summary>
        public int UrlCount { get; set; }


        /// <summary>
        /// Gets or sets the birthday of the user associated with the highscore entry.
        /// This property is optional and may contain the date of birth if provided by the user.
        /// </summary>
        public DateTime? Birthday { get; set; }


        /// <summary>
        /// Gets or sets the username associated with the user's highscore.
        /// This property is a string representation of the user's display name
        /// within the system.
        /// </summary>
        public string UserName { get; set; }
    }

#pragma warning restore
}