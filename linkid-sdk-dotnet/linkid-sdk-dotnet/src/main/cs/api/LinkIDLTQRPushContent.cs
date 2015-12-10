using System;

namespace safe_online_sdk_dotnet
{
    public class LinkIDLTQRPushContent
    {
        public LinkIDLTQRContent content { get; set; }
        public String userAgent { get; set; }
        public LinkIDLTQRLockType lockType { get; set; }

        public LinkIDLTQRPushContent(LinkIDLTQRContent content, String userAgent, LinkIDLTQRLockType lockType)
        {
            this.content = content;
            this.userAgent = userAgent;
            this.lockType = lockType;
        }
    }
}
