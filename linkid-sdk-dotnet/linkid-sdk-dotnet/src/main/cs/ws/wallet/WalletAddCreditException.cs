using System;
using WalletWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class WalletAddCreditException : System.Exception
    {
        public WalletWSNameSpace.WalletAddCreditErrorCode errorCode { get; set; }

        public WalletAddCreditException(WalletWSNameSpace.WalletAddCreditErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
