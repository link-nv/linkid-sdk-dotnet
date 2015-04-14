﻿/*
 * SafeOnline project.
 * 
 * Copyright 2006-2008 	Lin.k N.V. All rights reserved.
 * Lin.k N.V. proprietary/confidential. Use is subject to license terms.
 */

using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;
using ReportingWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class ReportingClientImpl : ReportingClient
    {
        private ReportingServicePortClient client;

        public ReportingClientImpl(string location, string username, string password)
		{			
			string address = "https://" + location + "/linkid-ws-username/reporting20";
			EndpointAddress remoteAddress = new EndpointAddress(address);

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.MaxReceivedMessageSize = 2147483647;

            this.client = new ReportingServicePortClient(binding, remoteAddress);
            this.client.Endpoint.Behaviors.Add(new PasswordDigestBehavior(username, password));
		}

        public ReportingClientImpl(string location, X509Certificate2 appCertificate, X509Certificate2 linkidCertificate)
        {
            string address = "https://" + location + "/linkid-ws/reporting20";
            EndpointAddress remoteAddress = new EndpointAddress(address);

            this.client = new ReportingServicePortClient(new LinkIDBinding(linkidCertificate), remoteAddress);

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

        public List<LinkIDPaymentOrder> getPaymentReport(DateTime startDate, DateTime endDate)
        {
            return getPaymentReport(startDate, endDate, null, null);
        }

        public List<LinkIDPaymentOrder> getPaymentReportForOrderReferences(List<String> orderReferences)
        {
            return getPaymentReport(null, null, orderReferences, null);
        }

        public List<LinkIDPaymentOrder> ﻿getPaymentReportForMandates(List<String> mandateReferences)
        {
            return getPaymentReport(null, null, null, mandateReferences);
        }

        public List<LinkIDParkingSession> getParkingReport(DateTime startDate, DateTime endDate)
        {
            return getParkingReport(startDate, endDate, null, null, null, null);
        }

        public List<LinkIDParkingSession> getParkingReport(DateTime startDate, DateTime endDate, List<String> parkings)
        {
            return getParkingReport(startDate, endDate, null, null, null, parkings);
        }

        public List<LinkIDParkingSession> getParkingReportForBarCodes(List<String> barCodes)
        {
            return getParkingReport(null, null, barCodes, null, null, null);
        }

        public List<LinkIDParkingSession> getParkingReportForTicketNumbers(List<String> ticketNumbers)
        {
            return getParkingReport(null, null, null, ticketNumbers, null, null);
        }

        public List<LinkIDParkingSession> getParkingReportForDTAKeys(List<String> dtaKeys)
        {
            return getParkingReport(null, null, null, null, dtaKeys, null);
        }

        public List<LinkIDParkingSession> getParkingReportForParkings(List<String> parkings)
        {
            return getParkingReport(null, null, null, null, null, parkings);
        }


        // Helper methods

        private List<LinkIDParkingSession> getParkingReport(Nullable<DateTime> startDate, Nullable<DateTime> endDate,
            List<String> barCodes, List<String> ticketNumbers, List<String> dtaKeys, List<String> parkings)
        {
            ParkingReportRequest request = new ParkingReportRequest();

            if (null != startDate)
            {
                request.startDate = startDate.Value;
                request.startDateSpecified = true;
            }

            if (null != endDate)
            {
                request.endDate = endDate.Value;
                request.endDateSpecified = true;
            }

            if (null != barCodes)
            {
                request.barCodes = barCodes.ToArray();
            }

            if (null != ticketNumbers)
            {
                request.ticketNumbers = ticketNumbers.ToArray();
            }

            if (null != dtaKeys)
            {
                request.dtaKeys = dtaKeys.ToArray();
            }

            if (null != parkings)
            {
                request.parkings = parkings.ToArray();
            }

            ReportingWSNameSpace.ParkingSession[] wsParkingSessions = this.client.parkingReport(request);

            List<LinkIDParkingSession> parkingSessions = new List<LinkIDParkingSession>();
            foreach (ReportingWSNameSpace.ParkingSession wsParkingSession in wsParkingSessions)
            {
                parkingSessions.Add(convert(wsParkingSession));
            }

            return parkingSessions;
        }

        private List<LinkIDPaymentOrder> getPaymentReport(Nullable<DateTime> startDate, Nullable<DateTime> endDate,
            List<String> orderReferences, List<String> mandateReferences)
        {
            PaymentReportRequest request = new PaymentReportRequest();

            if (null != startDate)
            {
                request.startDate = startDate.Value;
                request.startDateSpecified = true;
            }

            if (null != endDate)
            {
                request.endDate = endDate.Value;
                request.endDateSpecified = true;
            }

            if (null != orderReferences)
            {
                request.orderReferences = orderReferences.ToArray();
            }

            if (null != mandateReferences)
            {
                request.mandateReferences = mandateReferences.ToArray();
            }

            ReportingWSNameSpace.PaymentOrder[] wsOrders= this.client.paymentReport(request);

            List<LinkIDPaymentOrder> orders = new List<LinkIDPaymentOrder>();
            foreach (ReportingWSNameSpace.PaymentOrder wsOrder in wsOrders)
            {
                orders.Add(convert(wsOrder));
            }

            return orders;
        }

        private LinkIDPaymentOrder convert(ReportingWSNameSpace.PaymentOrder wsOrder)
        {
            List<LinkIDPaymentTransaction> transactions = new List<LinkIDPaymentTransaction>();
            if (null != wsOrder.transactions)
            {
                foreach (PaymentTransactionV20 paymentTransaction in wsOrder.transactions)
                {
                    transactions.Add(new LinkIDPaymentTransaction(convert(paymentTransaction.paymentMethodType),
                        paymentTransaction.paymentMethod, convert(paymentTransaction.paymentState),
                        paymentTransaction.creationDate, paymentTransaction.authorizationDate, paymentTransaction.capturedDate,
                        paymentTransaction.docdataReference, paymentTransaction.amount, convert(paymentTransaction.currency)));
                }
            }

            List<LinkIDWalletTransaction> walletTransactions = new List<LinkIDWalletTransaction>();
            if (null != wsOrder.walletTransactions)
            {
                foreach (WalletTransactionV20 walletTransaction in wsOrder.walletTransactions)
                {
                    walletTransactions.Add(new LinkIDWalletTransaction(walletTransaction.walletId, walletTransaction.creationDate,
                        walletTransaction.transactionId, walletTransaction.amount, convert(walletTransaction.currency)));
                }
            }

            return new LinkIDPaymentOrder(wsOrder.date, wsOrder.amount, convert(wsOrder.currency), wsOrder.description,
                convert(wsOrder.paymentState), wsOrder.amountPayed, wsOrder.authorized, wsOrder.captured,
                wsOrder.orderReference, wsOrder.userId, wsOrder.email, wsOrder.givenName, wsOrder.familyName,
                transactions, walletTransactions);
        }

        private LinkIDPaymentMethodType convert(ReportingWSNameSpace.PaymentMethodType wsPaymentMethodType)
        {
            switch (wsPaymentMethodType)
            {
                case PaymentMethodType.UNKNOWN: return LinkIDPaymentMethodType.UNKNOWN;
                case PaymentMethodType.VISA: return LinkIDPaymentMethodType.VISA;
                case PaymentMethodType.MASTERCARD: return LinkIDPaymentMethodType.MASTERCARD;
                case PaymentMethodType.SEPA: return LinkIDPaymentMethodType.SEPA;
                case PaymentMethodType.KLARNA: return LinkIDPaymentMethodType.KLARNA;
            }

            return LinkIDPaymentMethodType.UNKNOWN;
        }

        private LinkIDCurrency convert(ReportingWSNameSpace.Currency wsCurrency)
        {
            switch (wsCurrency)
            {
                case ReportingWSNameSpace.Currency.EUR:
                    return LinkIDCurrency.EUR;
            }

            throw new RuntimeException("Unsupported currency " + wsCurrency + "!");
        }

        private LinkIDPaymentState convert(ReportingWSNameSpace.PaymentStatusType wsPaymentState)
        {
            switch (wsPaymentState)
            {
                case ReportingWSNameSpace.PaymentStatusType.STARTED:
                    return LinkIDPaymentState.STARTED;
                case ReportingWSNameSpace.PaymentStatusType.WAITING_FOR_UPDATE:
                    return LinkIDPaymentState.WAITING_FOR_UPDATE;
                case ReportingWSNameSpace.PaymentStatusType.FAILED:
                    return LinkIDPaymentState.FAILED;
                case ReportingWSNameSpace.PaymentStatusType.DEFERRED:
                    return LinkIDPaymentState.DEFERRED;
                case ReportingWSNameSpace.PaymentStatusType.AUTHORIZED:
                    return LinkIDPaymentState.PAYED;
                case ReportingWSNameSpace.PaymentStatusType.REFUND_STARTED:
                    return LinkIDPaymentState.REFUND_STARTED;
                case ReportingWSNameSpace.PaymentStatusType.REFUNDED:
                    return LinkIDPaymentState.REFUNDED;
            }

            throw new RuntimeException("Unsupported payment state " + wsPaymentState+ "!");

        }

        private LinkIDParkingSession convert(ReportingWSNameSpace.ParkingSession wsParkingSession)
        {
            return new LinkIDParkingSession(wsParkingSession.date, wsParkingSession.barCode, wsParkingSession.parking,
                wsParkingSession.userId, wsParkingSession.turnover, wsParkingSession.validated,
                wsParkingSession.paymentOrderReference, convert(wsParkingSession.paymentState));
        }
    }
}
