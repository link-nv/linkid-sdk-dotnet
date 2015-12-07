using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDAuthCancelException : System.Exception
    {
        public AuthCancelErrorCode errorCode { get; set; }

        public LinkIDAuthCancelException(AuthCancelErrorCode errorCode, String message)
            : base(message)
        {
            this.errorCode = errorCode;
        }
    }
}
