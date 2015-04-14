using System;
using WalletWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class WalletRemoveCreditException : System.Exception
    {
        public WalletWSNameSpace.WalletRemoveCreditErrorCode errorCode { get; set; }

        public WalletRemoveCreditException(WalletWSNameSpace.WalletRemoveCreditErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
