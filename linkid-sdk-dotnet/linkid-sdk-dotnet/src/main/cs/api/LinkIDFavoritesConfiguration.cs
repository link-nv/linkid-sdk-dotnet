using System;

namespace safe_online_sdk_dotnet
{
    public class LinkIDFavoritesConfiguration
    {
        public String title { get; set; }
        public String info { get; set; }
        public String logoUrl { get; set; }
        public String backgroundColor { get; set; }
        public String textColor { get; set; }

        public LinkIDFavoritesConfiguration(String title, String info, String logoUrl,
            String backgroundColor, String textColor)
        {
            this.title = title;
            this.info = info;
            this.logoUrl = logoUrl;
            this.backgroundColor = backgroundColor;
            this.textColor = textColor;
        }
    }
}
