using System;
using WalletWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class WalletRemoveException : System.Exception
    {
        public WalletWSNameSpace.WalletRemoveErrorCode errorCode { get; set; }

        public WalletRemoveException(WalletWSNameSpace.WalletRemoveErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
