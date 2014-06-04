using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using AttributeWSNamespace;

namespace safe_online_sdk_dotnet
{
    public class LoginConfig
    {
        private static string SESSION_SAML2_AUTH_UTIL = "linkID.saml2AuthUtil";
        private static string SESSION_LOGIN_CONFIG = "linkID.loginConfig";

        public bool mobileMinimal { get; set; }
        public bool mobileForceRegistration { get; set; }
        public String targetURI { get; set; }
        public String linkIDLandingPage { get; set; }

        public static Saml2AuthUtil getSaml2AuthUtil(HttpSessionState session)
        {
            return (Saml2AuthUtil)session[SESSION_SAML2_AUTH_UTIL];
        }

        public static void storeSaml2AuthUtil(HttpSessionState session, Saml2AuthUtil saml2AuthUtil)
        {
            session[SESSION_SAML2_AUTH_UTIL] = saml2AuthUtil;
        }

        public static LoginConfig getLoginConfig(HttpSessionState session)
        {
            return (LoginConfig)session[SESSION_LOGIN_CONFIG];
        }

        public static string getMobileMinimalPath(string linkIDHost)
        {
            return "https://" + linkIDHost + "/linkid-qr/auth-min";
        }

        public static string getMobileMinimalRegPath(string linkIDHost)
        {
            return "https://" + linkIDHost + "/linkid-qr/reg-min";
        }

        public LoginConfig(HttpRequest request, HttpSessionState session, String linkIDHost)
        {
            string LINKID_MOBILE_MINIMAL_ENTRY = getMobileMinimalPath(linkIDHost);
            string LINKID_MOBILE_REG_MINIMAL_ENTRY = getMobileMinimalRegPath(linkIDHost);

            mobileForceRegistration = null != request[RequestConstants.MOBILE_FORCE_REG_REQUEST_PARAM];
            targetURI = request[RequestConstants.TARGET_URI_REQUEST_PARAM];

            if (mobileForceRegistration)
                linkIDLandingPage = LINKID_MOBILE_REG_MINIMAL_ENTRY;
            else
                linkIDLandingPage = LINKID_MOBILE_MINIMAL_ENTRY;

            // store on session
            session[SESSION_LOGIN_CONFIG] = this;

        }

        public void finalize(HttpResponse response)
        {
            response.ContentType = "text/html";
            response.Write("<html>");
            response.Write("<head>");
            response.Write("<script type=\"text/javascript\">");
            response.Write("window.top.location.replace(\"" + targetURI + "\");");
            response.Write("</script>");
            response.Write("</head>");
            response.Write("<body>");
            response.Write("<noscript><p>You are successfully logged in. Since your browser does not support JavaScript, you must close this popup window and refresh the original window manually.</p></noscript>");
            response.Write("</body>");
            response.Write("</html>");
        }

        public String generateHawsRedirectURL(String sessionId)
        {
            return linkIDLandingPage + "?" + RequestConstants.HAWS_SESSION_ID_PARAM + "=" + sessionId;
        }

        public static void handleLinkIDWithPOST(HttpRequest request, HttpResponse response, HttpSessionState session,
            String authnContextSessionParam, String linkIDHost, String applicationName, String language,
            String targetURL, String privateKeyPemFile, String appCertPemFile, String linkIDCertPemFile,
            HtmlForm form, HiddenField SAMLRequestField, HiddenField LanguageField)
        {
            // Load applications's keypair and linkID's certificate
            RSACryptoServiceProvider applicationKey = KeyStoreUtil.GetPrivateKeyFromPem(privateKeyPemFile, true);
            X509Certificate2 applicationCert = new X509Certificate2(appCertPemFile);
            applicationCert.PrivateKey = applicationKey;
            X509Certificate2 linkidCert = new X509Certificate2(linkIDCertPemFile);

            handleLinkIDWithPOST(request, response, session, authnContextSessionParam, linkIDHost, applicationName,
                language, targetURL, applicationCert, linkidCert,
                form, SAMLRequestField, LanguageField);
        }

