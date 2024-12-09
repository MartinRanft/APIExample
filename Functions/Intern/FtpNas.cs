using ByteWizardApi.Model.Intern;

using FluentFTP;

namespace ByteWizardApi.Functions.Intern
{
    /// <summary>
    /// Represents a class that manages interactions with an FTP server to retrieve lists of file names
    /// and download files as per the requests.
    /// </summary>
    /// <remarks>
    /// The FtpNas class is utilized to facilitate communication with a secure FTP server. It includes
    /// methods to fetch file listings and download files in binary format.
    /// </remarks>
    public sealed class FtpNas(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;


        /// <summary>
        /// Retrieves a list of file names from an FTP server that match image file extensions such as .jpg, .png, or .gif.
        /// </summary>
        /// <returns>
        /// A list of strings containing the names of the image files found on the FTP server.
        /// If the server fails to authenticate or no image files are found, an empty list is returned.
        /// </returns>
        internal List<string> GetPicList()
        {
            List<string> result = [];

            using FtpClient conn = new FtpClient();
            conn.Host = _configuration["FtpData:Host"];
            conn.Credentials = new System.Net.NetworkCredential(_configuration["FtpData:Username"], _configuration["FtpData:Password"]);
            conn.Connect();

            if(conn.IsAuthenticated)
            {
                foreach(FtpListItem? item in conn.GetListing())
                {
                    if(item.Name.EndsWith(".jpg") || item.Name.EndsWith(".png") || item.Name.EndsWith(".gif"))
                    {
                        result.Add(item.Name);
                    }
                }
                conn.Disconnect();
                return result;
            }
            return result;
        }


        /// <summary>
        /// Downloads files from an FTP server based on a provided list of file names and returns their data models.
        /// </summary>
        /// <param name="picList">A list of file names to be downloaded from the FTP server.</param>
        /// <returns>A list of FtpDataModel objects containing the file names and their binary data.
        /// If the server fails to authenticate or no files are found, an empty list is returned.</returns>
        internal List<FtpDataModel> GetFilel(List<string> picList)
        {
            List<FtpDataModel> result = [];

            using FtpClient conn = new FtpClient();
            conn.Host = _configuration["FtpData:Host"];
            conn.Credentials = new System.Net.NetworkCredential(_configuration["FtpData:Username"], _configuration["FtpData:Password"]);
            conn.Connect();

            if(conn.IsAuthenticated)
            {
                foreach(string filename in picList)
                {
                    using MemoryStream stream = new();
                    if(conn.DownloadStream(stream, filename))
                    {
                        result.Add(new FtpDataModel
                        {
                            FileName = filename,
                            Data = stream.ToArray()
                        });
                    }
                }
                conn.Disconnect();
                return result;
            }
            return result;
        }
    }
}