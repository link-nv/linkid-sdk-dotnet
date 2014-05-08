using System;
using System.Collections.Generic;
using AttributeWSNamespace;


namespace safe_online_sdk_dotnet
{
    public class WSLoginUtil
    {

        public static AuthnSession startLinkIDAuthentication(string linkIDHost, string username, string password,
            string applicationName, string deviceContext, Dictionary<string, List<Object>> attributeSuggestions, 
            PaymentContext paymentContext, string language, string userAgent, bool forceRegistration)
        {

            Dictionary<string, string> deviceContextMap = new Dictionary<string, string>();
            deviceContextMap.Add(RequestConstants.DEVICE_CONTEXT_TITLE, deviceContext);

            // generate SAML v2.0 authentication request
            Saml2AuthUtil saml2AuthUtil = new Saml2AuthUtil();
            AuthnRequestType authnRequest = saml2AuthUtil.generateAuthnRequestObject(applicationName, null, null,
                "http://foo.bar", LoginConfig.getMobileMinimalPath(linkIDHost), false, deviceContextMap, 
                attributeSuggestions, paymentContext);

            // send request
            AuthClient client = new AuthClientImpl(linkIDHost, username, password);
            return client.start(saml2AuthUtil, authnRequest, language, userAgent, forceRegistration);
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
