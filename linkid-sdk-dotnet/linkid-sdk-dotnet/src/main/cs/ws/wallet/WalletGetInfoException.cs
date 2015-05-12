using System;
using WalletWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class WalletGetInfoException : System.Exception
    {
        public WalletWSNameSpace.WalletGetInfoErrorCode errorCode { get; set; }

        public WalletGetInfoException(WalletWSNameSpace.WalletGetInfoErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }

    }
}
