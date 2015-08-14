﻿using System;
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

            AuthnSession authnSession = (AuthnSession)Session[LINKID_SESSION];

            if (null == authnSession)
            {
                // start a linkID authentication
                LinkIDAuthenticationContext linkIDContext = new LinkIDAuthenticationContext();
                linkIDContext.authenticationMessage = "WS Authn Message";
                linkIDContext.finishedMessage = "WS Finished Message";
                linkIDContext.applicationName = TestUtil.APP_NAME;
                linkIDContext.language = TestUtil.language;

                authnSession = WSLoginUtil.startLinkIDAuthentication(TestUtil.LINKID_HOST,
                    TestUtil.wsUsername, TestUtil.wsPassword, linkIDContext, null);

                qr.Src = "data:image/png;base64," + authnSession.qrCodeImageEncoded;

                Session[LINKID_SESSION] = authnSession;
            }
            else
            {
                // poll linkID authentication
                PollResponse pollResponse =  WSLoginUtil.pollLinkIDAuthentication(TestUtil.LINKID_HOST,
                    TestUtil.wsUsername, TestUtil.wsPassword,
                    authnSession.saml2AuthUtil, authnSession.sessionId, TestUtil.language);

                state.Text = "<h2>Poll response</h2>";
                state.Text += "AuthenticationState: " + pollResponse.authenticationState + "<br/>";
                state.Text += "PaymentState: " + pollResponse.paymentState + "<br/>";
                state.Text += "Payment Menu URL: " + pollResponse.paymentMenuURL + "<br/>";

                if (null != pollResponse.authenticationContext)
                {
                    // logged in, dump user data
                    state.Text += pollResponse.authenticationContext.ToString();
                }

                // hide qr image
                qr.Visible = false;
            }
        }
    }
}