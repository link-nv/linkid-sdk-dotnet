using System;

namespace safe_online_sdk_dotnet
{
    public class LinkIDLTQRClientSession
    {
        public String ltqrReference { get; set; }
        public LinkIDQRInfo qrCodeInfo { get; set; }
        public String clientSessionId { get; set; }
        public String userId { get; set; }
        public DateTime created { get; set; }
        public String paymentOrderReference { get; set; }
        public LinkIDPaymentState paymentState { get; set; }

        public LinkIDLTQRClientSession(String ltqrReference, LinkIDQRInfo qrCodeInfo, String clientSessionId,
            String userId, DateTime created, String paymentOrderReference, LinkIDPaymentState paymentState)
        {
            this.ltqrReference = ltqrReference;
            this.qrCodeInfo = qrCodeInfo;
            this.clientSessionId = clientSessionId;
            this.userId = userId;
            this.created = created;
            this.paymentOrderReference = paymentOrderReference;
            this.paymentState = paymentState;
        }
    }
}
