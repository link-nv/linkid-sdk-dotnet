using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDPaymentStatusException : System.Exception
    {
        public PaymentStatusErrorCode errorCode { get; set; }

        public LinkIDPaymentStatusException(PaymentStatusErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
