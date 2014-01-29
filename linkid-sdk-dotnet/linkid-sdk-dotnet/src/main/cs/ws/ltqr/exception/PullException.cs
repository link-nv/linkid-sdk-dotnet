using System;

namespace safe_online_sdk_dotnet
{
    public class PullException : System.Exception
    {
        public ErrorCode errorCode { get; set; }

        public PullException(ErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }

    }
}
