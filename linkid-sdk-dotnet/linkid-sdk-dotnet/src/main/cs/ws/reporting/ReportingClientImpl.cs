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
			string address = "https://" + location + "/linkid-ws-username/reporting";
			EndpointAddress remoteAddress = new EndpointAddress(address);

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.MaxReceivedMessageSize = 2147483647;

            this.client = new ReportingServicePortClient(binding, remoteAddress);
            this.client.Endpoint.Behaviors.Add(new PasswordDigestBehavior(username, password));
		}

        public ReportingClientImpl(string location, X509Certificate2 appCertificate, X509Certificate2 linkidCertificate)
        {
            string address = "https://" + location + "/linkid-ws/reporting";
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

        public List<PaymentTransaction> getPaymentReport(DateTime startDate, DateTime endDate)
        {
            return getPaymentReport(startDate, endDate, null, null);
        }

        public List<PaymentTransaction> getPaymentReportForOrderReferences(List<String> orderReferences)
        {
            return getPaymentReport(null, null, orderReferences, null);
        }

        public List<PaymentTransaction> ﻿getPaymentReportForMandates(List<String> mandateReferences)
        {
            return getPaymentReport(null, null, null, mandateReferences);
        }

        public List<ParkingSession> getParkingReport(DateTime startDate, DateTime endDate)
        {
            return getParkingReport(startDate, endDate, null, null, null, null);
        }

        public List<ParkingSession> getParkingReport(DateTime startDate, DateTime endDate, List<String> parkings)
        {
            return getParkingReport(startDate, endDate, null, null, null, parkings);
        }

        public List<ParkingSession> getParkingReportForBarCodes(List<String> barCodes)
        {
            return getParkingReport(null, null, barCodes, null, null, null);
        }

        public List<ParkingSession> getParkingReportForTicketNumbers(List<String> ticketNumbers)
        {
            return getParkingReport(null, null, null, ticketNumbers, null, null);
        }

        public List<ParkingSession> getParkingReportForDTAKeys(List<String> dtaKeys)
        {
            return getParkingReport(null, null, null, null, dtaKeys, null);
        }

        public List<ParkingSession> getParkingReportForParkings(List<String> parkings)
        {
            return getParkingReport(null, null, null, null, null, parkings);
        }


        // Helper methods

        private List<ParkingSession> getParkingReport(Nullable<DateTime> startDate, Nullable<DateTime> endDate,
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

            List<ParkingSession> parkingSessions = new List<ParkingSession>();
            foreach (ReportingWSNameSpace.ParkingSession wsParkingSession in wsParkingSessions)
            {
                parkingSessions.Add(convert(wsParkingSession));
            }

            return parkingSessions;
        }

        private List<PaymentTransaction> getPaymentReport(Nullable<DateTime> startDate, Nullable<DateTime> endDate,
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

            ReportingWSNameSpace.PaymentTransaction[] wsTransactions = this.client.paymentReport(request);

            List<PaymentTransaction> transactions = new List<PaymentTransaction>();
            foreach(ReportingWSNameSpace.PaymentTransaction wsTransaction in wsTransactions)
            {
                transactions.Add(convert(wsTransaction));
            }

            return transactions;
        }

        private PaymentTransaction convert(ReportingWSNameSpace.PaymentTransaction wsTransaction)
        {
            return new PaymentTransaction(wsTransaction.date, wsTransaction.amount, convert(wsTransaction.currency),
                wsTransaction.paymentMethod, wsTransaction.description, convert(wsTransaction.paymentState),
                wsTransaction.paid, wsTransaction.orderReference, wsTransaction.docdataReference,
                wsTransaction.userId, wsTransaction.email, wsTransaction.givenName, wsTransaction.familyName);
        }

        private Currency convert(ReportingWSNameSpace.Currency wsCurrency)
        {
            switch (wsCurrency)
            {
                case ReportingWSNameSpace.Currency.EUR:
                    return Currency.EUR;
            }

            throw new RuntimeException("Unsupported currency " + wsCurrency + "!");
        }

        private PaymentState convert(ReportingWSNameSpace.PaymentStatusType wsPaymentState)
        {
            switch (wsPaymentState)
            {
                case ReportingWSNameSpace.PaymentStatusType.STARTED:
                    return PaymentState.STARTED;
                case ReportingWSNameSpace.PaymentStatusType.WAITING_FOR_UPDATE:
                    return PaymentState.WAITING_FOR_UPDATE;
                case ReportingWSNameSpace.PaymentStatusType.FAILED:
                    return PaymentState.FAILED;
                case ReportingWSNameSpace.PaymentStatusType.DEFERRED:
                    return PaymentState.DEFERRED;
                case ReportingWSNameSpace.PaymentStatusType.AUTHORIZED:
                    return PaymentState.PAYED;
                case ReportingWSNameSpace.PaymentStatusType.REFUND_STARTED:
                    return PaymentState.REFUND_STARTED;
                case ReportingWSNameSpace.PaymentStatusType.REFUNDED:
                    return PaymentState.REFUNDED;
            }

            throw new RuntimeException("Unsupported payment state " + wsPaymentState+ "!");

        }

        private ParkingSession convert(ReportingWSNameSpace.ParkingSession wsParkingSession)
        {
            return new ParkingSession(wsParkingSession.date, wsParkingSession.barCode, wsParkingSession.parking,
                wsParkingSession.userId, wsParkingSession.turnover, wsParkingSession.validated,
                wsParkingSession.paymentOrderReference, convert(wsParkingSession.paymentState));
        }
    }
}
