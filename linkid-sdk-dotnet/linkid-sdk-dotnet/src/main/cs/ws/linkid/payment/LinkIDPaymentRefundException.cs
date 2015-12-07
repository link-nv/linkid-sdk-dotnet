using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDPaymentRefundException : System.Exception
    {
        public PaymentRefundErrorCode errorCode { get; set; }

        public LinkIDPaymentRefundException(PaymentRefundErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
