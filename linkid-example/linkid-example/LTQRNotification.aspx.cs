using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Security;


using safe_online_sdk_dotnet;

/*
 * Example Long term QR notification page.
 * This page will be poked by linkID if a linkID user pays one of your long term QR sessions.
 * 
 * e.g. http://localhost:53825/LTQRNotification.aspx?id=c7353cd2-a3e3-41a7-9a31-d7b659d5cce1&clientSessionId=011e9a4e-7e3c-4c49-b0fd-ae2572e1ebb3
 * 
 * linkID will do a HTTP GET to here with 2 query parameters, the session ID and the client session ID.
 * The page will then fetch the session information using the LTQR WS client.
 */

namespace linkid_example
{
    public partial class LTQRNotification : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String sessionId = Request[RequestConstants.LTQR_SESSION_ID_PARAM];
            String clientSessionId = Request[RequestConstants.LTQR_CLIENT_SESSION_ID_PARAM];
            if (null == sessionId)
            {
                // no sessionId in notification, ignoring...
                return;
            }

            // disable ssl validation, ONLY FOR DEVELOPMENT!
//            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(WCFUtil.AnyCertificateValidationCallback);

            // fetch session
            LTQRClient client = new LTQRClientImpl(LinkIDLoginHaws.LINKID_HOST,
                LinkIDLoginHaws.wsUsername, LinkIDLoginHaws.wsPassword);
            LTQRClientSession[] clientSessions = client.pull(new string[]{sessionId}, new string[]{clientSessionId});
            String output = "";
            foreach (LTQRClientSession clientSession in clientSessions)
            {
                output += clientSession.ToString() + "<br />";
            }

            this.OutputLabel.Text = "<h1>LTQR Client sessions</h1>";
            this.OutputLabel.Text += output;

        }
    }
}
