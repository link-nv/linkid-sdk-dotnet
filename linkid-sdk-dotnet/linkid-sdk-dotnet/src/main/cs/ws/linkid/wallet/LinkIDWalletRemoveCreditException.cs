using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDWalletRemoveCreditException: System.Exception
    {
        public WalletRemoveCreditErrorCode errorCode { get; set; }

        public LinkIDWalletRemoveCreditException(WalletRemoveCreditErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
