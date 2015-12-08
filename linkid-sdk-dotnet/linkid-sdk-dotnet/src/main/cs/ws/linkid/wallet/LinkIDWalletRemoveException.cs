using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDWalletRemoveException: System.Exception
    {
        public WalletRemoveErrorCode errorCode { get; set; }

        public LinkIDWalletRemoveException(WalletRemoveErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
