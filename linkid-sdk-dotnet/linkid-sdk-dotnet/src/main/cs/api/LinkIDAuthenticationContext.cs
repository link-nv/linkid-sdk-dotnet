using System;
using System.Collections.Generic;
using System.Web.SessionState;

namespace safe_online_sdk_dotnet
{
    /// <summary>
    /// Contains the linkID authentication/payment configuration
    /// </summary>
    public class LinkIDAuthenticationContext
    {
        public static String SESSION_LINKID_CONTEXT = "linkID.AuthenticationContext.";

        // device context param names
        public static readonly String AUTHENTICATION_MESSAGE = "linkID.authenticationMessage";
        public static readonly String FINISHED_MESSAGE = "linkID.finishedMessage";
        public static readonly String IDENTITY_PROFILE_PREFIX = "linkID.identityProfile";
        public static readonly String SESSION_EXPIRY_OVERRIDE = "linkID.sessionExpiryOverride";
        public static readonly String THEME = "linkID.theme";
        public static readonly String MOBILE_LANDING_SUCCESS_URL = "linkID.mobileLandingSuccess";
        public static readonly String MOBILE_LANDING_ERROR_URL = "linkID.mobileLandingError";
        public static readonly String MOBILE_LANDING_CANCEL_URL = "linkID.mobileLandingCancel";
        public static readonly String NOTIFICATION_LOCATION = "linkID.notificationLocation";

        public String applicationName { get; set; }
        public String applicationFriendlyName { get; set; }

        public String language { get; set; }

        public String authenticationMessage { get; set; }
        public String finishedMessage { get; set; }
        public String identityProfile { get; set; }
        public long sessionExpiryOverride { get; set; }
        public String theme { get; set; }
        public String notificationLocation { get; set; }

        public String mobileLandingSuccess { get; set; }
        public String mobileLandingError { get; set; }
        public String mobileLandingCancel { get; set; }

        public bool forceAuthentication { get; set; }
        public Dictionary<string, List<Object>> attributeSuggestions { get; set; }
        public LinkIDPaymentContext paymentContext { get; set; }
        public LinkIDCallback callback { get; set; }

        public LinkIDAuthenticationContext()
        {
        }

        public void store(HttpSessionState session)
        {
            session.Add(SESSION_LINKID_CONTEXT, this);
        }

        public static LinkIDAuthenticationContext getLinkIDContext(HttpSessionState session)
        {
            return (LinkIDAuthenticationContext)session[SESSION_LINKID_CONTEXT];
        }

        public Dictionary<string, string> getDeviceContextMap()
        {
            Dictionary<string, string> deviceContextMap = new Dictionary<string, string>();
            deviceContextMap.Add(AUTHENTICATION_MESSAGE, authenticationMessage);
            deviceContextMap.Add(FINISHED_MESSAGE, finishedMessage);

            // identity profile
            if (null != identityProfile)
            {
                deviceContextMap.Add(IDENTITY_PROFILE_PREFIX, identityProfile);
            }

            if (sessionExpiryOverride > 0)
            {
                deviceContextMap.Add(SESSION_EXPIRY_OVERRIDE, sessionExpiryOverride.ToString());
            }
            if (null != theme)
            {
                deviceContextMap.Add(THEME, theme);
            }
            if (null != notificationLocation)
            {
                deviceContextMap.Add(NOTIFICATION_LOCATION, notificationLocation);
            }

            if (null != mobileLandingSuccess)
            {
                deviceContextMap.Add(MOBILE_LANDING_SUCCESS_URL, mobileLandingSuccess);
            }
            if (null != mobileLandingError)
            {
                deviceContextMap.Add(MOBILE_LANDING_ERROR_URL, mobileLandingError);
            }
            if (null != mobileLandingCancel)
            {
                deviceContextMap.Add(MOBILE_LANDING_CANCEL_URL, mobileLandingCancel);
            }

            return deviceContextMap;
        }
    }
}
