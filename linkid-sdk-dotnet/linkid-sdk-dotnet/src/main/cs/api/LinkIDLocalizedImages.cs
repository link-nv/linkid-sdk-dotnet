using System;
using System.Collections.Generic;

namespace safe_online_sdk_dotnet
{
    public class LinkIDLocalizedImages
    {
        public Dictionary<String, LinkIDLocalizedImage> imageMap { get; set; }

        public LinkIDLocalizedImages(Dictionary<String, LinkIDLocalizedImage> imageMap)
        {
            this.imageMap = imageMap;
        }
    }
}
