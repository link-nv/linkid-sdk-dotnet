using System;
using System.Collections.Generic;

namespace safe_online_sdk_dotnet
{
    public class LinkIDParkingReport
    {
        public long total { get; set; }
        public List<LinkIDParkingSession> parkingSessions { get; set; }

        public LinkIDParkingReport(long total, List<LinkIDParkingSession> parkingSessions)
        {
            this.total = total;
            this.parkingSessions = parkingSessions;
        }
    }
}
