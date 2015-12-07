using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDLTQRChangeException : System.Exception
    {
        public LTQRChangeErrorCode errorCode { get; set; }

        public LinkIDLTQRChangeException(LTQRChangeErrorCode errorCode, String message)
            : base(message)
        {
            this.errorCode = errorCode;
        }
    }
}
