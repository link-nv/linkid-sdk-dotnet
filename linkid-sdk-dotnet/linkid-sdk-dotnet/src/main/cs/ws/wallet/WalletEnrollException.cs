using System;
using WalletWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class WalletEnrollException : System.Exception
    {
        public WalletWSNameSpace.WalletEnrollErrorCode errorCode { get; set; }

        public WalletEnrollException(WalletWSNameSpace.WalletEnrollErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }

    }
}
