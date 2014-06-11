using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;


namespace safe_online_sdk_dotnet
{
    public class LoginUtil
    {
        public static String SESSION_DEVICE_AUTHN_MESSAGE = "AuthnRequest.DeviceAuthnMessage";
        public static String SESSION_DEVICE_FINISHED_MESSAGE = "AuthnRequest.DeviceFinishedMessage";
        public static String SESSION_IDENTITY_PROFILES = "AuthnRequest.IdentityProfiles";
        public static String SESSION_ATTRIBUTE_SUGGESTIONS = "AuthnRequest.AttributeSuggestions";
        public static String SESSION_PAYMENT_CONTEXT = "AuthnRequest.PaymentContext";

        public static void setDeviceAuthnMessage(HttpSessionState session, string deviceAuthnMessage)
        {
            session.Add(SESSION_DEVICE_AUTHN_MESSAGE, deviceAuthnMessage);
        }

        public static string getDeviceAuthnMessage(HttpSessionState session)
        {
            return (string)session[SESSION_DEVICE_AUTHN_MESSAGE];
        }

        public static void setDeviceFinishedMessage(HttpSessionState session, string deviceFinishedMessage)
        {
            session.Add(SESSION_DEVICE_FINISHED_MESSAGE, deviceFinishedMessage);
        }

        public static string getDeviceFinishedMessage(HttpSessionState session)
        {
            return (string)session[SESSION_DEVICE_FINISHED_MESSAGE];
        }

        public static List<String> getIdentityProfiles(HttpSessionState session)
        {
            return (List<String>)session[SESSION_IDENTITY_PROFILES];
        }

        public static void setIdentityProfiles(HttpSessionState session, List<String> identityProfiles)
        {
            session.Add(SESSION_IDENTITY_PROFILES, identityProfiles);
        }

        public static void setAttriuteSuggestions(HttpSessionState session, Dictionary<string, List<Object>> attributeSuggestions)
        {
            session.Add(SESSION_ATTRIBUTE_SUGGESTIONS, attributeSuggestions);
        }

        public static Dictionary<string, List<Object>> getAttributeSuggestions(HttpSessionState session)
        {
            return (Dictionary<string, List<Object>>)session[SESSION_ATTRIBUTE_SUGGESTIONS];
        }

        public static void setPaymentContext(HttpSessionState session, PaymentContext paymentContext)
        {
            session.Add(SESSION_PAYMENT_CONTEXT, paymentContext);
        }

        public static PaymentContext getPaymentContext(HttpSessionState session)
        {
            return (PaymentContext)session[SESSION_PAYMENT_CONTEXT];
        }

        public static Dictionary<string, string> generateDeviceContextMap(string authenticationMessage, 
            string finishedMessage, List<String> identityProfiles)
        {
            Dictionary<string, string> deviceContextMap = new Dictionary<string, string>();
            deviceContextMap.Add(RequestConstants.AUTHENTICATION_MESSAGE, authenticationMessage);
            deviceContextMap.Add(RequestConstants.FINISHED_MESSAGE, finishedMessage);

            // identity profiles
            if (null != identityProfiles)
            {
                int i = 0;
                foreach (String identityProfile in identityProfiles)
                {
                    deviceContextMap.Add(RequestConstants.IDENTITY_PROFILE_PREFIX + "." + i, identityProfile);
                    i++;
                }
            }
            return deviceContextMap;
        }
    }
}
