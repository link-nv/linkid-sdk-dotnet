using System;

namespace safe_online_sdk_dotnet
{
    public class LinkIDPaymentStatus
    {
        public LinkIDPaymentState paymentState;
        public bool captured;
        public double amountPayed;
        public LinkIDPaymentDetails paymentDetails;

        public LinkIDPaymentStatus(LinkIDPaymentState paymentState, bool captured, double amountPayed,
            LinkIDPaymentDetails paymentDetails)
        {
            this.paymentState = paymentState;
            this.captured = captured;
            this.amountPayed = amountPayed;
            this.paymentDetails = paymentDetails;
        }

        public override String ToString()
        {
            String output = "";

            output += "Payment status: \n";
            output += "  * paymentState: " + paymentState + "\n";
            output += "  * captured: " + captured + "\n";
            output += "  * amountPayed: " + amountPayed + "\n";
            output += "  * paymentDetails:\n";
            output += "    " + paymentDetails.ToString();

            return output;
        }
    }
}
