namespace ByteWizardApi.Controllers.Intern
{
    /// <summary>
    /// Represents a custom attribute to provide metadata description for a field.
    /// </summary>
    /// <remarks>
    /// This attribute is designed for internal use within the ByteWizardApi.Controllers.Intern namespace.
    /// It is sealed, meaning it cannot be inherited by other classes.
    /// It can only be applied to fields, ensuring that additional metadata in the form of a string description
    /// is provided directly at the field level.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    internal sealed class AiAttribut(string beschreibung) : Attribute
    {
        /// <summary>
        /// Gets the description associated with this attribute.
        /// </summary>
        /// <value>
        /// A string containing the metadata description provided for the field.
        /// </value>
        public string Beschreibung { get; } = beschreibung;
    }
}