using System;
using LTQRWSNameSpace;
using System.Collections.Generic;

namespace safe_online_sdk_dotnet
{
    public class LinkIDLTQRInfo
    {
        public String ltqrReference { get; private set; }
        public String sessionId { get; private set; }
        public DateTime created { get; private set; }
        //
        public LinkIDQRInfo qrCodeInfo{ get; private set; }
        //
        public LinkIDLTQRContent content { get; private set; }
        //
        public LinkIDLTQRLockType lockType { get; private set; }
        public bool locked { get; private set; }
        //
        public bool waitForUnblock { get; private set; }
        public bool blocked { get; private set; }

        public LinkIDLTQRInfo(String ltqrReference, String sessionId, DateTime created,
            LinkIDQRInfo qrCodeInfo, LinkIDLTQRContent content, LinkIDLTQRLockType lockType, bool locked,
            bool waitForUnblock, bool blocked)
        {
            this.ltqrReference = ltqrReference;
            this.sessionId = sessionId;
            this.created = created;
            this.qrCodeInfo = qrCodeInfo;
            this.content = content;
            this.lockType = lockType;
            this.locked = locked;
            this.waitForUnblock = waitForUnblock;
            this.blocked = blocked;
        }

        public override String ToString()
        {
            String output = "";

            output += "LTQR info\n";
            output += "  * ltqrReference: " + ltqrReference + "\n";
            output += "  * sessionId: " + sessionId + "\n";
            output += "  * created: " + created + "\n";

            return output;
        }

    }
}
