using System;

namespace safe_online_sdk_dotnet
{
    public class LinkIDLTQRInfoException : System.Exception
    {
        public ErrorCode errorCode { get; set; }

        public LinkIDLTQRInfoException(ErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }

    }
}
