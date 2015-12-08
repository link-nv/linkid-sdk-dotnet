using System;

namespace safe_online_sdk_dotnet
{
    public class LinkIDReportPageFilter
    {
        public int firstResult { get; set; }
        public int maxResults { get; set; }

        public LinkIDReportPageFilter(int firstResult, int maxResults)
        {
            this.firstResult = firstResult;
            this.maxResults = maxResults;
        }
    }
}
