using System;

namespace safe_online_sdk_dotnet
{
    public class LinkIDWalletReportInfo
    {
        public String reference { get; set; }
        public String description { get; set; }

        public LinkIDWalletReportInfo(String reference, String description)
        {
            this.reference = reference;
            this.description = description;
        }
    }
}
