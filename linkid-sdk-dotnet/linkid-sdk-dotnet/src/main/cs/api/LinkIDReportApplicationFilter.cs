using System;

namespace safe_online_sdk_dotnet
{
    public class LinkIDReportApplicationFilter
    {
        public String applicationName { get; set; }

        public LinkIDReportApplicationFilter(String applicationName)
        {
            this.applicationName = applicationName;
        }
    }
}
