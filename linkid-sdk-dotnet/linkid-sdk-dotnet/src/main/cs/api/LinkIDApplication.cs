using System;

namespace safe_online_sdk_dotnet
{
    public class LinkIDApplication
    {
        public String name { get; set; }
        public String friendlyName { get; set; }

        public LinkIDApplication(String name, String friendlyName)
        {
            this.name = name;
            this.friendlyName = friendlyName;
        }

    }
}
