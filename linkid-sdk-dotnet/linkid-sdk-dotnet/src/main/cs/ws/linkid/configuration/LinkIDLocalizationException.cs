using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDLocalizationException : System.Exception
    {
        public ConfigLocalizationErrorCode errorCode { get; set; }

        public LinkIDLocalizationException(ConfigLocalizationErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