        public static void handleLinkIDWithPOST(HttpRequest request, HttpResponse response, HttpSessionState session,
            String authnContextSessionParam, String linkIDHost, String applicationName, String language,
            String targetURL, X509Certificate2 applicationCert, X509Certificate2 linkidCert,
            HtmlForm form, HiddenField SAMLRequestField, HiddenField LanguageField)
        {
            string[] responses = request.Form.GetValues(RequestConstants.SAML2_POST_BINDING_RESPONSE_PARAM);

            /*
             * Check if "force" query param is present.
             * If set, an authentication will be started, regardless if the user was already logged in.
             * For e.g. linkID payments...
             */
            bool forceAuthentication = null != request.QueryString[RequestConstants.FORCE_AUTH_REQUEST_PARAM];

            /*
             * If a SAML2 response was found but no authentication context was on the session we received a
             * SAML2 authentication response.
             */
            if (null != responses && null == session[authnContextSessionParam])
            {
                Saml2AuthUtil saml2AuthUtil = LoginConfig.getSaml2AuthUtil(session);
                LoginConfig loginConfig = LoginConfig.getLoginConfig(session);
                string encodedSaml2Response = responses[0];

                AuthenticationProtocolContext
                     context = saml2AuthUtil.validateEncodedAuthnResponse(encodedSaml2Response, linkIDHost,
                                                                          applicationCert, linkidCert);
                session[authnContextSessionParam] = context;

                loginConfig.finalize(response);
            }

            /*
             * No authentication context found so not yet logged in.
             * 
             * Generate a SAML2 authentication request and store in the hiddenfield.
             * Put the used authentication utility class on the session.
             */
            if (null == session[authnContextSessionParam] || forceAuthentication)
            {
                if (forceAuthentication)
                {
                    session[authnContextSessionParam] = null;
                }

                /*
                 * Check page's request parameters.
                 * They will contain e.g. 
                 *   what linkID authentication mode (mobile, direct, ....), 
                 *   optional target URL to redirect to after handling a linkID authentication response, ...
                 */
                LoginConfig loginConfig = new LoginConfig(request, session, linkIDHost);

                // Construct the SAML v2.0 Authentication request and fill in the form parameters
                Saml2AuthUtil saml2AuthUtil = new Saml2AuthUtil((RSACryptoServiceProvider)applicationCert.PrivateKey);
                LoginConfig.storeSaml2AuthUtil(session, saml2AuthUtil);

                form.Action = loginConfig.linkIDLandingPage;

                // device context
                string deviceAuthnMessage = LoginUtil.getDeviceAuthnMessage(session);
                string deviceFinishedMessage = LoginUtil.getDeviceFinishedMessage(session);
                Dictionary<string, string> deviceContextMap = new Dictionary<string, string>();
                deviceContextMap.Add(RequestConstants.AUTHENTICATION_MESSAGE, deviceAuthnMessage);
                deviceContextMap.Add(RequestConstants.FINISHED_MESSAGE, deviceFinishedMessage);

                // attribute suggestions
                Dictionary<string, List<Object>> attributeSuggestions = LoginUtil.getAttributeSuggestions(session);

                // payment context
                PaymentContext paymentContext = LoginUtil.getPaymentContext(session);

                SAMLRequestField.ID = RequestConstants.SAML2_POST_BINDING_REQUEST_PARAM;
                SAMLRequestField.Value = saml2AuthUtil.generateEncodedAuthnRequest(applicationName, null, null,
                    targetURL, loginConfig.linkIDLandingPage, false, deviceContextMap, attributeSuggestions,
                    paymentContext);

                if (null != language)
                {
                    LanguageField.ID = RequestConstants.LANGUAGE_REQUEST_PARAM;
                    LanguageField.Value = language;
                }
            }
        }

        public static void handleLinkIDWithHAWS(HttpRequest request, HttpResponse response, HttpSessionState session,
            String authnContextSessionParam, String linkIDHost, String applicationName, String language,
            String targetURL, String username, String password)
        {
            String hawsSessionId = request[RequestConstants.HAWS_SESSION_ID_PARAM];

            /*
             * Check if "force" query param is present.
             * If set, an authentication will be started, regardless if the user was already logged in.
             * For e.g. linkID payments...
             */
            bool forceAuthentication = null != request.QueryString[RequestConstants.FORCE_AUTH_REQUEST_PARAM];

            /*
             * If a SAML2 response was found but no authentication context was on the session we received a
             * SAML2 authentication response.
             */
            if (null != hawsSessionId && null == session[authnContextSessionParam])
            {
                Saml2AuthUtil saml2AuthUtil = LoginConfig.getSaml2AuthUtil(session);
                LoginConfig loginConfig = LoginConfig.getLoginConfig(session);

                // fetch SAML 2.0 authentication response from linkID
                HawsClient hawsClient = new HawsClientImpl(linkIDHost, username, password);
                ResponseType saml2Response = hawsClient.pull(hawsSessionId);

                AuthenticationProtocolContext context = saml2AuthUtil.parseAuthnResponse(saml2Response);
                session[authnContextSessionParam] = context;

                loginConfig.finalize(response);
            }

            /*
             * No authentication context found so not yet logged in.
             * 
             * Generate a SAML2 authentication request and store in the hiddenfield.
             * Put the used authentication utility class on the session.
             */
            if (null == session[authnContextSessionParam] || forceAuthentication)
            {
                if (forceAuthentication)
                {
                    session[authnContextSessionParam] = null;
                }

                /*
                 * Check page's request parameters.
                 * They will contain e.g. 
                 *   what linkID authentication mode (mobile, direct, ....), 
                 *   optional target URL to redirect to after handling a linkID authentication response, ...
                 */
                LoginConfig loginConfig = new LoginConfig(request, session, linkIDHost);

                // Construct the SAML v2.0 Authentication request and fill in the form parameters
                Saml2AuthUtil saml2AuthUtil = new Saml2AuthUtil();
                LoginConfig.storeSaml2AuthUtil(session, saml2AuthUtil);

                // device context
                string deviceAuthnMessage = LoginUtil.getDeviceAuthnMessage(session);
                string deviceFinishedMessage = LoginUtil.getDeviceFinishedMessage(session);
                Dictionary<string, string> deviceContextMap = new Dictionary<string, string>();
                deviceContextMap.Add(RequestConstants.AUTHENTICATION_MESSAGE, deviceAuthnMessage);
                deviceContextMap.Add(RequestConstants.FINISHED_MESSAGE, deviceFinishedMessage);

                // attribute suggestions
                Dictionary<string, List<Object>> attributeSuggestions = LoginUtil.getAttributeSuggestions(session);

                // payment context
                PaymentContext paymentContext = LoginUtil.getPaymentContext(session);

                // generate authn request
                AuthnRequestType authnRequest = saml2AuthUtil.generateAuthnRequestObject(applicationName, null, null,
                    targetURL, loginConfig.linkIDLandingPage, false, deviceContextMap,
                    attributeSuggestions, paymentContext);

                // push authn request to linkID
                HawsClient hawsClient = new HawsClientImpl(linkIDHost, username, password);
                String sessionId = hawsClient.push(authnRequest, language);

                // redirect
                response.Redirect(loginConfig.generateHawsRedirectURL(sessionId));
            }
        }
    }
}
