using System;
using System.Collections.Generic;
using System.Text;

namespace safe_online_sdk_dotnet
{
    public class LinkIDWalletReportTransaction : LinkIDWalletTransaction
    {
        public String id { get; set; }
        public String userId { get; set; }
        public String applicationName { get; set; }
        public String applicationFriendly { get; set; }
        public LinkIDWalletReportType type { get; set; }
        public LinkIDWalletReportInfo reportInfo { get; set; }

        public LinkIDWalletReportTransaction(String id, String walletId, DateTime creationDate, DateTime refundedDate,
            DateTime committedDate, String transactionId,
            double amount, Nullable<LinkIDCurrency> currency, String walletCoin, 
            double refundAmount, String paymentDescription, String userId, String applicationName,
            String applicationFriendly, LinkIDWalletReportType type, LinkIDWalletReportInfo reportInfo) 
            : base(walletId, creationDate, refundedDate, committedDate, transactionId, amount, currency, walletCoin, refundAmount, paymentDescription)
        {
            this.id = id;
            this.userId = userId;
            this.applicationName = applicationName;
            this.applicationFriendly = applicationFriendly;
            this.type = type;
            this.reportInfo = reportInfo;
        }

        public override String ToString()
        {
            String output = "";

            output += "    Wallet report transaction: \n";
            output += "      * id: " + id + "\n";
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
