using System;

namespace safe_online_sdk_dotnet
{
    public class LinkIDPaymentStatus
    {
        public String orderReference { get; set; }
        public String userId { get; set; }
        public LinkIDPaymentState paymentState { get; set; }
        public bool authorized { get; set; }
        public bool captured { get; set; }
        public double amountPayed { get; set; }
        public double amount { get; set; }
        public double refundAmount { get; set; }
        public Nullable<LinkIDCurrency> currency { get; set; }
        public String walletCoin { get; set; }
        public String description { get; set; }
        public String profile { get; set; }
        public DateTime created { get; set; }
        public String mandateReference { get; set; }
        public LinkIDPaymentDetails paymentDetails { get; set; }

        public LinkIDPaymentStatus(String orderReference, String userId, LinkIDPaymentState paymentState,
            bool authorized, bool captured, double amountPayed, double amount, double refundAmount, 
            Nullable<LinkIDCurrency> currency, String walletCoin, String description, String profile, 
            DateTime created, String mandateReference, LinkIDPaymentDetails paymentDetails)
        {
            this.orderReference = orderReference;
            this.userId = userId;
            this.paymentState = paymentState;
            this.authorized = authorized;
            this.captured = captured;
            this.amountPayed = amountPayed;
            this.amount = amount;
            this.refundAmount = refundAmount;
            this.currency = currency;
            this.walletCoin = walletCoin;
            this.description = description;
            this.profile = profile;
            this.created = created;
            this.mandateReference = mandateReference;
            this.paymentDetails = paymentDetails;
        }

        public override String ToString()
        {
            String output = "";

            output += "Payment status: \n";
            output += "  * orderReference: " + orderReference + "\n";
            output += "  * userId: " + userId + "\n";
            output += "  * paymentState: " + paymentState + "\n";
            output += "  * authorized: " + authorized + "\n";
            output += "  * captured: " + captured + "\n";
            output += "  * amountPayed: " + amountPayed + "\n";
            output += "  * amount: " + amount + "\n";
            output += "  * refundAmount: " + refundAmount + "\n";
            output += "  * currency: " + currency + "\n";
            output += "  * walletCoin: " + walletCoin + "\n"; 
            output += "  * description: " + description + "\n";
            output += "  * profile: " + profile + "\n";
            output += "  * created: " + created + "\n";
            output += "  * mandateReference: " + mandateReference + "\n";
            output += "  * paymentDetails:\n";
            output += "    " + paymentDetails.ToString();

            return output;
        }
    }
}
