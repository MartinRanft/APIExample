namespace ByteWizardApi.Model.Website
{
#pragma warning disable

    /// <summary>
    /// Represents news related to a website, containing details such as
    /// news topic, news content, display name, and timestamp of when
    /// the news was created or posted.
    /// </summary>
    internal class WebsiteNews
    {
        public int? ID { get; set; }
        public string WebsiteTehma { get; set; }
        public string WebsiteText { get; set; }
        public string DisplayName { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    /// <summary>
    /// Represents the data structure used to add news content to a website,
    /// including fields such as the news topic, text, and associated user information.
    /// </summary>
    public class AddWebsiteNews
    {
        public int? ID { get; set; }
        public string website_tehma { get; set; }
        public string website_text { get; set; }
        public int website_user { get; set; }
    }

    /// <summary>
    /// Represents a request to add news to a website, encapsulating both the
    /// news content and the user information involved in the request.
    /// </summary>
    public class AddNewsRequest
    {
        public AddWebsiteNews News { get; set; }
        public User User { get; set; }
    }

#pragma warning restore IDE1006
}