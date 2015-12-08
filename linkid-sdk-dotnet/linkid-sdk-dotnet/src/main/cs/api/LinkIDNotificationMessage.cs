using System;
using System.Web;

namespace safe_online_sdk_dotnet
{
    public class LinkIDNotificationMessage
    {
        public LinkIDNotificationTopic topic { get; set; }
        
        // core notifications
        public String userId { get; set; }
        public String filter { get; set; }
        public String info { get; set; }

        // payment status
        public String paymentOrderReference { get; set; }

        // LTQR
        public String ltqrReference { get; set; }
        public String ltqrClientSessionId { get; set; }
        public String ltqrPaymentOrderReference { get; set; }

        public LinkIDNotificationMessage(HttpRequest request)
        {
            this.topic = LinkIDNotificationTopicExtensions.parse(request[LinkIDNotificationConstants.TOPIC_PARAM]);
            this.userId = request[LinkIDNotificationConstants.USER_ID_PARAM];
            this.filter = request[LinkIDNotificationConstants.FILTER_PARAM];
            this.info = request[LinkIDNotificationConstants.INFO_PARAM];
            
            this.paymentOrderReference = request[LinkIDNotificationConstants.PAYMENT_ORDER_REF_PARAM];
            
            this.ltqrReference = request[LinkIDNotificationConstants.LTQR_REF_PARAM];
            this.ltqrClientSessionId = request[LinkIDNotificationConstants.LTQR_CLIENT_SESSION_ID_PARAM];
            this.ltqrPaymentOrderReference = request[LinkIDNotificationConstants.LTQR_PAYMENT_ORDER_REF_PARAM];
        }

    }
}
