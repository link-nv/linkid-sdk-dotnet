using System;
using WalletWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class WalletReleaseException : System.Exception
    {
        public WalletWSNameSpace.WalletReleaseErrorCode errorCode { get; set; }

        public WalletReleaseException(WalletWSNameSpace.WalletReleaseErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
