using System;
using CaptureWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class CaptureException : System.Exception
    {
        public CaptureWSNameSpace.ErrorCode errorCode { get; set; }

        public CaptureException(CaptureWSNameSpace.ErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
