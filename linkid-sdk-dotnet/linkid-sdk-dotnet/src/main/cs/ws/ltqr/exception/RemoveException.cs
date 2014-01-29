using System;

namespace safe_online_sdk_dotnet
{
    public class RemoveException : System.Exception
    {
        public ErrorCode errorCode { get; set; }

        public RemoveException(ErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }

    }
}
