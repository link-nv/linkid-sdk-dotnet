/*
 * linkID project.
 * 
 * Copyright 2006-2015 linkID N.V. All rights reserved.
 * linkID N.V. proprietary/confidential. Use is subject to license terms.
 */

using LinkIDWSNameSpace;
using AttributeWSNamespace;
using System;
using System.ServiceModel;
using System.Net.Security;
using System.ServiceModel.Security;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace safe_online_sdk_dotnet
{
    public class LinkIDServiceClientImpl : LinkIDServiceClient
    {
        private LinkIDServicePortClient client;

        public LinkIDServiceClientImpl(string location, string username, string password)
        {
            string address = "https://" + location + "/linkid-ws-username/linkid31";
            EndpointAddress remoteAddress = new EndpointAddress(address);

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

            this.client = new LinkIDServicePortClient(binding, remoteAddress);
            this.client.Endpoint.Behaviors.Add(new PasswordDigestBehavior(username, password));
        }

        public void enableLogging()
        {
            this.client.Endpoint.Behaviors.Add(new LoggingBehavior());
        }

        public LinkIDAuthSession authStart(LinkIDAuthenticationContext authenticationContext, string userAgent)
        {

            AuthStartRequest request = new AuthStartRequest();

            AuthnRequestType authnRequest = Saml2AuthUtil.generateAuthnRequest(authenticationContext);
            XmlDocument authnRequestDocument = Saml2AuthUtil.toXmlDocument(authnRequest);
            request.Any = authnRequestDocument.DocumentElement;

            request.language = authenticationContext.language;
            request.userAgent = userAgent;

            // operate
            AuthStartResponse response = this.client.authStart(request);

            // parse response
            if (null != response.error)
            {
                throw new LinkIDAuthException(response.error.error, response.error.info);
            }

            if (null != response.success)
            {
                return new LinkIDAuthSession(response.success.sessionId, convert(response.success.qrCodeInfo));
            }

            throw new RuntimeException("No success nor error element in the response ?!");

        }

        public LinkIDAuthPollResponse authPoll(String sessionId, String language)
        {
            AuthPollRequest request = new AuthPollRequest();

            request.sessionId = sessionId;
            request.language = language;

            // operate
            AuthPollResponse response = this.client.authPoll(request);

            // parse response
            if (null != response.error)
            {
                throw new LinkIDAuthPollException(response.error.error, response.error.info);
            }

            if (null != response.success)
            {
                LinkIDPaymentState paymentState = LinkIDPaymentState.STARTED;
                if (response.success.paymentStateSpecified)
                {
                    paymentState = convert(response.success.paymentState);
                }

                // parse authentication response
                LinkIDAuthnResponse linkIDAuthnResponse = null;
                if (null != response.success.authenticationResponse)
                {
                    linkIDAuthnResponse = convert(response.success.authenticationResponse);
                }

                return new LinkIDAuthPollResponse(response.success.authenticationState, paymentState,
                    response.success.paymentMenuURL, linkIDAuthnResponse);
            }

            throw new RuntimeException("No success nor error element in the response ?!");
        }

        public void authCancel(String sessionId)
        {
            AuthCancelRequest request = new AuthCancelRequest();

            request.sessionId = sessionId;

            // operate
            AuthCancelResponse response = client.authCancel(request);

            if (null != response.error)
            {
                throw new LinkIDAuthCancelException(response.error.error, response.error.info);
            }
        }

        // Helper methods

        private LinkIDQRInfo convert(QRCodeInfo qrCodeInfo)
        {
            // convert base64 encoded QR image
            byte[] qrCodeImage = Convert.FromBase64String(qrCodeInfo.qrEncoded);

            return new LinkIDQRInfo(qrCodeImage, qrCodeInfo.qrEncoded, qrCodeInfo.qrURL, qrCodeInfo.qrContent, qrCodeInfo.mobile);

        }

        private LinkIDPaymentState convert(PaymentStatusType wsPaymentState)
        {
            switch (wsPaymentState)
            {
                case PaymentStatusType.STARTED:
                    return LinkIDPaymentState.STARTED;
                case PaymentStatusType.FAILED:
                    return LinkIDPaymentState.FAILED;
                case PaymentStatusType.AUTHORIZED:
                    return LinkIDPaymentState.PAYED;
                case PaymentStatusType.REFUND_STARTED:
                    return LinkIDPaymentState.REFUND_STARTED;
                case PaymentStatusType.REFUNDED:
                    return LinkIDPaymentState.REFUNDED;
                case PaymentStatusType.WAITING_FOR_UPDATE:
                    return LinkIDPaymentState.WAITING_FOR_UPDATE;
            }

            throw new RuntimeException("Unexpected payment state:" + wsPaymentState);
        }

        private LinkIDAuthnResponse convert(XmlElement authnResponseElement)
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "Response";
            xRoot.Namespace = Saml2Constants.SAML2_PROTOCOL_NAMESPACE;

            XmlTextReader reader = new XmlTextReader(new StringReader(authnResponseElement.OuterXml));

            XmlSerializer serializer = new XmlSerializer(typeof(ResponseType), xRoot);
            ResponseType samlResponse = (ResponseType)serializer.Deserialize(reader);
            reader.Close();

            return Saml2AuthUtil.parse(samlResponse);

        }
    }
}
