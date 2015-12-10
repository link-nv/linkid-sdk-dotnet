using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDLTQRPushResponse
    {
        public LinkIDLTQRSession ltqrSession { get; set; }

        public LTQRPushErrorCode errorCode { get; set; }
        public String errorMessage { get; set; }

        public LinkIDLTQRPushResponse(LinkIDLTQRSession ltqrSession)
        {
            this.ltqrSession = ltqrSession;
        }

        public LinkIDLTQRPushResponse(LTQRPushErrorCode errorCode, String errorMessage)
        {
            this.errorCode = errorCode;
            this.errorMessage = errorMessage;
        }
    }
}
