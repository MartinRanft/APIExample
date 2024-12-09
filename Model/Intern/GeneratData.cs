using ByteWizardApi.Model.Intern.Checkpoints;

namespace ByteWizardApi.Model.Intern
{
    /// <summary>
    /// Represents the data required to generate images using AI models.
    /// Contains user settings and prompts that influence the image generation process.
    /// </summary>
    public sealed class GeneratData
    {
        /// <summary>
        /// Gets or sets the user-specific settings for AI image generation.
        /// These settings include parameters such as user ID, model configuration,
        /// and various weights that influence the generated image output.
        /// </summary>
        public UserAiSettings UserSettings { get; set; }


        /// <summary>
        /// Gets or sets the positive prompt used in AI image generation.
        /// This prompt guides the AI model towards desired features and characteristics
        /// to include in the generated image, influencing its overall appearance and content.
        /// </summary>
        public string PositivePrompt { get; set; }


        /// <summary>
        /// Gets or sets the negative prompt used in AI image generation.
        /// The negative prompt specifies the elements or features that should be minimized
        /// or excluded in the generated image, providing more control over the output aesthetics.
        /// </summary>
        public string NegativePrompt { get; set; }
    }


    /// <summary>
    /// Represents the AI settings configured by a user for image generation.
    /// Includes various parameters such as Lora stack selections and their respective strengths,
    /// model choice, and configuration settings that tailor the generation process.
    /// </summary>
    public sealed class UserAiSettings
    {
        /// <summary>
        /// Gets or sets the unique identifier for a user.
        /// This ID is used to associate the AI settings with a specific user
        /// and ensure the correct configuration and model settings are applied
        /// during the image generation process.
        /// </summary>
        public ulong? UserId { get; set; }


        /// <summary>
        /// Gets or sets the first Lora configuration setting for AI operation.
        /// This setting defines specific parameters for integration with LoraStack,
        /// affecting the performance and output of the artificial intelligence processes.
        /// </summary>
        public BildConfigEnums.LoraStack Lora1 { get; set; }


        /// <summary>
        /// Gets or sets the first strength parameter for the AI settings used in image generation.
        /// This value determines the weight applied to the first LoraStack model within the AI workflow.
        /// </summary>
        public double? Strength1 { get; set; }


        /// <summary>
        /// Gets or sets the secondary LoraStack configuration for AI processing settings.
        /// This property determines additional stack parameters that may affect the output
        /// of AI-driven tasks within the application.
        /// </summary>
        public BildConfigEnums.LoraStack Lora2 { get; set; }


        /// <summary>
        /// Gets or sets the strength level for the second Lora model within the user's AI settings.
        /// This value represents the weight or influence that the second Lora model has during
        /// the image generation process.
        /// </summary>
        public double? Strength2 { get; set; }


        /// <summary>
        /// Gets or sets the third LORA stacking configuration used in the AI model.
        /// This configuration influences the model's processing behavior and output
        /// by defining specific parameters for LORA integration in a stack sequence.
        /// </summary>
        public BildConfigEnums.LoraStack Lora3 { get; set; }


        /// <summary>
        /// Gets or sets the third strength parameter used in AI image generation.
        /// This parameter influences the weighting of the third Lora model
        /// within the image generation workflow.
        /// </summary>
        public double? Strength3 { get; set; }


        /// <summary>
        /// Gets or sets the fourth level of Lora stack configuration.
        /// This property defines specific parameters that affect the behavior and performance
        /// of the AI model within the Lora4 context.
        /// </summary>
        public BildConfigEnums.LoraStack Lora4 { get; set; }


        /// <summary>
        /// Gets or sets the strength value associated with the fourth Lora model in the AI generation process.
        /// This parameter determines the weight or influence of the fourth Lora model on the output.
        /// </summary>
        public double? Strength4 { get; set; }


        /// <summary>
        /// Gets or sets the fifth model configuration option within the LoRA (Low-Rank Adaptation) stack.
        /// This property allows fine-tuning and selection of specific settings for AI-related image processing tasks,
        /// enhancing the customization and output control of the AI model.
        /// </summary>
        public BildConfigEnums.LoraStack Lora5 { get; set; }


        /// <summary>
        /// Gets or sets the weight for the fifth LoRA model in the AI image generation process.
        /// Affects how strongly this model influences the final output image.
        /// </summary>
        public double? Strength5 { get; set; }


        /// <summary>
        /// Gets or sets the AI model configuration used for generating images.
        /// This setting determines the specific model and its associated parameters
        /// that influence the output of the image generation process.
        /// </summary>
        public BildConfigEnums.Modelle Model { get; set; }


        /// <summary>
        /// Gets or sets the configuration strength utilized in the image generation process.
        /// This value modifies the influence of configurations during the creation of the image,
        /// impacting the output based on pre-defined settings.
        /// </summary>
        public double? Cfg { get; set; }


        /// <summary>
        /// Gets or sets the date and time when the user AI settings were loaded.
        /// This property provides a timestamp that indicates when the current configuration
        /// was retrieved or initialized, aiding in tracking and managing configuration updates.
        /// </summary>
        public DateTime? LoadedDateTime { get; set; }
    }
}