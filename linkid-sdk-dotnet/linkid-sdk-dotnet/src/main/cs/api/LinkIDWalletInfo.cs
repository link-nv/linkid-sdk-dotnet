using System;

namespace safe_online_sdk_dotnet
{
    public class LinkIDWalletInfo
    {
        public String walletId { get; set; }
        public double amount { get; set; }
        public LinkIDCurrency currency { get; set; }

        public LinkIDWalletInfo(String walletId, double amount, LinkIDCurrency currency)
        {
            this.walletId = walletId;
            this.amount = amount;
            this.currency = currency;
        }
    }
}
