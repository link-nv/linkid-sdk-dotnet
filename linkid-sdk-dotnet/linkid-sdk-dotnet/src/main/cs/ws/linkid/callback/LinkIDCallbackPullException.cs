using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDCallbackPullException : System.Exception
    {
        public CallbackPullErrorCode errorCode { get; set; }

        public LinkIDCallbackPullException(CallbackPullErrorCode errorCode, String message)
            : base(message)
        {
            this.errorCode = errorCode;
        }
    }
}
