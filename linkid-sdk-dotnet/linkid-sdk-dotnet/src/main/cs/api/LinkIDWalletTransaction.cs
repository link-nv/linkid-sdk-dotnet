using System;
using System.Collections.Generic;
using System.Text;

namespace safe_online_sdk_dotnet
{
    public class LinkIDWalletTransaction
    {
        public String walletId { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime refundedDate { get; set; }
        public DateTime committedDate { get; set; }
        public String transactionId { get; set; }
        public double amount { get; set; }
        public Nullable<LinkIDCurrency> currency { get; set; }
        public String walletCoin { get; set; }
        public double refundAmount { get; set; }
        public String paymentDescription { get; set; }

        public LinkIDWalletTransaction(String walletId, DateTime creationDate, DateTime refundedDate,
            DateTime committedDate, String transactionId,
            double amount, Nullable<LinkIDCurrency> currency, String walletCoin, 
            double refundAmount, String paymentDescription)
        {
            this.walletId = walletId;
            this.creationDate = creationDate;
            this.refundedDate = refundedDate;
            this.committedDate = committedDate;
            this.transactionId = transactionId;
            this.amount = amount;
            this.currency = currency;
            this.walletCoin = walletCoin;
            this.refundAmount = refundAmount;
            this.paymentDescription = paymentDescription;
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
            output += "      * refundedDate: " + refundedDate + "\n";
            output += "      * committedDate: " + committedDate + "\n";
            output += "      * transactionId: " + transactionId + "\n";
            output += "      * refundAmount: " + refundAmount + "\n";
            output += "      * paymentDescription: " + paymentDescription + "\n";

            return output;
        }

    }
}
