using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDWalletAddCreditException: System.Exception
    {
        public WalletAddCreditErrorCode errorCode { get; set; }

        public LinkIDWalletAddCreditException(WalletAddCreditErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
