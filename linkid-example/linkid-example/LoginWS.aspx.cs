using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Serialization;
using safe_online_sdk_dotnet;

namespace linkid_example
{
    public partial class LoginWS : System.Web.UI.Page
    {
        public static String LINKID_SESSION = "LinkID.AuthnSession";

        protected void Page_Load(object sender, EventArgs e)
        {
            PollResult result = doLinkIDPoll();
            if (!result.hideQR)
            {
                qr.Src = "data:image/png;base64," + result.encodedQR;
                qr.Visible = true;
            }
            else
            {
                state.Text = result.info;
                qr.Visible = false;
            }
        }

        [WebMethod(EnableSession=true)]
        public static string pollLinkID()
        {
            PollResult result = doLinkIDPoll();
            return new JavaScriptSerializer().Serialize(result);

        }

        private static PollResult doLinkIDPoll()
        {
            // Allow any SSL certficate ( ONLY FOR DEVELOPMENT!! )
            ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback(WCFUtil.AnyCertificateValidationCallback);

            LinkIDAuthSession linkIDSession = (LinkIDAuthSession)HttpContext.Current.Session[LINKID_SESSION];
            PollResult result;

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
                linkIDSession = TestUtil.getClient().authStart(linkIDContext, HttpContext.Current.Request.UserAgent);

                //qr.Src = "data:image/png;base64," + linkIDSession.qrCodeInfo.qrEncoded;

                HttpContext.Current.Session[LINKID_SESSION] = linkIDSession;

                result = new PollResult(false, linkIDSession.qrCodeInfo.qrEncoded, false, null);
            }
            else
            {
                // poll linkID authentication
                LinkIDAuthPollResponse pollResponse = TestUtil.getClient().authPoll(linkIDSession.sessionId, TestUtil.language);

                String info = "";
                info = "<h2>Poll response</h2>";
                info += "AuthenticationState: " + pollResponse.linkIDAuthenticationState + "<br/>";
                info += "PaymentState: " + pollResponse.paymentState + "<br/>";
                info += "Payment Menu URL: " + pollResponse.paymentMenuURL + "<br/>";

                if (null != pollResponse.linkIDAuthnResponse)
                {
                    // logged in, dump user data
                    info += pollResponse.linkIDAuthnResponse.ToString();
                }

                result = new PollResult(true, null, null != pollResponse.linkIDAuthnResponse, info);
            }

            return result;
        }

        protected void onRestart(Object sender, EventArgs e)
        {
            Session[LINKID_SESSION] = null;
            Session.Abandon();
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        public class PollResult
        {
            public bool hideQR { get; set; }
            public String encodedQR { get; set; }
            public bool finished { get; set; }
            public String info { get; set; }

            public PollResult(bool hideQR, String encodedQR, bool finished, String info)
            {
                this.hideQR = hideQR;
                this.encodedQR = encodedQR;
                this.finished = finished;
                this.info = info;
            }
        }
    }
}
