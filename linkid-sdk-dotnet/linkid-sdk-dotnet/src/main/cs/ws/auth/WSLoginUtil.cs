using System;
using System.Collections.Generic;
using AttributeWSNamespace;


namespace safe_online_sdk_dotnet
{
    public class WSLoginUtil
    {

        public static AuthnSession startLinkIDAuthentication(string linkIDHost, string username, string password,
            LinkIDAuthenticationContext linkIDContext, string userAgent)
        {

            // generate SAML v2.0 authentication request
            Saml2AuthUtil saml2AuthUtil = new Saml2AuthUtil();
            AuthnRequestType authnRequest = saml2AuthUtil.generateAuthnRequestObject(linkIDContext, 
                "http://foo.bar", LoginConfig.getMobileMinimalPath(linkIDHost));

            // send request
            AuthClient client = new AuthClientImpl(linkIDHost, username, password);
            return client.start(saml2AuthUtil, authnRequest, linkIDContext.language, userAgent);
        }

        public static PollResponse pollLinkIDAuthentication(string linkIDHost, string username, string password, 
            Saml2AuthUtil saml2AuthUtil, string sessionId, string language)
        {

            // poll
            AuthClient client = new AuthClientImpl(linkIDHost, username, password);
            return client.poll(saml2AuthUtil, sessionId, language);
        }
    }
}
