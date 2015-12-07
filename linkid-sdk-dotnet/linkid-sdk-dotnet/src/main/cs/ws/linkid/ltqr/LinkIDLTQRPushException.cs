using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDLTQRPushException : System.Exception
    {
        public LTQRPushErrorCode errorCode { get; set; }

        public LinkIDLTQRPushException(LTQRPushErrorCode errorCode, String message)
            : base(message)
        {
            this.errorCode = errorCode;
        }
    }
}
