using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDLTQRBulkPushException : System.Exception
    {
        public LTQRBulkPushErrorCode errorCode { get; set; }

        public LinkIDLTQRBulkPushException(LTQRBulkPushErrorCode errorCode, String message)
            : base(message)
        {
            this.errorCode = errorCode;
        }
    }
}
