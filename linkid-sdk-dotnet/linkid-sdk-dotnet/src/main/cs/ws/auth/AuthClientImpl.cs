/*
 * SafeOnline project.
 * 
 * Copyright 2006-2008 	Lin.k N.V. All rights reserved.
 * Lin.k N.V. proprietary/confidential. Use is subject to license terms.
 */

using AuthWSNameSpace;
using System;
using System.ServiceModel;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;
using System.Xml;
using System.Xml.Serialization;
using AttributeWSNamespace;
using System.IO;

namespace safe_online_sdk_dotnet
{
    public class AuthClientImpl : AuthClient
    {
        private AuthServicePortClient client;

        public AuthClientImpl(string location, string username, string password)
		{			
			string address = "https://" + location + "/linkid-ws-username/auth";
			EndpointAddress remoteAddress = new EndpointAddress(address);

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

            this.client = new AuthServicePortClient(binding, remoteAddress);
            this.client.Endpoint.Behaviors.Add(new PasswordDigestBehavior(username, password));
		}

        public AuthClientImpl(string location, X509Certificate2 appCertificate, X509Certificate2 linkidCertificate)
        {
            string address = "https://" + location + "/linkid-ws/haws";
            EndpointAddress remoteAddress = new EndpointAddress(address);

            this.client = new AuthServicePortClient(new LinkIDBinding(linkidCertificate), remoteAddress);


            this.client.ClientCredentials.ClientCertificate.Certificate = appCertificate;
            this.client.ClientCredentials.ServiceCertificate.DefaultCertificate = linkidCertificate;
            // To override the validation for our self-signed test certificates
            this.client.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;

            this.client.Endpoint.Contract.ProtectionLevel = ProtectionLevel.Sign;

        }

        public void enableLogging()
        {
            this.client.Endpoint.Behaviors.Add(new LoggingBehavior());
        }

        public AuthnSession start(Saml2AuthUtil saml2AuthUtil, AttributeWSNamespace.AuthnRequestType authnRequest, 
            string language, string userAgent, bool forceRegistration)
        {
            StartRequest request = new StartRequest();

            XmlDocument authnRequestDocument = Saml2AuthUtil.toXmlDocument(authnRequest);
            request.Any = authnRequestDocument.DocumentElement;

            request.language = language;
            request.userAgent = userAgent;
            request.forceRegistration = forceRegistration;

            // operate
            StartResponse response = this.client.start(request);

            // parse response
            if (null != response.error)
            {
                throw new AuthnException(response.error.error, response.error.info);
            }

            if (null != response.success)
            {

                // convert base64 encoded QR image
                byte[] qrCodeImage = Convert.FromBase64String(response.success.encodedQRCode);

                return new AuthnSession(response.success.sessionId, qrCodeImage, 
                    response.success.encodedQRCode, response.success.qrCodeURL,
                    response.success.authenticationContext, saml2AuthUtil);
            }

            throw new RuntimeException("No success nor error element in the response ?!");
        }

        public PollResponse poll(Saml2AuthUtil saml2AuthUtil, string sessionId, string language)
        {

            PollRequest request = new PollRequest();

            request.sessionId = sessionId;
            request.language = language;

            // operate
            AuthWSNameSpace.PollResponse response = this.client.poll(request);

            // parse response
            if (null != response.error)
            {
                throw new PollException(response.error.error, response.error.info);
            }

            if (null != response.success)
            {
                
                // check authentication response present and parse
                AuthenticationProtocolContext authenticationContext = null;
                if (null != response.success.authenticationResponse)
                {
                    XmlRootAttribute xRoot = new XmlRootAttribute();
                    xRoot.ElementName = "Response";
                    xRoot.Namespace = Saml2Constants.SAML2_PROTOCOL_NAMESPACE;

                    XmlTextReader reader = new XmlTextReader(new StringReader(response.success.authenticationResponse.OuterXml));

                    XmlSerializer serializer = new XmlSerializer(typeof(ResponseType), xRoot);
                    ResponseType samlResponse = (ResponseType)serializer.Deserialize(reader);
                    reader.Close();

                    authenticationContext = saml2AuthUtil.parseAuthnResponse(samlResponse);

                }

                return new PollResponse(response.success.authenticationState, response.success.paymentState,
                    response.success.paymentMenuURL, authenticationContext);

            }

            throw new RuntimeException("No success nor error element in the response ?!");

        }

    }
}
