using System;

namespace safe_online_sdk_dotnet
{
    public class LinkIDWalletInfo
    {
        public String walletId { get; set; }

        public LinkIDWalletInfo(String walletId)
        {
            this.walletId = walletId;
        }
    }
}
