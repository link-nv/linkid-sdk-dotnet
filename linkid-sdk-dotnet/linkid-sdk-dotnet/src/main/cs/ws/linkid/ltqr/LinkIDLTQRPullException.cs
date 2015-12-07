using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDLTQRPullException : System.Exception
    {
        public LTQRErrorCode errorCode{ get; set; }

        public LinkIDLTQRPullException(LTQRErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
