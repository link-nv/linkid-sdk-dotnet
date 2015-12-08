using System;

namespace safe_online_sdk_dotnet
{
    public sealed class LinkIDNotificationConstants
    {
        // core notifications
        public static readonly String TOPIC_PARAM = "topic";
        public static readonly String FILTER_PARAM = "filter";
        public static readonly String USER_ID_PARAM = "userId";
        public static readonly String INFO_PARAM = "info";

        // payment status
        public static readonly String PAYMENT_ORDER_REF_PARAM = "orderRef";

        // LTQR
        public static readonly String LTQR_REF_PARAM = "ltqrRef";
        public static readonly String LTQR_PAYMENT_ORDER_REF_PARAM = "paymentOrderRef";
        public static readonly String LTQR_CLIENT_SESSION_ID_PARAM = "clientSessionId";

        private LinkIDNotificationConstants()
        {
        }
    }
}
