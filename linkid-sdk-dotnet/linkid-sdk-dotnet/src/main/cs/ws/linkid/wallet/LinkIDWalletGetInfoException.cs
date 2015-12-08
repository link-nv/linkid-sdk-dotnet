using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDWalletGetInfoException: System.Exception
    {
        public WalletGetInfoErrorCode errorCode { get; set; }

        public LinkIDWalletGetInfoException(WalletGetInfoErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
