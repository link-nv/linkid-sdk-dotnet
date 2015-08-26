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
			string address = "https://" + location + "/linkid-ws-username/mandate30";
			EndpointAddress remoteAddress = new EndpointAddress(address);

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

            this.client = new MandateServicePortClient(binding, remoteAddress);
            this.client.Endpoint.Behaviors.Add(new PasswordDigestBehavior(username, password));
		}

        public void enableLogging()
        {
            this.client.Endpoint.Behaviors.Add(new LoggingBehavior());
        }

        public String pay(String mandateReference, LinkIDPaymentContext linkIDPaymentContext, String language)
        {
            MandatePaymentRequest request = new MandatePaymentRequest();

            MandateWSNameSpace.PaymentContextV20 paymentContext = new MandateWSNameSpace.PaymentContextV20();
            paymentContext.amount = linkIDPaymentContext.amount.amount;
            paymentContext.currencySpecified = linkIDPaymentContext.amount.currency.HasValue;
            if (linkIDPaymentContext.amount.currency.HasValue)
            {
                paymentContext.currency = convert(linkIDPaymentContext.amount.currency.Value);
            }
            paymentContext.walletCoin = linkIDPaymentContext.amount.walletCoin;
            paymentContext.description = linkIDPaymentContext.description;
            paymentContext.orderReference = linkIDPaymentContext.orderReference;
            paymentContext.paymentProfile = linkIDPaymentContext.paymentProfile;
            paymentContext.validationTime = linkIDPaymentContext.paymentValidationTime;
            paymentContext.allowPartial = linkIDPaymentContext.allowPartial;
            paymentContext.onlyWallets = linkIDPaymentContext.onlyWallets;
            paymentContext.paymentStatusLocation = linkIDPaymentContext.paymentStatusLocation;
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

        private MandateWSNameSpace.Currency convert(LinkIDCurrency currency)
        {
            switch (currency)
            {
                case LinkIDCurrency.EUR:
                    return MandateWSNameSpace.Currency.EUR;
            }

            throw new RuntimeException("Currency " + currency.ToString() + " is not supported!");
        }

    }
}
