/*
 * SafeOnline project.
 * 
 * Copyright 2006-2008 	Lin.k N.V. All rights reserved.
 * Lin.k N.V. proprietary/confidential. Use is subject to license terms.
 */

using PaymentWSNameSpace;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
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

        public PaymentClientImpl(string location, string username, string password)
		{
            string address = "https://" + location + "/linkid-ws-username/payment30";
			EndpointAddress remoteAddress = new EndpointAddress(address);

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.MaxReceivedMessageSize = 2147483647;

            this.client = new PaymentServicePortClient(binding, remoteAddress);
            this.client.Endpoint.Behaviors.Add(new PasswordDigestBehavior(username, password));
		}

        public void enableLogging()
        {
            this.client.Endpoint.Behaviors.Add(new LoggingBehavior());
        }

        public LinkIDPaymentStatus getStatus(String orderReference)
        {
            PaymentStatusRequest request = new PaymentStatusRequest();
            request.orderReference = orderReference;
			PaymentStatusResponse response = this.client.status(request);

            List<LinkIDPaymentTransaction> transactions = new List<LinkIDPaymentTransaction>();
            if (null != response.paymentDetails.paymentTransactions)
            {
                foreach (PaymentTransactionV20 wsPaymentTransaction in response.paymentDetails.paymentTransactions)
                {
                    transactions.Add(new LinkIDPaymentTransaction(convert(wsPaymentTransaction.paymentMethodType),
                        wsPaymentTransaction.paymentMethod, convert(wsPaymentTransaction.paymentState),
                        wsPaymentTransaction.creationDate, wsPaymentTransaction.authorizationDate,
                        wsPaymentTransaction.capturedDate, wsPaymentTransaction.docdataReference,
                        wsPaymentTransaction.amount, convert(wsPaymentTransaction.currency)));
                }
            }

            List<LinkIDWalletTransaction> walletTransactions = new List<LinkIDWalletTransaction>();
            if (null != response.paymentDetails.walletTransactions)
            {
                foreach (WalletTransactionV20 wsWalletTransaction in response.paymentDetails.walletTransactions)
                {
                    walletTransactions.Add(new LinkIDWalletTransaction(wsWalletTransaction.walletId, wsWalletTransaction.creationDate,
                        wsWalletTransaction.transactionId, wsWalletTransaction.amount, convert(wsWalletTransaction.currency)));
                }
            }

            return new LinkIDPaymentStatus(convert(response.paymentStatus), response.captured, response.amountPayed,
                new LinkIDPaymentDetails(transactions, walletTransactions));
        }

        private LinkIDCurrency convert(Currency currency)
        {
            if (null == currency) return LinkIDCurrency.EUR;

            switch (currency)
            {
                case Currency.EUR: return LinkIDCurrency.EUR;          
            }

            return LinkIDCurrency.EUR;
        }

        private LinkIDPaymentState convert(PaymentStatusType paymentStatusType)
        {
            if (null == paymentStatusType) return LinkIDPaymentState.STARTED;

            switch (paymentStatusType)
            {
                case PaymentStatusType.STARTED: return LinkIDPaymentState.STARTED;
                case PaymentStatusType.WAITING_FOR_UPDATE: return LinkIDPaymentState.WAITING_FOR_UPDATE;
                case PaymentStatusType.AUTHORIZED: return LinkIDPaymentState.PAYED;
                case PaymentStatusType.DEFERRED: return LinkIDPaymentState.DEFERRED;
                case PaymentStatusType.FAILED: return LinkIDPaymentState.FAILED;
                case PaymentStatusType.REFUND_STARTED: return LinkIDPaymentState.REFUND_STARTED;
                case PaymentStatusType.REFUNDED: return LinkIDPaymentState.REFUNDED;
            }

            return LinkIDPaymentState.STARTED;
        }

        private LinkIDPaymentMethodType convert(PaymentMethodType paymentMethodType)
        {
            if (null == paymentMethodType) return LinkIDPaymentMethodType.UNKNOWN;

            switch (paymentMethodType)
            {
                case PaymentMethodType.UNKNOWN: return LinkIDPaymentMethodType.UNKNOWN;
                case PaymentMethodType.VISA: return LinkIDPaymentMethodType.VISA;
                case PaymentMethodType.MASTERCARD: return LinkIDPaymentMethodType.MASTERCARD;
                case PaymentMethodType.SEPA: return LinkIDPaymentMethodType.SEPA;
                case PaymentMethodType.KLARNA: return LinkIDPaymentMethodType.KLARNA;
            }

            return LinkIDPaymentMethodType.UNKNOWN;
        }
	}
}
