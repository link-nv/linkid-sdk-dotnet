using System;

namespace safe_online_sdk_dotnet
{
    public class LinkIDLTQRSession
    {
        public String ltqrReference { get; set; }
        public LinkIDQRInfo qrCodeInfo { get; set; }
        public String paymentOrderReference { get; set; }

        public LinkIDLTQRSession(String ltqrReference, LinkIDQRInfo qrCodeInfo, String paymentOrderReference)
        {
            this.ltqrReference = ltqrReference;
            this.qrCodeInfo = qrCodeInfo;
            this.paymentOrderReference = paymentOrderReference;
        }
    }
}
