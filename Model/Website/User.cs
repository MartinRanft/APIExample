namespace ByteWizardApi.Model.Website
{
        /// <summary>
        /// Represents a user within the ByteWizardApi system, encapsulating both personal
        /// and account pertinent information such as user IDs, names, and account status.
        /// </summary>
        public sealed class User
    {
            /// <summary>
            /// Gets or sets the unique identifier for the user.
            /// </summary>
            /// <remarks>
            /// This identifier is used to distinguish users within the system.
            /// </remarks>
            public UInt32 ID { get; set; }

            /// <summary>
            /// Gets or sets the UserID of the user.
            /// </summary>
            /// <remarks>
            /// The UserID serves as the primary key for identifying a user within the system's database.
            /// </remarks>
            public long UserID { get; set; }
        
#pragma warning disable IDE0079 // Unnötige Unterdrückung entfernen
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            /// <summary>
            /// Gets or sets the username associated with the user.
            /// </summary>
            /// <remarks>
            /// This property represents the unique username used for user identification and login.
            /// </remarks>
            public string UserName { get; set; }
#pragma warning restore IDE0079 // Unnötige Unterdrückung entfernen
#pragma warning disable IDE0079 // Unnötige Unterdrückung entfernen
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

#pragma warning disable IDE0079 // Unnötige Unterdrückung entfernen
#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
            /// <summary>
            /// Gets or sets the Discord username associated with the user.
            /// </summary>
            /// <remarks>
            /// This property contains the display name used by the user on Discord.
            /// </remarks>
            public string DiscordName { get; set; }
#pragma warning restore IDE0079 // Unnötige Unterdrückung entfernen
#pragma warning restore IDE0079 // Unnötige Unterdrückung entfernen
#pragma warning disable IDE0079 // Unnötige Unterdrückung entfernen
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.

#pragma warning disable IDE0079 // Unnötige Unterdrückung entfernen
#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        /// <summary>
        /// Gets or sets the avatar for the user on Discord.
        /// </summary>
        /// <remarks>
        /// This property holds the byte array representing the user's profile picture on Discord.
        /// </remarks>
            public byte[]? DiscordAvatar { get; set; }
#pragma warning restore IDE0079 // Unnötige Unterdrückung entfernen
#pragma warning restore IDE0079 // Unnötige Unterdrückung entfernen
#pragma warning disable IDE0079 // Unnötige Unterdrückung entfernen
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.

#pragma warning disable IDE0079 // Unnötige Unterdrückung entfernen
#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        public string Password { get; set; }
#pragma warning restore IDE0079 // Unnötige Unterdrückung entfernen
#pragma warning restore IDE0079 // Unnötige Unterdrückung entfernen
#pragma warning disable IDE0079 // Unnötige Unterdrückung entfernen
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.

        /// <summary>
        /// Gets or sets a value indicating whether the user has administrative privileges.
        /// </summary>
        /// <remarks>
        /// This property is used to determine if the user has access to administrative features and capabilities within the system.
        /// </remarks>
        public bool isAdmin { get; set; }

#pragma warning restore IDE0079 // Unnötige Unterdrückung entfernen

        /// <summary>
        /// Gets or sets a value indicating whether the user's account is active.
        /// </summary>
        /// <remarks>
        /// This property is used to determine if the user can access the system and utilize its features.
        /// </remarks>
        public bool isActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is a server moderator.
        /// </summary>
        /// <remarks>
        /// This property determines if the user has moderation privileges on the server within the system.
        /// </remarks>
        public bool IsServerMod { get; set; }

        /// <summary>
        /// Gets or sets the API user identifier.
        /// </summary>
        /// <remarks>
        /// This property is used to associate the user with their specific API credentials or profile within the
        /// ByteWizardApi system.
        /// </remarks>
        public UInt32 apiUser { get; set; }

        /// <summary>
        /// Gets or sets the creation date and time of the user.
        /// </summary>
        /// <remarks>
        /// This property indicates when the user was initially created in the system.
        /// </remarks>
        public DateTime created { get; set; }
    }
}