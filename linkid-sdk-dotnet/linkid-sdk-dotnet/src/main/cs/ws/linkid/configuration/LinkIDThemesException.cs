using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDThemesException : System.Exception
    {
        public ConfigThemesErrorCode errorCode { get; set; }

        public LinkIDThemesException(ConfigThemesErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
