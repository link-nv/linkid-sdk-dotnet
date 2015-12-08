using System;

namespace safe_online_sdk_dotnet
{
    public class LinkIDReportWalletFilter
    {
        public String walletId { get; set; }

        public LinkIDReportWalletFilter(String walletId)
        {
            this.walletId = walletId;
        }
    }
}
