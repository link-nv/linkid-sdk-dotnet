using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDLTQRRemoveException : System.Exception
    {
        public LTQRErrorCode errorCode{ get; set; }

        public LinkIDLTQRRemoveException(LTQRErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
