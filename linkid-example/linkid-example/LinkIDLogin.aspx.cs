using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Security.Cryptography;
using System.Net;
using System.Net.Security;
using System.IO;
using System.Collections.Specialized;

using safe_online_sdk_dotnet;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using System.Xml;


namespace linkid_example
{
    public partial class LinkIDLogin : System.Web.UI.Page
    {         
        /*
         * Application specific configuration...
         */
        // linkID host to be used
        //public static string LINKID_HOST = "192.168.5.14:8443";
        public static string LINKID_HOST = "demo.linkid.be";

        // location of this page, linkID will post its authentication response back to this location.
        private static string LOGINPAGE_LOCATION = "http://localhost:53825/LinkIDLogin.aspx";

        // application details
        public static string APP_NAME = "demo-test";

        // set the language to be used in the linkID iFrame
        public static string language = "nl";

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

        protected void Page_Load(object sender, EventArgs e)
        {
            // Allow any SSL certficate ( ONLY FOR DEVELOPMENT!! )
            ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback(WCFUtil.AnyCertificateValidationCallback);

            LoginConfig.handleLinkIDWithPOST(Request, Response, Session, SESSION_AUTH_CONTEXT, LINKID_HOST,
                APP_NAME, language, LOGINPAGE_LOCATION, KEY_APP, CERT_APP, CERT_LINKID,
                this.form1, this.SAMLRequest, this.LanguageField);
            
        }
    }
}
