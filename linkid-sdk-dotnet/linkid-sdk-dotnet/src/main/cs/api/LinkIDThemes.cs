using System;
using System.Collections.Generic;

namespace safe_online_sdk_dotnet
{
    public class LinkIDThemes
    {
        public List<LinkIDTheme> themes { get; set; }

        public LinkIDThemes(List<LinkIDTheme> themes)
        {
            this.themes = themes;
        }
    }
}
