using System;

namespace safe_online_sdk_dotnet
{
    public class ChangeException : System.Exception
    {
        public ChangeErrorCode errorCode { get; set; }

        public ChangeException(ChangeErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }

    }
}
