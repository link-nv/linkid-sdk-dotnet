using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Security;


using safe_online_sdk_dotnet;

/*
 * Example linkID notification page.
 * This page will be poked by linkID for notifications of all kind.
 */

namespace linkid_example
{
    public partial class LTQRNotification : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LinkIDNotificationMessage message = new LinkIDNotificationMessage(Request);

            switch (message.topic)
            {
                case LinkIDNotificationTopic.REMOVE_USER:
                case LinkIDNotificationTopic.UNSUBSCRIBE_USER:
                case LinkIDNotificationTopic.ATTRIBUTE_UPDATE:
                case LinkIDNotificationTopic.ATTRIBUTE_REMOVAL:
                case LinkIDNotificationTopic.IDENTITY_UPDATE:
                case LinkIDNotificationTopic.EXPIRED_AUTHENTICATION:
                case LinkIDNotificationTopic.EXPIRED_PAYMENT:
                case LinkIDNotificationTopic.MANDATE_ARCHIVED:
                case LinkIDNotificationTopic.LTQR_SESSION_NEW:
                case LinkIDNotificationTopic.LTQR_SESSION_CANCEL:
                case LinkIDNotificationTopic.LTQR_SESSION_UPDATE:
                case LinkIDNotificationTopic.CONFIGURATION_UPDATE:
                case LinkIDNotificationTopic.PAYMENT_ORDER_UPDATE:
                case LinkIDNotificationTopic.AUTHENTICATION_RETRIEVED:
                case LinkIDNotificationTopic.AUTHENTICATION_SUCCESS:
                case LinkIDNotificationTopic.AUTHENTICATION_PAYMENT_FINISHED:
                case LinkIDNotificationTopic.AUTHENTICATION_CANCELED:
                case LinkIDNotificationTopic.AUTHENTICATION_FAILED:
                    
                    return;
            }

        }
    }
}
