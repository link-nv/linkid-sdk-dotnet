/*
 * SafeOnline project.
 * 
 * Copyright 2006-2008 	Lin.k N.V. All rights reserved.
 * Lin.k N.V. proprietary/confidential. Use is subject to license terms.
 */

using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Net.Security;
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

        public List<LinkIDWalletReportTransaction> getWalletReport(String walletOrganizationId,
            LinkIDReportDateFilter dateFilter)
        {
            return getWalletReport(walletOrganizationId, dateFilter, null, null);
        }

        public List<LinkIDWalletReportTransaction> getWalletReport(String walletOrganizationId, 
            LinkIDReportApplicationFilter applicationFilter)
        {
            return getWalletReport(walletOrganizationId, null, applicationFilter, null);
        }

        public List<LinkIDWalletReportTransaction> getWalletReport(String walletOrganizationId, 
            LinkIDReportWalletFilter walletFilter)
        {
            return getWalletReport(walletOrganizationId, null, null, walletFilter);
        }



        // Helper methods

        private List<LinkIDWalletReportTransaction> getWalletReport(String walletOrganizationId,
            LinkIDReportDateFilter dateFilter, LinkIDReportApplicationFilter applicationFilter,
            LinkIDReportWalletFilter walletFilter)
        {
            WalletReportRequest request = new WalletReportRequest();

            request.walletOrganizationId = walletOrganizationId;

            if (null != dateFilter)
            {
                request.dateFilter = new DateFilter();
                request.dateFilter.startDate = dateFilter.startDate;
                if (null != dateFilter.endDate)
                {
                    request.dateFilter.endDateSpecified = true;
                    request.dateFilter.endDate = dateFilter.endDate.Value;
                }
            }
            if (null != applicationFilter)
            {
                request.applicationFilter = new ApplicationFilter();
                request.applicationFilter.applicationName = applicationFilter.applicationName;
            }
            if (null != walletFilter)
            {
                request.walletFilter = new WalletFilter();
                request.walletFilter.walletId = walletFilter.walletId;
                request.walletFilter.userId = walletFilter.userId;
            }

            WalletReportResponse response = this.client.walletReport(request);

            if (null != response.error)
            {
                throw new ReportingException(response.error.errorCode);
            }

            ReportingWSNameSpace.WalletReportTransaction[] wsTransactions = response.transactions;

            List<LinkIDWalletReportTransaction> transactions = new List<LinkIDWalletReportTransaction>();
            foreach (ReportingWSNameSpace.WalletReportTransaction wsTransaction in wsTransactions)
            {
                transactions.Add(convert(wsTransaction));
            }

            return transactions;

        }


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

            ParkingReportResponse response = this.client.parkingReport(request);

            if (null != response.error)
            {
                throw new ReportingException(response.error.errorCode);
            }

            ReportingWSNameSpace.ParkingSession[] wsParkingSessions = response.sessions;

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

            PaymentReportResponse response = this.client.paymentReport(request);

            if (null != response.error)
            {
                throw new ReportingException(response.error.errorCode);
            }

            ReportingWSNameSpace.PaymentOrder[] wsOrders= response.orders;

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

        private LinkIDWalletReportTransaction convert(ReportingWSNameSpace.WalletReportTransaction wsTransaction)
        {
            return new LinkIDWalletReportTransaction(wsTransaction.walletId, wsTransaction.creationDate,
                wsTransaction.transactionId, wsTransaction.amount, convert(wsTransaction.currency),
                wsTransaction.userId, wsTransaction.applicationName);
        }
    }
}
