using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDAuthException : System.Exception
    {
        public AuthStartErrorCode errorCode { get; set; }

        public LinkIDAuthException(AuthStartErrorCode errorCode, String message) : base(message)
        {
            this.errorCode = errorCode;
        }
    }
}
