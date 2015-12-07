using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDAuthPollException : System.Exception
    {
        public AuthPollErrorCode errorCode { get; set; }

        public LinkIDAuthPollException(AuthPollErrorCode errorCode, String message)
            : base(message)
        {
            this.errorCode = errorCode;
        }
    }
}
