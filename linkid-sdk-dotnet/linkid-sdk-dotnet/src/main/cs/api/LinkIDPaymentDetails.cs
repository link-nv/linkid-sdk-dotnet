using System;
using System.Collections.Generic;

namespace safe_online_sdk_dotnet
{
    public class LinkIDPaymentDetails
    {
        public List<LinkIDPaymentTransaction> transactions { get; set; }
        public List<LinkIDWalletTransaction> walletTransactions { get; set; }

        public LinkIDPaymentDetails(List<LinkIDPaymentTransaction> transactions,
            List<LinkIDWalletTransaction> walletTransactions)
        {
            this.transactions = transactions;
            this.walletTransactions = walletTransactions;
        }

        public override String ToString()
        {
            String output = "";

            if (transactions.Count > 0)
            {
                output += "  * Transactions\n";
                foreach (LinkIDPaymentTransaction transaction in transactions)
                {
                    output += transaction.ToString();
                }
            }
            if (walletTransactions.Count > 0)
            {
                output += "  * Wallet transactions\n";
                foreach (LinkIDWalletTransaction walletTransaction in walletTransactions)
                {
                    output += walletTransaction.ToString();
                }
            }

            return output;
        }

    }
}
