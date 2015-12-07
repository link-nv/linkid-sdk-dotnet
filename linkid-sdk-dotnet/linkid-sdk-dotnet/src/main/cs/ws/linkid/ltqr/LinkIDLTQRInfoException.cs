using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDLTQRInfoException : System.Exception
    {
        public LTQRErrorCode errorCode { get; set; }

        public LinkIDLTQRInfoException(LTQRErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
