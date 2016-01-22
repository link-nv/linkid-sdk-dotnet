using System;
using System.Collections;
using System.Collections.Generic;

namespace safe_online_sdk_dotnet
{
    public enum LinkIDNotificationTopic { 
        REMOVE_USER,
        UNSUBSCRIBE_USER,
        ATTRIBUTE_UPDATE,
        ATTRIBUTE_REMOVAL,
        IDENTITY_UPDATE,
        EXPIRED_AUTHENTICATION,
        EXPIRED_PAYMENT,
        MANDATE_ARCHIVED,
        LTQR_SESSION_NEW,
        LTQR_SESSION_CANCEL,
        LTQR_SESSION_UPDATE,
        CONFIGURATION_UPDATE,
        PAYMENT_ORDER_UPDATE,
        AUTHENTICATION_RETRIEVED,
        AUTHENTICATION_SUCCESS,
        AUTHENTICATION_PAYMENT_FINISHED,
        AUTHENTICATION_CANCELED,
        AUTHENTICATION_FAILED
    };

    static class LinkIDNotificationTopicExtensions
    {
        public static Dictionary<String, LinkIDNotificationTopic> topicMap = new Dictionary<string, LinkIDNotificationTopic>()
        {
            {"urn:net:lin-k:linkid:topic:user:remove", LinkIDNotificationTopic.REMOVE_USER},
            {"urn:net:lin-k:linkid:topic:user:unsubscribe", LinkIDNotificationTopic.UNSUBSCRIBE_USER},
            {"urn:net:lin-k:linkid:topic:user:attribute:update", LinkIDNotificationTopic.ATTRIBUTE_UPDATE},
            {"urn:net:lin-k:linkid:topic:user:attribute:remove", LinkIDNotificationTopic.ATTRIBUTE_REMOVAL},
            {"urn:net:lin-k:linkid:topic:user:identity:update", LinkIDNotificationTopic.IDENTITY_UPDATE},
            {"urn:net:lin-k:linkid:topic:expired:authentication", LinkIDNotificationTopic.EXPIRED_AUTHENTICATION},
            {"urn:net:lin-k:linkid:topic:expired:payment", LinkIDNotificationTopic.EXPIRED_PAYMENT},
            {"urn:net:lin-k:linkid:topic:mandate:archived", LinkIDNotificationTopic.MANDATE_ARCHIVED},
            {"urn:net:lin-k:linkid:topic:ltqr:session:new", LinkIDNotificationTopic.LTQR_SESSION_NEW},
            {"urn:net:lin-k:linkid:topic:ltqr:session:cancel", LinkIDNotificationTopic.LTQR_SESSION_CANCEL},
            {"urn:net:lin-k:linkid:topic:ltqr:session:update", LinkIDNotificationTopic.LTQR_SESSION_UPDATE},
            {"urn:net:lin-k:linkid:topic:config:update", LinkIDNotificationTopic.CONFIGURATION_UPDATE},
            {"urn:net:lin-k:linkid:topic:payment:update", LinkIDNotificationTopic.PAYMENT_ORDER_UPDATE},
            {"urn:net:lin-k:linkid:topic:authentication:retrieved", LinkIDNotificationTopic.AUTHENTICATION_RETRIEVED},
            {"urn:net:lin-k:linkid:topic:authentication:success", LinkIDNotificationTopic.AUTHENTICATION_SUCCESS},
            {"urn:net:lin-k:linkid:topic:authentication:payment:finished", LinkIDNotificationTopic.AUTHENTICATION_PAYMENT_FINISHED},
            {"urn:net:lin-k:linkid:topic:authentication:canceled", LinkIDNotificationTopic.AUTHENTICATION_CANCELED},
            {"urn:net:lin-k:linkid:topic:authentication:failed", LinkIDNotificationTopic.AUTHENTICATION_FAILED},
        };

        public static LinkIDNotificationTopic parse(String topicString)
        {
            return topicMap[topicString];
        }
    }

}
