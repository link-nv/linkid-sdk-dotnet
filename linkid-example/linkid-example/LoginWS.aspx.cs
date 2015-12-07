using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using safe_online_sdk_dotnet;

namespace linkid_example
{
    public partial class LoginWS : System.Web.UI.Page
    {
        public static String LINKID_SESSION = "LinkID.AuthnSession";

        protected void Page_Load(object sender, EventArgs e)
        {

            // Allow any SSL certficate ( ONLY FOR DEVELOPMENT!! )
            ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback(WCFUtil.AnyCertificateValidationCallback);

            LinkIDAuthSession linkIDSession = (LinkIDAuthSession)Session[LINKID_SESSION];

            if (null == linkIDSession)
            {
                // configure linkID authentication context
                LinkIDAuthenticationContext linkIDContext = new LinkIDAuthenticationContext();
                linkIDContext.authenticationMessage = "WS Authn Message";
                linkIDContext.finishedMessage = "WS Finished Message";
                linkIDContext.applicationName = TestUtil.APP_NAME;
                linkIDContext.language = TestUtil.language;
                linkIDContext.identityProfile = "linkid_basic";
                
                // attribute suggestions
                Dictionary<string, List<Object>> attributeSuggestions = new Dictionary<string, List<object>>();
                attributeSuggestions.Add("test.attribute.string", new List<Object> { "test" });
                attributeSuggestions.Add("test.attribute.multi.date", new List<Object> { DateTime.Now });
                attributeSuggestions.Add("test.attribute.boolean", new List<Object> { true });
                attributeSuggestions.Add("test.attribute.integer", new List<Object> { 69 });
                attributeSuggestions.Add("test.attribute.double", new List<Object> { 3.14159 });
                linkIDContext.attributeSuggestions = attributeSuggestions;

                // linkIDContext.paymentContext = new LinkIDPaymentContext(new LinkIDPaymentAmount(199, LinkIDCurrency.EUR, null));
                // linkIDContext.callback = new LinkIDCallback("https://www.google.be", null, true);

                // start the linkID authentication
                linkIDSession = TestUtil.getClient().authStart(linkIDContext, Request.UserAgent);

                qr.Src = "data:image/png;base64," + linkIDSession.qrCodeInfo.qrEncoded;

                Session[LINKID_SESSION] = linkIDSession;
            }
            else
            {
                // poll linkID authentication
                LinkIDAuthPollResponse pollResponse = TestUtil.getClient().authPoll(linkIDSession.sessionId, TestUtil.language);

                state.Text = "<h2>Poll response</h2>";
                state.Text += "AuthenticationState: " + pollResponse.linkIDAuthenticationState + "<br/>";
                state.Text += "PaymentState: " + pollResponse.paymentState + "<br/>";
                state.Text += "Payment Menu URL: " + pollResponse.paymentMenuURL + "<br/>";

                if (null != pollResponse.linkIDAuthnResponse)
                {
                    // logged in, dump user data
                    state.Text += pollResponse.linkIDAuthnResponse.ToString();
                }

                // hide qr image
                qr.Visible = false;
            }
        }

        protected void onRestart(Object sender, EventArgs e)
        {
            Session[LINKID_SESSION] = null;
            Session.Abandon();
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }
    }
}
