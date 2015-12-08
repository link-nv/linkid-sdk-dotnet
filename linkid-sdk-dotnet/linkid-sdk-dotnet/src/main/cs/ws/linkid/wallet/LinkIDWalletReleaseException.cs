using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDWalletReleaseException: System.Exception
    {
        public WalletReleaseErrorCode errorCode { get; set; }

        public LinkIDWalletReleaseException(WalletReleaseErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
