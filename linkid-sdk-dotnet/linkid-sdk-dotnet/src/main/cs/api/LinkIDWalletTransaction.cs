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
        public LinkIDCurrency currency { get; set; }

        public LinkIDWalletTransaction(String walletId, DateTime creationDate, String transactionId,
            double amount, LinkIDCurrency currency)
        {
            this.walletId = walletId;
            this.creationDate = creationDate;
            this.transactionId = transactionId;
            this.amount = amount;
            this.currency = currency;
        }

        public override String ToString()
        {
            String output = "";

            output += "    Wallet transaction: \n";
            output += "      * amount: " + amount + currency + "\n";
            output += "      * walletId: " + walletId + "\n";
            output += "      * creationDate: " + creationDate + "\n";
            output += "      * transactionId: " + transactionId + "\n";

            return output;
        }

    }
}
