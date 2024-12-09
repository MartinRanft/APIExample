namespace ByteWizardApi.Model.Intern
{
    /// <summary>
    /// Represents data associated with a token used for image generation.
    /// </summary>
    /// <remarks>
    /// Contains necessary information such as user identity and prompts required for generating images.
    /// Primarily used as an input parameter in image generation processes.
    /// </remarks>
    public class TokenData
    {
        /// <summary>
        /// Gets or sets the unique identifier for a user.
        /// </summary>
        /// <remarks>
        /// This property is used to identify the user associated with a particular token
        /// during the image generation process. A null or zero value indicates that no
        /// valid user ID has been provided, which can result in a failure to generate images.
        /// </remarks>
        public ulong? UserId { get; set; } = null!;


        /// <summary>
        /// Gets or sets the positive prompt string used for image generation.
        /// </summary>
        /// <remarks>
        /// The positive prompt is a textual input that guides the image generation process
        /// by specifying desired features or elements to be included in the generated images.
        /// </remarks>
        public string PositivePrompt { get; set; } = null!;
    }
}
