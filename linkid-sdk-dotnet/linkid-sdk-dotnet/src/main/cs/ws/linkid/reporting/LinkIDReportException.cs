using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDReportException: System.Exception
    {
        public ReportErrorCode errorCode { get; set; }

        public LinkIDReportException(ReportErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
