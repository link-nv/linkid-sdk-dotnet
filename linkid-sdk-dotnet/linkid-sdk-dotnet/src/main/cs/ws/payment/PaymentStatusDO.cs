using System;

namespace safe_online_sdk_dotnet
{
    public class PaymentStatusDO
    {
        public PaymentState paymentState { get; set; }
        public Boolean captured { get; set; }

        public PaymentStatusDO(PaymentState paymentState, Boolean captured)
        {
            this.paymentState = paymentState;
            this.captured = captured;
        }
    }
}
