using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDWalletEnrollException: System.Exception
    {
        public WalletEnrollErrorCode errorCode { get; set; }

        public LinkIDWalletEnrollException(WalletEnrollErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
