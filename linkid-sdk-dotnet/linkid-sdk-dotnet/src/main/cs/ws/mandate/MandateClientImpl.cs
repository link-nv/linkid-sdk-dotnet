/*
 * SafeOnline project.
 * 
 * Copyright 2006-2008 	Lin.k N.V. All rights reserved.
 * Lin.k N.V. proprietary/confidential. Use is subject to license terms.
 */

using MandateWSNameSpace;
using System;
using System.ServiceModel;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace safe_online_sdk_dotnet
{
    public class MandateClientImpl : MandateClient
    {
        private MandateServicePortClient client;

        public MandateClientImpl(string location, string username, string password)
		{			
			string address = "https://" + location + "/linkid-ws-username/mandate";
			EndpointAddress remoteAddress = new EndpointAddress(address);

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

            this.client = new MandateServicePortClient(binding, remoteAddress);
            this.client.Endpoint.Behaviors.Add(new PasswordDigestBehavior(username, password));
		}

        public MandateClientImpl(string location, X509Certificate2 appCertificate, X509Certificate2 linkidCertificate)
        {
            string address = "https://" + location + "/linkid-ws/mandate";
            EndpointAddress remoteAddress = new EndpointAddress(address);

            this.client = new MandateServicePortClient(new LinkIDBinding(linkidCertificate), remoteAddress);


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

        public String pay(String mandateReference, PaymentContext paymentContextDO, String language)
        {
            MandatePaymentRequest request = new MandatePaymentRequest();

            MandateWSNameSpace.PaymentContext paymentContext = new MandateWSNameSpace.PaymentContext();
            paymentContext.amount = paymentContextDO.amount;
            paymentContext.currency = convert(paymentContextDO.currency);
            paymentContext.description = paymentContextDO.description;
            paymentContext.orderReference = paymentContextDO.orderReference;
            paymentContext.paymentProfile = paymentContextDO.paymentProfile;
            request.paymentContext = paymentContext;

            request.mandateReference = mandateReference;

            request.language = null != language ? language : "en";

            // operate
            MandatePaymentResponse response = this.client.pay(request);

            // parse response
            if (null != response.error)
            {
                throw new MandatePayException(response.error.errorCode);
            }

            if (null != response.success.orderReference)
            {
                return response.success.orderReference;
            }

            throw new RuntimeException("No success nor error element in the response ?!");

        }

        // helper methods

        private MandateWSNameSpace.Currency convert(Currency currency)
        {
            switch (currency)
            {
                case Currency.EUR:
                    return MandateWSNameSpace.Currency.EUR;
            }

            throw new RuntimeException("Currency " + currency.ToString() + " is not supported!");
        }

    }
}
