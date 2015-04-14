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

        public String applicationName { get; set; }
        public String applicationFriendlyName { get; set; }
        public String serviceProviderUrl { get; set; }          // The URL that will handle the returned SAML response
        public String identityProviderUrl { get; set; }         // The LinkID authentication entry URL
        
        public String authenticationMessage { get; set; }
        public String finishedMessage { get; set; }
        public String language { get; set; }

        public List<String> identityProfiles { get; set; }
        public long sessionExpiryOverride { get; set; }
        public String theme { get; set; }

        public bool mobileForceRegistration { get; set; }
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

            // identity profiles
            if (null != identityProfiles)
            {
                int i = 0;
                foreach (String identityProfile in identityProfiles)
                {
                    deviceContextMap.Add(IDENTITY_PROFILE_PREFIX + "." + i, identityProfile);
                    i++;
                }
            }

            if (sessionExpiryOverride > 0)
            {
                deviceContextMap.Add(SESSION_EXPIRY_OVERRIDE, sessionExpiryOverride.ToString());
            }
            if (null != theme)
            {
                deviceContextMap.Add(THEME, theme);
            }

            return deviceContextMap;
        }
    }
}
