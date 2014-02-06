using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using safe_online_sdk_dotnet;
using AttributeWSNamespace;

namespace linkid_example
{
    public partial class LinkIDLoginHaws : System.Web.UI.Page
    {
        // linkID host to be used
        //public static string LINKID_HOST = "192.168.5.14:8443";
        public static string LINKID_HOST = "demo.linkid.be";

        // location of this page, linkID will post its authentication response back to this location.
        private static string LOGINPAGE_LOCATION = "http://localhost:53825/LinkIDLoginHaws.aspx";

        // application details
        public static string APP_NAME = "demo-test";

        // set the language to be used in the linkID iFrame
        public static string language = "en";

        // username,password case HAWS binding / WS-Security Password token is used
        public static string wsUsername = "demo-test";
        public static string wsPassword = "08427E9F-6355-4DE4-B992-B1AB93CEE9D4";

        /*
         * linkID authentication context session attribute
         * 
         * After a successfull authentication with linkID this will hold the returned   
         * AuthenticationProtocolContext object which contains the linkID user ID,
         * used authentication device(s) and optionally the returned linkID attributes
         * for the application.
         */
        public static string SESSION_AUTH_CONTEXT = "linkID.authContext";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Allow any SSL certficate ( ONLY FOR DEVELOPMENT!! )
            ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback(WCFUtil.AnyCertificateValidationCallback);

            // handles request to start a linkID authentication and validates the incoming authentication responses
            LoginConfig.handleLinkIDWithHAWS(Request, Response, Session, SESSION_AUTH_CONTEXT, LINKID_HOST,
                APP_NAME, language, LOGINPAGE_LOCATION, wsUsername, wsPassword);
        }
    }
}
