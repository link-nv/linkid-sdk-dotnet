using System;

namespace safe_online_sdk_dotnet
{
    public class LinkIDPaymentAmount
    {
        // amount to pay, carefull amount is in cents!!
        public Double amount { get; set; }
        public Nullable<LinkIDCurrency> currency { get; set; }
        public String walletCoin { get; set; }

        public LinkIDPaymentAmount(Double amount, Nullable<LinkIDCurrency> currency, String walletCoin)
        {
            this.amount = amount;
            this.currency = currency;
            this.walletCoin = walletCoin;
        }
    }
}
