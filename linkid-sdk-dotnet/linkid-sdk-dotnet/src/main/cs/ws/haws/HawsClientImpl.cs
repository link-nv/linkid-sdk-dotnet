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

        public void enableLogging()
        {
            this.client.Endpoint.Behaviors.Add(new LoggingBehavior());
        }

        public string push(AttributeWSNamespace.AuthnRequestType authnRequest, string language)
        {
            PushRequestV2 request = new PushRequestV2();

            XmlDocument authnRequestDocument = Saml2AuthUtil.toXmlDocument(authnRequest);
            request.Any = authnRequestDocument.DocumentElement;

            request.language = language;

            // operate
            PushResponse response = this.client.pushV2(request);

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
