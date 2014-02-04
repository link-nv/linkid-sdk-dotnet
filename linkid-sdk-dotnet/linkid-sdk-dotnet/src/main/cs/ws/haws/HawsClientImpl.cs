/*
 * SafeOnline project.
 * 
 * Copyright 2006-2008 	Lin.k N.V. All rights reserved.
 * Lin.k N.V. proprietary/confidential. Use is subject to license terms.
 */

using HawsWSNameSpace;
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
    public class HawsClientImpl : HawsClient
    {
        private HawsServicePortClient client;

        public HawsClientImpl(string location, string username, string password)
		{			
			string address = "https://" + location + "/linkid-ws-username/haws";
			EndpointAddress remoteAddress = new EndpointAddress(address);

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

            this.client = new HawsServicePortClient(binding, remoteAddress);
            this.client.Endpoint.Behaviors.Add(new PasswordDigestBehavior(username, password));
		}

        public HawsClientImpl(string location, X509Certificate2 appCertificate, X509Certificate2 linkidCertificate)
        {
            string address = "https://" + location + "/linkid-ws/haws";
            EndpointAddress remoteAddress = new EndpointAddress(address);

            this.client = new HawsServicePortClient(new LinkIDBinding(linkidCertificate), remoteAddress);


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

        public string push(AttributeWSNamespace.AuthnRequestType authnRequest, string language, string theme, LoginMode loginMode, StartPage startPage)
        {
            PushRequest request = new PushRequest();

            XmlDocument authnRequestDocument = Saml2AuthUtil.toXmlDocument(authnRequest);
            request.Any = authnRequestDocument.DocumentElement;

            request.language = language;
            request.theme = theme;
            request.loginMode = loginMode.ToString();
            request.startPage = startPage.ToString();

            // operate
            PushResponse response = this.client.push(request);

            // parse response
            if (null != response.error)
            {
                throw new HawsPushException(response.error.error, response.error.info);
            }

            if (null != response.sessionId)
            {
                return response.sessionId;
            }

            throw new RuntimeException("No success nor error element in the response ?!");
        }

        public AttributeWSNamespace.ResponseType pull(string sessionId)
        {
            PullRequest request = new PullRequest();
            request.sessionId = sessionId;

            // operate
            PullResponse response = this.client.pull(request);

            // parse response
            if (null != response.error)
            {
                throw new HawsPullException(response.error.error, response.error.info);
            }

            if (null != response.success)
            {
                XmlRootAttribute xRoot = new XmlRootAttribute();
                xRoot.ElementName = "Response";
                xRoot.Namespace = Saml2Constants.SAML2_PROTOCOL_NAMESPACE;

                XmlTextReader reader = new XmlTextReader(new StringReader(response.success.OuterXml));

                XmlSerializer serializer = new XmlSerializer(typeof(ResponseType), xRoot);
                ResponseType samlResponse = (ResponseType)serializer.Deserialize(reader);
                reader.Close();

                return samlResponse;
            }

            throw new RuntimeException("No success nor error element in the response ?!");
        }

    }
}
