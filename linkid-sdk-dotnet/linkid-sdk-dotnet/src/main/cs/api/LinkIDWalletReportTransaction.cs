using System;
using System.Collections.Generic;
using System.Text;

namespace safe_online_sdk_dotnet
{
    public class LinkIDWalletReportTransaction : LinkIDWalletTransaction
    {
        public String userId { get; set; }
        public String applicationName { get; set; }

        public LinkIDWalletReportTransaction(String walletId, DateTime creationDate, String transactionId,
            double amount, Nullable<LinkIDCurrency> currency, String walletCoin, 
            String userId, String applicationName) : base(walletId, creationDate, transactionId, amount, currency, walletCoin)
        {            
            this.userId = userId;
            this.applicationName = applicationName;
        }

        public override String ToString()
        {
            String output = "";

            output += "    Wallet report transaction: \n";
            if (null != currency)
            {
                output += "      * amount: " + amount + currency + "\n";
            }
            else
            {
                output += "      * amount: " + amount + walletCoin + "\n";
            }
            output += "      * walletId: " + walletId + "\n";
            output += "      * creationDate: " + creationDate + "\n";
            output += "      * transactionId: " + transactionId + "\n";
            output += "      * userId: " + userId + "\n";
            output += "      * applicationName: " + applicationName + "\n";

            return output;
        }

    }
}
