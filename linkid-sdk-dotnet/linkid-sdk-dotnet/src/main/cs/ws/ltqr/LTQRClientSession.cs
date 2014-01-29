using System;
using LTQRWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LTQRClientSession
    {
        public String sessionId { get; private set; }
        public String clientSessionId { get; private set; }
        public String userId { get; private set; }
        public DateTime created { get; private set; }
        public LTQRPaymentStatusType paymentState { get; private set; }

        public LTQRClientSession(String sessionId, String clientSessionId, String userId, DateTime created, LTQRPaymentStatusType paymentState)
        {
            this.sessionId = sessionId;
            this.clientSessionId = clientSessionId;
            this.userId = userId;
            this.created = created;
            this.paymentState = paymentState;
        }

        public override String ToString()
        {
            return "SessionID: \"" + sessionId + "\", ClientSessionID: \"" + clientSessionId + "\", userId: \"" + userId + "\", created: \"" + created + "\", paymentState: \"" + paymentState + "\"";                
        }
    }
}
