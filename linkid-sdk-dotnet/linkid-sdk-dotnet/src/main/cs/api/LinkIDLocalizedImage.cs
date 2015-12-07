using System;

namespace safe_online_sdk_dotnet
{
    public class LinkIDLocalizedImage
    {
        public String url { get; set; }
        public String language { get; set; }

        public LinkIDLocalizedImage(String url, String language)
        {
            this.url = url;
            this.language = language;
        }
    }
}
