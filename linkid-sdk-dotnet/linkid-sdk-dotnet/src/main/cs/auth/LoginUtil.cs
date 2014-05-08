using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;


namespace safe_online_sdk_dotnet
{
    public class LoginUtil
    {
        public static String SESSION_DEVICE_CONTEXT = "AuthnRequest.DeviceContext";
        public static String SESSION_ATTRIBUTE_SUGGESTIONS = "AuthnRequest.AttributeSuggestions";
        public static String SESSION_PAYMENT_CONTEXT = "AuthnRequest.PaymentContext";

        public static void setDeviceContext(HttpSessionState session, string deviceContext)
        {
            session.Add(SESSION_DEVICE_CONTEXT, deviceContext);
        }

        public static string getDeviceContext(HttpSessionState session)
        {
            return (string)session[SESSION_DEVICE_CONTEXT];
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

    }
}
