namespace ByteWizardApi.Model.Intern
{
#pragma warning disable


    /// <summary>
    /// Represents a data model for storing file information retrieved from an FTP server.
    /// </summary>
    internal sealed class FtpDataModel
    {
        /// <summary>
        /// Gets or sets the name of the file retrieved from the FTP server.
        /// </summary>
        public string FileName { get; set; }


        /// <summary>
        /// Gets or sets the binary data of the file retrieved from the FTP server.
        /// </summary>
        public byte[] Data { get; set; }
    }

#pragma warning restore
}