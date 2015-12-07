using System;

namespace safe_online_sdk_dotnet
{
    public class LinkIDAuthSession
    {
        public String sessionId { get; set; }
        public LinkIDQRInfo qrCodeInfo { get; set; }

        public LinkIDAuthSession(String sessionId, LinkIDQRInfo qrCodeInfo)
        {
            this.sessionId = sessionId;
            this.qrCodeInfo = qrCodeInfo;
        }
    }
}
