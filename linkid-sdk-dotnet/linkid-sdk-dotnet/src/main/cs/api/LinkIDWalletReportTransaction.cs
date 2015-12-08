using System;
using System.Collections.Generic;
using System.Text;

namespace safe_online_sdk_dotnet
{
    public class LinkIDWalletReportTransaction : LinkIDWalletTransaction
    {
        public String userId { get; set; }
        public String applicationName { get; set; }
        public String applicationFriendly { get; set; }
        public LinkIDWalletReportType type { get; set; }

        public LinkIDWalletReportTransaction(String walletId, DateTime creationDate, String transactionId,
            double amount, Nullable<LinkIDCurrency> currency, String walletCoin, 
            double refundAmount, String paymentDescription, String userId, String applicationName,
            String applicationFriendly, LinkIDWalletReportType type) 
            : base(walletId, creationDate, transactionId, amount, currency, walletCoin, refundAmount, paymentDescription)
        {            
            this.userId = userId;
            this.applicationName = applicationName;
            this.applicationFriendly = applicationFriendly;
            this.type = type;
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
            output += "      * applicationFriendly: " + applicationFriendly + "\n";
            output += "      * type: " + type + "\n";

            return output;
        }

    }
}
