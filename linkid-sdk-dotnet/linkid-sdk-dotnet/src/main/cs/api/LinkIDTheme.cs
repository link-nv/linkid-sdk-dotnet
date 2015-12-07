using System;

namespace safe_online_sdk_dotnet
{
    public class LinkIDTheme
    {
        public String name { get; set; }
        public bool defaultTheme { get; set; }

        public LinkIDLocalizedImages logo { get; set; }
        public LinkIDLocalizedImages authLogo { get; set; }
        public LinkIDLocalizedImages background { get; set; }
        public LinkIDLocalizedImages tabletBackground { get; set; }
        public LinkIDLocalizedImages alternativeBackground { get; set; }

        public String backgroundColor { get; set; }
        public String textColor { get; set; }

        public LinkIDTheme(String name, bool defaultTheme, LinkIDLocalizedImages logo,
            LinkIDLocalizedImages authLogo, LinkIDLocalizedImages background,
            LinkIDLocalizedImages tabletBackground, LinkIDLocalizedImages alternativeBackground, 
            String backgroundColor, String textColor)
        {
            this.name = name;
            this.defaultTheme = defaultTheme;
            this.logo = logo;
            this.authLogo = authLogo;
            this.background = background;
            this.tabletBackground = tabletBackground;
            this.alternativeBackground = alternativeBackground;
            this.backgroundColor = backgroundColor;
            this.textColor = textColor;
        }
    }
}
