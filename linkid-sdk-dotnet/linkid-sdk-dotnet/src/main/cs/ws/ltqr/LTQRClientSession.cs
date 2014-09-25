using System;
using LTQRWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LTQRClientSession
    {
        public String ltqrReference { get; private set; }
        public String clientSessionId { get; private set; }
        public String userId { get; private set; }
        public DateTime created { get; private set; }
        public LTQRPaymentStatusType paymentState { get; private set; }
        public String paymentOrderReference { get; private set; }

        public LTQRClientSession(String ltqrReference, String clientSessionId, String userId, DateTime created,
            LTQRPaymentStatusType paymentState, String paymentOrderReference)
        {
            this.ltqrReference = ltqrReference;
            this.clientSessionId = clientSessionId;
            this.userId = userId;
            this.created = created;
            this.paymentState = paymentState;
            this.paymentOrderReference = paymentOrderReference;
        }

        public override String ToString()
        {
            return "LTQRReference: \"" + ltqrReference + "\", Payment order ref: \"" + paymentOrderReference 
                + "\", ClientSessionID: \"" + clientSessionId + "\", userId: \"" + userId + 
                "\", created: \"" + created + "\", paymentState: \"" + paymentState + "\"";                
        }
    }
}
