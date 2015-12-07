using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDPaymentCaptureException : System.Exception
    {
        public PaymentCaptureErrorCode errorCode { get; set; }

        public LinkIDPaymentCaptureException(PaymentCaptureErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
