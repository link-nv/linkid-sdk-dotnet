using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDMandatePaymentException : System.Exception
    {
        public MandatePaymentErrorCode errorCode { get; set; }

        public LinkIDMandatePaymentException(MandatePaymentErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
