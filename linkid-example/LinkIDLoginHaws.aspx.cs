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
        // location of this page, linkID will post its authentication response back to this location.
        private static string LOGINPAGE_LOCATION = "http://localhost:53825/LinkIDLoginHaws.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Allow any SSL certficate ( ONLY FOR DEVELOPMENT!! )
            ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback(WCFUtil.AnyCertificateValidationCallback);

            // handles request to start a linkID authentication and validates the incoming authentication responses
            LoginConfig.handleLinkIDWithHAWS(Request, Response, Session, TestUtil.SESSION_AUTH_CONTEXT, 
                TestUtil.LINKID_HOST, LOGINPAGE_LOCATION, TestUtil.wsUsername, TestUtil.wsPassword);
        }
    }
}
