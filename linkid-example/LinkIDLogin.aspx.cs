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
        // location of this page, linkID will post its authentication response back to this location.
        private static string LOGINPAGE_LOCATION = "http://localhost:53825/LinkIDLogin.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Allow any SSL certficate ( ONLY FOR DEVELOPMENT!! )
            ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback(WCFUtil.AnyCertificateValidationCallback);

            LoginConfig.handleLinkIDWithPOST(Request, Response, Session, TestUtil.SESSION_AUTH_CONTEXT,
                TestUtil.LINKID_HOST, LOGINPAGE_LOCATION, TestUtil.KEY_APP, TestUtil.CERT_APP, TestUtil.CERT_LINKID,
                this.form1, this.SAMLRequest, this.LanguageField);
            
        }
    }
}
