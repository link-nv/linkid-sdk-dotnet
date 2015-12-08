using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDWalletInfoReportException: System.Exception
    {
        public WalletInfoReportErrorCode errorCode { get; set; }

        public LinkIDWalletInfoReportException(WalletInfoReportErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
