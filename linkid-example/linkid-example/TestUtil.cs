using System;
using System.Web;
using System.Collections.Generic;
using safe_online_sdk_dotnet;

namespace linkid_example
{
    public class TestUtil
    {
        // linkID host to be used
        //public static string LINKID_HOST = "192.168.5.14:8443";
        public static string LINKID_HOST = "demo.linkid.be";

        // application details
        public static string APP_NAME = "example-mobile";

        // set the language to be used in the linkID iFrame
        public static string language = "en";

        // username,password case HAWS binding / WS-Security Password token is used
        public static string wsUsername = "example-mobile";
        public static string wsPassword = "6E6C1CB7-965C-48A0-B2B0-6B65674BE19F";

        // certificates and key locations case SAML Post / WS-Security X509 profile is used
        public static string KEY_DIR = "C:\\cygwin\\home\\devel\\keystores\\";
        public static string CERT_LINKID = KEY_DIR + "linkid.crt";
        public static string CERT_APP = KEY_DIR + "demotest.crt";
        public static string KEY_APP = KEY_DIR + "demotest.key";

        /*
         * linkID authentication context session attribute
         * 
         * After a successfull authentication with linkID this will hold the returned   
         * AuthenticationProtocolContext object which contains the linkID user ID,
         * used authentication device(s) and optionally the returned linkID attributes
         * for the application.
         */
        public static string SESSION_AUTH_CONTEXT = "linkID.authContext";

        public static string dumpAuthenticationContext(AuthenticationProtocolContext authContext)
        {
            string output = "<h1>Successfully authenticated</h1>";
            output += "<p>UserID=" + authContext.getUserId() +
                " authenticated using device " + authContext.getAuthenticatedDevices()[0] + "</p>";

            if (null != authContext.getPaymentResponse())
            {

                // log payment response
                output  += "<h2>Payment response</h2>";
                output += "  * Transaction ID = " + authContext.getPaymentResponse().txnId + "<br/>";
                output += "  * Transaction ID = " + authContext.getPaymentResponse().paymentState + "<br/>";
            }

            if (null != authContext.getAttributes())
            {
                // log attributes
                foreach (String key in authContext.getAttributes().Keys)
                {
                    output += "<p/>";
                    output += "<h2>Attribute=" + key + " via SAML Response</h2>";
                    output += logAttributes(authContext.getAttributes()[key]);
                }
            }

            return output;

        }

        public static string logAttributes(List<AttributeSDK> attributes)
        {
            string output = "";

            foreach (AttributeSDK attribute in attributes)
            {
                output += "AttributeID: " + attribute.getAttributeId() + "<br/>";
                if (attribute.getValue() is Compound)
                {
                    Compound compound = (Compound)attribute.getValue();
                    foreach (AttributeSDK member in compound.members)
                    {
                        output +=
                            "  * Member: " + member.getAttributeName() + " value=" + member.getValue() + "<br/>";
                    }
                }
                else
                {
                    output += "Value: " + attribute.getValue() + "<br/>";
                }
            }

            return output;
        }


    }
}
