using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDConfigWalletApplicationsException : System.Exception
    {
        public ConfigWalletApplicationsErrorCode errorCode { get; set; }

        public LinkIDConfigWalletApplicationsException(ConfigWalletApplicationsErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
