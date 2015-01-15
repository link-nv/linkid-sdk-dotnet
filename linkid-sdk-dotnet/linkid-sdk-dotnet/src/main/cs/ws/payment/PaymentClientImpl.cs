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
			string address = "https://" + location + "/linkid-ws/payment20";
			EndpointAddress remoteAddress = new EndpointAddress(address);

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

            this.client = new PaymentServicePortClient(binding, remoteAddress);
		}

        public void enableLogging()
        {
            this.client.Endpoint.Behaviors.Add(new LoggingBehavior());
        }

        public PaymentStatusDO getStatus(String orderReference)
        {

            PaymentStatusRequest request = new PaymentStatusRequest();
            request.orderReference = orderReference;
			PaymentStatusResponse response = this.client.status(request);

            PaymentState paymentState = PaymentState.STARTED;
            switch (response.paymentStatus)
            {
                case PaymentStatusType.STARTED:
                    paymentState = PaymentState.STARTED;
                    break;
                case PaymentStatusType.AUTHORIZED:
                    paymentState = PaymentState.PAYED;
                    break;
                case PaymentStatusType.FAILED:
                    paymentState = PaymentState.FAILED;
                    break;
                case PaymentStatusType.REFUNDED:
                    paymentState = PaymentState.REFUNDED;
                    break;
                case PaymentStatusType.REFUND_STARTED:
                    paymentState = PaymentState.REFUND_STARTED;
                    break;
            }

            return new PaymentStatusDO(paymentState, response.captured);
		}
	}
}
