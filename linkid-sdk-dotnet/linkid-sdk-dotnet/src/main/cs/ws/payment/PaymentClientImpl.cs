/*
 * SafeOnline project.
 * 
 * Copyright 2006-2008 	Lin.k N.V. All rights reserved.
 * Lin.k N.V. proprietary/confidential. Use is subject to license terms.
 */

using PaymentWSNameSpace;
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;

namespace safe_online_sdk_dotnet
{
	/// <summary>
	/// Client implementation of the linkID ID Mapping Web Service.
	/// </summary>
	public class PaymentClientImpl : PaymentClient
	{
		private PaymentServicePortClient client;

        public PaymentClientImpl(string location)
		{			
			string address = "https://" + location + "/linkid-ws/payment";
			EndpointAddress remoteAddress = new EndpointAddress(address);

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

            this.client = new PaymentServicePortClient(binding, remoteAddress);
		}

        public void enableLogging()
        {
            this.client.Endpoint.Behaviors.Add(new LoggingBehavior());
        }
		
		public PaymentState getStatus(String transactionId) {

            PaymentStatusRequest request = new PaymentStatusRequest();
            request.transactionId = transactionId;
			PaymentStatusResponse response = this.client.status(request);

            switch (response.paymentStatus)
            {
                case PaymentStatusType.STARTED:
                    return PaymentState.STARTED;
                case PaymentStatusType.AUTHORIZED:
                    return PaymentState.PAYED;
                case PaymentStatusType.FAILED:
                    return PaymentState.FAILED;
                case PaymentStatusType.REFUNDED:
                    return PaymentState.REFUNDED;
                case PaymentStatusType.REFUND_STARTED:
                    return PaymentState.REFUND_STARTED;
            }

            throw new RuntimeException("Payment state type " + response.paymentStatus + "is not supported!");
		}
	}
}
