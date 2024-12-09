namespace ByteWizardApi.Model.Website
{
    /// <summary>
    /// Represents a model for capturing login credentials including username and password.
    /// </summary>
    public sealed class LoginModel
    {
        /// <summary>
        /// Gets or sets the username for the login credentials.
        /// </summary>
        /// <value>
        /// The username is a string that identifies the user attempting to log in.
        /// </value>
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the password for the login credentials.
        /// </summary>
        /// <value>
        /// The password is a sensitive string that authenticates the user attempting to log in.
        /// </value>
        public string? Password { get; set; }
    }
}