using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDWalletCommitException: System.Exception
    {
        public WalletCommitErrorCode errorCode { get; set; }

        public LinkIDWalletCommitException(WalletCommitErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
