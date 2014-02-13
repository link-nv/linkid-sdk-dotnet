using System;
using LTQRWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LTQRClientSession
    {
        public String orderReference { get; private set; }
        public String clientSessionId { get; private set; }
        public String userId { get; private set; }
        public DateTime created { get; private set; }
        public LTQRPaymentStatusType paymentState { get; private set; }

        public LTQRClientSession(String orderReference, String clientSessionId, String userId, DateTime created, LTQRPaymentStatusType paymentState)
        {
            this.orderReference = orderReference;
            this.clientSessionId = clientSessionId;
            this.userId = userId;
            this.created = created;
            this.paymentState = paymentState;
        }

        public override String ToString()
        {
            return "OrderReference: \"" + orderReference + "\", ClientSessionID: \"" + clientSessionId + "\", userId: \"" + userId + "\", created: \"" + created + "\", paymentState: \"" + paymentState + "\"";                
        }
    }
}
