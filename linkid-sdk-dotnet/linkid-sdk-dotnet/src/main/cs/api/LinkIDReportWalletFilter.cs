using System;

namespace safe_online_sdk_dotnet
{
    public class LinkIDReportWalletFilter
    {
        public String walletId { get; set; }
        public String userId { get; set; }

        public LinkIDReportWalletFilter(String walletId, String userId)
        {
            this.walletId = walletId;
            this.userId = userId;
        }
    }
}
