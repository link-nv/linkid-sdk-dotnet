using System;

namespace safe_online_sdk_dotnet
{
    public class LinkIDReportDateFilter
    {
        public DateTime startDate { get; set; }
        public Nullable<DateTime> endDate { get; set; }

        public LinkIDReportDateFilter(DateTime startDate, Nullable<DateTime> endDate)
        {
            this.startDate = startDate;
            this.endDate = endDate;
        }
    }
}
