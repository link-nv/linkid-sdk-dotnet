using System;
using WalletWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class WalletCommitException : System.Exception
    {
        public WalletWSNameSpace.WalletCommitErrorCode errorCode { get; set; }

        public WalletCommitException(WalletWSNameSpace.WalletCommitErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
