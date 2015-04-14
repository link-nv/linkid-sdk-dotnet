using System;
using System.Collections.Generic;
using System.Text;

namespace safe_online_sdk_dotnet
{
    public class LinkIDPaymentOrder
    {
        public DateTime date { get; set; }
        public double amount { get; set; }
        public LinkIDCurrency currency { get; set; }
        public String description { get; set; }
        public LinkIDPaymentState paymentState { get; set; }
        public double amountPayed { get; set; }
        public bool authorized { get; set; }
        public bool captured { get; set; }
        public String orderReference { get; set; }
        public String userId { get; set; }
        public String email { get; set; }
        public String givenName { get; set; }
        public String familyName { get; set; }
        public List<LinkIDPaymentTransaction> transactions { get; set; }
        public List<LinkIDWalletTransaction> walletTransactions { get; set; }

        public LinkIDPaymentOrder(DateTime date, double amount, LinkIDCurrency currency, String description,
            LinkIDPaymentState paymentState, double amountPayed, bool authorized, bool captured,
            String orderReference, String userId, String email, String givenName, String familyName,
            List<LinkIDPaymentTransaction> transactions, List<LinkIDWalletTransaction> walletTransactions)
        {
            this.date = date;
            this.amount = amount;
            this.currency = currency;
            this.description = description;
            this.paymentState = paymentState;
            this.amountPayed = amountPayed;
            this.authorized = authorized;
            this.captured = captured;
            this.orderReference = orderReference;
            this.userId = userId;
            this.email = email;
            this.givenName = givenName;
            this.familyName = familyName;
            this.transactions = transactions;
            this.walletTransactions = walletTransactions;
        }

        public override String ToString()
        {
            String output = "";

            output += "Payment order: " + orderReference + "\n";
            output += "  * date: " + date + "\n";
            output += "  * amount: " + amount + currency + "\n";
            output += "  * description: " + description + "\n";
            output += "  * paymentState: " + paymentState + "\n";
            output += "  * amountPayed: " + amountPayed + "\n";
            output += "  * authorized: " + authorized + "\n";
            output += "  * captured: " + captured + "\n";
            output += "  * authorized: " + authorized + "\n";
            output += "  * userId: " + userId + "\n";
            output += "  * email: " + email + "\n";
            output += "  * givenName: " + givenName + "\n";
            output += "  * familyName: " + familyName + "\n";
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
