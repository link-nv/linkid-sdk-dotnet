using System;
using System.Collections.Generic;
using System.Text;

namespace safe_online_sdk_dotnet
{
    public class LinkIDWalletTransaction
    {
        public String walletId { get; set; }
        public DateTime creationDate { get; set; }
        public String transactionId { get; set; }
        public double amount { get; set; }
        public Nullable<LinkIDCurrency> currency { get; set; }
        public String walletCoin { get; set; }

        public LinkIDWalletTransaction(String walletId, DateTime creationDate, String transactionId,
            double amount, Nullable<LinkIDCurrency> currency, String walletCoin)
        {
            this.walletId = walletId;
            this.creationDate = creationDate;
            this.transactionId = transactionId;
            this.amount = amount;
            this.currency = currency;
            this.walletCoin = walletCoin;
        }

        public override String ToString()
        {
            String output = "";

            output += "    Wallet transaction: \n";
            if (null != currency)
            {
                output += "      * amount: " + amount + currency.Value + "\n";
            }
            else
            {
                output += "      * amount: " + amount + walletCoin + "\n";
            }
            output += "      * walletId: " + walletId + "\n";
            output += "      * creationDate: " + creationDate + "\n";
            output += "      * transactionId: " + transactionId + "\n";

            return output;
        }

    }
}
