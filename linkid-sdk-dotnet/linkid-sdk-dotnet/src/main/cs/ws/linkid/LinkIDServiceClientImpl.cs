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
using System.Collections;
using System.Collections.Generic;

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
            binding.MaxReceivedMessageSize = 2147483647;

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

        public LinkIDAuthnResponse callbackPull(String sessionId)
        {
            CallbackPullRequest request = new CallbackPullRequest();

            request.sessionId = sessionId;

            // operate
            CallbackPullResponse response = client.callbackPull(request);

            if (null != response.error)
            {
                throw new LinkIDCallbackPullException(response.error.error, response.error.info);
            }

            if (null != response.success)
            {
                return convert(response.success);
            }

            throw new RuntimeException("No success nor error element in the response ?!");
        }

        public LinkIDThemes getThemes(String applicationName)
        {
            ConfigThemesRequest request = new ConfigThemesRequest();
            request.applicationName = applicationName;

            // operate
            ConfigThemesResponse response = client.configThemes(request);

            if (null != response.error)
            {
                throw new LinkIDThemesException(response.error.errorCode);
            }

            List<LinkIDTheme> linkIDThemes = new List<LinkIDTheme>();
            foreach(ConfigThemes themes in response.success)
            {
                linkIDThemes.Add(new LinkIDTheme(themes.name, themes.defaultTheme, convert(themes.logo),
                    convert(themes.authLogo), convert(themes.background), convert(themes.tabletBackground),
                    convert(themes.alternativeBackground), themes.backgroundColor, themes.textColor));
            }
            return new LinkIDThemes(linkIDThemes);
        }

        public List<LinkIDLocalization> getLocalization(List<String> keys)
        {
            // operate
            ConfigLocalizationResponse response = client.configLocalization(keys.ToArray());

            if (null != response.error)
            {
                throw new LinkIDLocalizationException(response.error.errorCode);
            }

            List<LinkIDLocalization> localizations = new List<LinkIDLocalization>();
            foreach (ConfigLocalization localization in response.success)
            {
                Dictionary<String, String> values = new Dictionary<string, string>();
                foreach(ConfigLocalizationValue localizationvalue in localization.values)
                {
                    values.Add(localizationvalue.languageCode, localizationvalue.localized);
                }
                localizations.Add(new LinkIDLocalization(localization.key, localization.type, values));
            }
            return localizations;
        }

        public LinkIDPaymentStatus getPaymentStatus(String orderReference)
        {
            PaymentStatusRequest request = new PaymentStatusRequest();
            request.orderReference = orderReference;

            // operate
            PaymentStatusResponse response = client.paymentStatus(request);

            if (null != response.error)
            {
                throw new LinkIDPaymentStatusException(response.error.errorCode);
            }

            if (null != response.success)
            {
                List<LinkIDPaymentTransaction> transactions = new List<LinkIDPaymentTransaction>();
                if (null != response.success.paymentDetails.paymentTransactions)
                {
                    foreach (PaymentTransaction wsPaymentTransaction in response.success.paymentDetails.paymentTransactions)
                    {
                        transactions.Add(new LinkIDPaymentTransaction(convert(wsPaymentTransaction.paymentMethodType),
                            wsPaymentTransaction.paymentMethod, convert(wsPaymentTransaction.paymentState),
                            wsPaymentTransaction.creationDate, wsPaymentTransaction.authorizationDate,
                            wsPaymentTransaction.capturedDate, wsPaymentTransaction.docdataReference,
                            wsPaymentTransaction.amount, convert(wsPaymentTransaction.currency).Value, 
                            wsPaymentTransaction.refundAmount));
                    }
                }

                List<LinkIDWalletTransaction> walletTransactions = new List<LinkIDWalletTransaction>();
                if (null != response.success.paymentDetails.walletTransactions)
                {
                    foreach (WalletTransaction wsWalletTransaction in response.success.paymentDetails.walletTransactions)
                    {
                        walletTransactions.Add(new LinkIDWalletTransaction(wsWalletTransaction.walletId, wsWalletTransaction.creationDate,
                            wsWalletTransaction.transactionId, wsWalletTransaction.amount,
                            wsWalletTransaction.currencySpecified ? convert(wsWalletTransaction.currency) : null,
                            wsWalletTransaction.walletCoin, wsWalletTransaction.refundAmount, response.success.description));
                    }
                }

                return new LinkIDPaymentStatus(response.success.orderReference, response.success.userId, 
                    convert(response.success.paymentStatus), response.success.authorized, response.success.captured, 
                    response.success.amountPayed, response.success.amount, response.success.refundAmount,
                    response.success.currencySpecified ? convert(response.success.currency) : null,
                    response.success.walletCoin, response.success.description, response.success.profile,
                    response.success.created, response.success.mandateReference,
                    new LinkIDPaymentDetails(transactions, walletTransactions));

            }

            throw new RuntimeException("No success nor error element in the response ?!");
        }

        public void paymentCapture(String orderReference)
        {
            PaymentCaptureRequest request = new PaymentCaptureRequest();
            request.orderReference = orderReference;

            // operate
            PaymentCaptureResponse response = client.paymentCapture(request);

            if (null != response.error)
            {
                throw new LinkIDPaymentCaptureException(response.error.errorCode);
            }
        }

        public void paymentRefund(String orderReference)
        {
            PaymentRefundRequest request = new PaymentRefundRequest();
            request.orderReference = orderReference;

            // operate
            PaymentRefundResponse response = client.paymentRefund(request);

            if (null != response.error)
            {
                throw new LinkIDPaymentRefundException(response.error.errorCode);
            }
        }

        public String mandatePayment(String mandateReference, LinkIDPaymentContext paymentContext, String language)
        {
            MandatePaymentRequest request = new MandatePaymentRequest();
            request.paymentContext = convert(paymentContext);
            request.mandateReference = mandateReference;
            if (null != language)
            {
                request.language = language;
            }

            MandatePaymentResponse response = client.mandatePayment(request);

            if (null != response.error)
            {
                throw new LinkIDMandatePaymentException(response.error.errorCode);
            }

            if (null != response.success)
            {
                return response.success.orderReference;
            }

            throw new RuntimeException("No success nor error element in the response ?!");
        }

        public LinkIDLTQRSession ltqrPush(LinkIDLTQRContent content, String userAgent, LinkIDLTQRLockType lockType)
        {
            LTQRPushRequest request = new LTQRPushRequest();
            request.content = convert(content);
            request.userAgent = userAgent;
            request.lockType = convert(lockType);

            // operate
            LTQRPushResponse response = client.ltqrPush(request);

            if (null != response.error)
            {
                throw new LinkIDLTQRPushException(response.error.errorCode, response.error.errorMessage);
            }

            if (null != response.success)
            {
                return new LinkIDLTQRSession(response.success.ltqrReference, convert(response.success.qrCodeInfo),
                    response.success.paymentOrderReference);
            }

            throw new RuntimeException("No success nor error element in the response ?!");
        }

        public List<LinkIDLTQRPushResponse> ltqrBulkPush(List<LinkIDLTQRPushContent> contents)
        {
            List<LTQRPushContent> requests = new List<LTQRPushContent>();
            foreach (LinkIDLTQRPushContent content in contents)
            {
                LTQRPushContent ltqrPushContent = new LTQRPushContent();
                ltqrPushContent.content = convert(content.content);
                ltqrPushContent.userAgent = content.userAgent;
                ltqrPushContent.lockType = convert(content.lockType);

                requests.Add(ltqrPushContent);
            }

            // operate
            LTQRBulkPushResponse response = client.ltqrBulkPush(requests.ToArray());

            if (null != response.error)
            {
                throw new LinkIDLTQRBulkPushException(response.error.errorCode, response.error.errorMessage);
            }

            if (null != response.success)
            {
                List<LinkIDLTQRPushResponse> results = new List<LinkIDLTQRPushResponse>();
                foreach (LTQRPushResponse2 ltqrPushResponse in response.success)
                {
                    if (null != ltqrPushResponse.success)
                    {
                        results.Add(new LinkIDLTQRPushResponse(new LinkIDLTQRSession(ltqrPushResponse.success.ltqrReference, 
                            convert(ltqrPushResponse.success.qrCodeInfo), ltqrPushResponse.success.paymentOrderReference)));
                    }
                    else
                    {
                        results.Add(new LinkIDLTQRPushResponse(ltqrPushResponse.error.errorCode, ltqrPushResponse.error.errorMessage));
                    }
                }
                return results;
            }

            throw new RuntimeException("No success nor error element in the response ?!");
        }

        public LinkIDLTQRSession ltqrChange(String ltqrReference, LinkIDLTQRContent content, String userAgent, Boolean unlock, Boolean unblock)
        {
            LTQRChangeRequest request = new LTQRChangeRequest();
            request.ltqrReference = ltqrReference;
            request.content = convert(content);
            request.userAgent = userAgent;
            request.unlock = unlock;
            request.unblock = unblock;

            // operate
            LTQRChangeResponse response = client.ltqrChange(request);

            if (null != response.error)
            {
                throw new LinkIDLTQRChangeException(response.error.errorCode, response.error.errorMessage);
            }

            if (null != response.success)
            {
                return new LinkIDLTQRSession(response.success.ltqrReference, convert(response.success.qrCodeInfo),
                    response.success.paymentOrderReference);
            }

            throw new RuntimeException("No success nor error element in the response ?!");

        }

        public List<LinkIDLTQRClientSession> ltqrPull(List<String> ltqrReferences, List<String> paymentOrderReferences, List<String> clientSessionIds)
        {
            LTQRPullRequest request = new LTQRPullRequest();

            if (null != ltqrReferences && ltqrReferences.Count > 0)
            {
                request.ltqrReferences = ltqrReferences.ToArray();
            }
            if (null != paymentOrderReferences && paymentOrderReferences.Count > 0)
            {
                request.paymentOrderReferences = paymentOrderReferences.ToArray();
            }
            if (null != clientSessionIds && clientSessionIds.Count > 0)
            {
                request.clientSessionIds = clientSessionIds.ToArray();
            }

            // Operate
            LTQRPullResponse response = client.ltqrPull(request);

            if (null != response.error)
            {
                throw new LinkIDLTQRPullException(response.error.errorCode);
            }

            if (null != response.success)
            {
                List<LinkIDLTQRClientSession> clientSessions = new List<LinkIDLTQRClientSession>();
                foreach (LTQRClientSession clientSession in response.success)
                {
                    clientSessions.Add(new LinkIDLTQRClientSession(clientSession.ltqrReference, 
                        convert(clientSession.qrCodeInfo), clientSession.clientSessionId, clientSession.userId,
                        clientSession.created, clientSession.paymentOrderReference, convert(clientSession.paymentStatus)));
                }

                return clientSessions;
            }

            throw new RuntimeException("No success nor error element in the response ?!");

        }

        public void ltqrRemove(List<String> ltqrReferences, List<String> paymentOrderReferences, List<String> clientSessionIds)
        {
            LTQRRemoveRequest request = new LTQRRemoveRequest();

            if (null != ltqrReferences && ltqrReferences.Count > 0)
            {
                request.ltqrReferences = ltqrReferences.ToArray();
            }
            if (null != paymentOrderReferences && paymentOrderReferences.Count > 0)
            {
                request.paymentOrderReferences = paymentOrderReferences.ToArray();
            }
            if (null != clientSessionIds && clientSessionIds.Count > 0)
            {
                request.clientSessionIds = clientSessionIds.ToArray();
            }

            // Operate
            LTQRRemoveResponse response = client.ltqrRemove(request);

            if (null != response.error)
            {
                throw new LinkIDLTQRRemoveException(response.error.errorCode);
            }

            if (null != response.success)
            {
                return;
            }

            throw new RuntimeException("No success nor error element in the response ?!");
        }

        public List<LinkIDLTQRInfo> ltqrInfo(List<String> ltqrReferences, String userAgent)
        {
            LTQRInfoRequest request = new LTQRInfoRequest();
            if (null != ltqrReferences && ltqrReferences.Count > 0)
            {
                request.ltqrReferences = ltqrReferences.ToArray();
            }
            request.userAgent = userAgent;

            // operate
            LTQRInfoResponse response = client.ltqrInfo(request);

            if (null != response.error)
            {
                throw new LinkIDLTQRInfoException(response.error.errorCode);
            }

            if (null != response.success)
            {
                List<LinkIDLTQRInfo> infos = new List<LinkIDLTQRInfo>();
                foreach (LTQRInfo ltqrInfo in response.success)
                {
                    infos.Add(new LinkIDLTQRInfo(ltqrInfo.ltqrReference, ltqrInfo.sessionId, ltqrInfo.created,
                        convert(ltqrInfo.qrCodeInfo), convert(ltqrInfo.content), convert(ltqrInfo.lockType), 
                        ltqrInfo.locked, ltqrInfo.waitForUnblock, ltqrInfo.blocked));
                }
                return infos;
            }

            throw new RuntimeException("No success nor error element in the response ?!");

        }

        public LinkIDPaymentReport getPaymentReport(LinkIDReportDateFilter dateFilter, LinkIDReportPageFilter pageFilter)
        {
            return getPaymentReport(dateFilter, null, null, pageFilter);
        }

        public LinkIDPaymentReport getPaymentReportForOrderReferences(List<String> orderReferences, LinkIDReportPageFilter pageFilter)
        {
            return getPaymentReport(null, orderReferences, null, pageFilter);
        }

        public LinkIDPaymentReport getPaymentReportForMandates(List<String> mandateReferences, LinkIDReportPageFilter pageFilter)
        {
            return getPaymentReport(null, null, mandateReferences, pageFilter);
        }

        public LinkIDParkingReport getParkingReport(LinkIDReportDateFilter dateFilter, LinkIDReportPageFilter pageFilter)
        {
            return getParkingReport(dateFilter, pageFilter, null, null, null, null);
        }

        public LinkIDParkingReport getParkingReport(LinkIDReportDateFilter dateFilter, LinkIDReportPageFilter pageFilter, List<String> parkings)
        {
            return getParkingReport(dateFilter, pageFilter, null, null, null, parkings);
        }

        public LinkIDParkingReport getParkingReportForBarCodes(List<String> barCodes, LinkIDReportPageFilter pageFilter)
        {
            return getParkingReport(null, pageFilter, barCodes, null, null, null);
        }

        public LinkIDParkingReport getParkingReportForTicketNumbers(List<String> ticketNumbers, LinkIDReportPageFilter pageFilter)
        {
            return getParkingReport(null, pageFilter, null, ticketNumbers, null, null);
        }

        public LinkIDParkingReport getParkingReportForDTAKeys(List<String> dtaKeys, LinkIDReportPageFilter pageFilter)
        {
            return getParkingReport(null, pageFilter, null, null, dtaKeys, null);
        }

        public LinkIDWalletReport getWalletReport(String language, String walletOrganizationId,
            LinkIDReportApplicationFilter applicationFilter, LinkIDReportWalletFilter walletFilter, 
            LinkIDReportDateFilter dateFilter, LinkIDReportPageFilter pageFilter)
        {
            WalletReportRequest request = new WalletReportRequest();
            request.language = language;
            request.walletOrganizationId = walletOrganizationId;

            if (null != dateFilter)
            {
                ReportDateFilter wsDateFilter = new ReportDateFilter();
                wsDateFilter.startDate = dateFilter.startDate;
                if (null != dateFilter.endDate)
                {
                    wsDateFilter.endDate = dateFilter.endDate.Value;
                    wsDateFilter.endDateSpecified = true;
                }
                request.dateFilter = wsDateFilter;
            }
            if (null != pageFilter)
            {
                ReportPageFilter wsPageFilter = new ReportPageFilter();
                wsPageFilter.firstResult = pageFilter.firstResult;
                wsPageFilter.maxResults = pageFilter.maxResults;
                request.pageFilter = wsPageFilter;
            }
            if (null != applicationFilter)
            {
                ReportApplicationFilter wsApplicationFilter = new ReportApplicationFilter();
                wsApplicationFilter.applicationName = applicationFilter.applicationName;
                request.applicationFilter = wsApplicationFilter;
            }
            if (null != walletFilter)
            {
                ReportWalletFilter wsWalletFilter = new ReportWalletFilter();
                wsWalletFilter.walletId = walletFilter.walletId;
                request.walletFilter = wsWalletFilter;
            }

            // operate
            WalletReportResponse response = client.walletReport(request);

            if (null != response.error)
            {
                throw new LinkIDReportException(response.error.errorCode);
            }

            List<LinkIDWalletReportTransaction> transactions = new List<LinkIDWalletReportTransaction>();
            if (null != response.transactions)
            {
                foreach (WalletReportTransaction txn in response.transactions)
                {
                    transactions.Add(new LinkIDWalletReportTransaction(txn.walletId, txn.creationDate, txn.transactionId,
                        txn.amount, convert(txn.currency), txn.walletCoin, txn.refundAmount, txn.paymentDescription,
                        txn.userId, txn.applicationName, txn.applicationFriendly, convert(txn.type)));
                }
            }
            return new LinkIDWalletReport(response.total, transactions);

        }

        public List<LinkIDWalletInfoReport> getWalletInfoReport(String language, List<String> walletIds)
        {
            WalletInfoReportRequest request = new WalletInfoReportRequest();
            request.language = language;
            request.walletId = walletIds.ToArray();

            // operate
            WalletInfoReportResponse response = client.walletInfoReport(request);

            if (null != response.error)
            {
                throw new LinkIDWalletInfoReportException(response.error.errorCode);
            }

            List<LinkIDWalletInfoReport> result = new List<LinkIDWalletInfoReport>();
            if (null != response.walletInfo)
            {
                foreach (WalletInfoReport walletInfo in response.walletInfo)
                {
                    result.Add(new LinkIDWalletInfoReport(walletInfo.walletId, walletInfo.created, walletInfo.removed,
                        walletInfo.userId, walletInfo.organizationId, walletInfo.organization, walletInfo.balance));
                }
            }
            return result;
        }

        public String walletEnroll(String userId, String walletOrganizationId, Nullable<double> amount, Nullable<LinkIDCurrency> currency, String walletCoin)
        {
            WalletEnrollRequest request = new WalletEnrollRequest();
            request.userId = userId;
            request.walletOrganizationId = walletOrganizationId;
            if (null != amount)
            {
                request.amount = amount.Value;
                request.amountSpecified = true;
            }
            else
            {
                request.amountSpecified = false;
            }
            if (null != currency)
            {
                request.currency = convert(currency);
                request.currencySpecified = true;
            }
            else
            {
                request.currencySpecified = false;
            }
            request.walletCoin = walletCoin;

            // operate
            WalletEnrollResponse response = client.walletEnroll(request);

            if (null != response.error)
            {
                throw new LinkIDWalletEnrollException(response.error.errorCode);
            }

            if (null != response.success)
            {
                return response.success.walletId;
            }

            throw new RuntimeException("No success nor error element in the response ?!");
        }

        public LinkIDWalletInfo walletGetInfo(String userId, String walletOrganizationId)
        {
            WalletGetInfoRequest request = new WalletGetInfoRequest();
            request.userId = userId;
            request.walletOrganizationId = walletOrganizationId;

            // operate
            WalletGetInfoResponse response = client.walletGetInfo(request);

            if (null != response.error)
            {
                throw new LinkIDWalletGetInfoException(response.error.errorCode);
            }

            if (null != response.success)
            {
                if (null == response.success.walletId)
                {
                    return null;
                }
                else
                {
                    return new LinkIDWalletInfo(response.success.walletId);
                }
            }

            throw new RuntimeException("No success nor error element in the response ?!");
 
        }

        public void walletAddCredit(String userId, String walletId, double amount, Nullable<LinkIDCurrency> currency, String walletCoin)
        {
            WalletAddCreditRequest request = new WalletAddCreditRequest();
            request.userId = userId;
            request.walletId = walletId;
            request.amount = amount;
            request.amountSpecified = true;
            request.currencySpecified = null != currency;
            if (null != currency)
            {
                request.currency = convert(currency);
            }
            request.walletCoin = walletCoin;

            // operate
            WalletAddCreditResponse response = client.walletAddCredit(request);

            if (null != response.error)
            {
                throw new LinkIDWalletAddCreditException(response.error.errorCode);
            }

            if (null != response.success)
            {
                return;
            }

            throw new RuntimeException("No success nor error element in the response ?!");

        }

        public void walletRemoveCredit(String userId, String walletId, double amount, Nullable<LinkIDCurrency> currency, String walletCoin)
        {
            WalletRemoveCreditRequest request = new WalletRemoveCreditRequest();
            request.userId = userId;
            request.walletId = walletId;
            request.amount = amount;
            request.amountSpecified = true;
            request.currencySpecified = null != currency;
            if (null != currency)
            {
                request.currency = convert(currency);
            }
            request.walletCoin = walletCoin;

            // operate
            WalletRemoveCreditResponse response = client.walletRemoveCredit(request);

            if (null != response.error)
            {
                throw new LinkIDWalletRemoveCreditException(response.error.errorCode);
            }

            if (null != response.success)
            {
                return;
            }

            throw new RuntimeException("No success nor error element in the response ?!");

        }

        public void walletRemove(String userId, String walletId)
        {
            WalletRemoveRequest request = new WalletRemoveRequest();
            request.userId = userId;
            request.walletId = walletId;

            // operate
            WalletRemoveResponse response = client.walletRemove(request);

            if (null != response.error)
            {
                throw new LinkIDWalletRemoveException(response.error.errorCode);
            }

            if (null != response.success)
            {
                return;
            }

            throw new RuntimeException("No success nor error element in the response ?!");
        }

        public void walletCommit(String userId, String walletId, String walletTransactionId)
        {
            WalletCommitRequest request = new WalletCommitRequest();
            request.userId = userId;
            request.walletId = walletId;
            request.walletTransactionId = walletTransactionId;

            // operate
            WalletCommitResponse response = client.walletCommit(request);

            if (null != response.error)
            {
                throw new LinkIDWalletCommitException(response.error.errorCode);
            }

            if (null != response.success)
            {
                return;
            }

            throw new RuntimeException("No success nor error element in the response ?!");
        }

        public void walletRelease(String userId, String walletId, String walletTransactionId)
        {
            WalletReleaseRequest request = new WalletReleaseRequest();
            request.userId = userId;
            request.walletId = walletId;
            request.walletTransactionId = walletTransactionId;

            // operate
            WalletReleaseResponse response = client.walletRelease(request);

            if (null != response.error)
            {
                throw new LinkIDWalletReleaseException(response.error.errorCode);
            }

            if (null != response.success)
            {
                return;
            }

            throw new RuntimeException("No success nor error element in the response ?!");
        }


        // Helper methods

        private LinkIDWalletReportType convert(WalletReportType reportType)
        {
            switch (reportType)
            {
                case WalletReportType.usertransaction:
                    return LinkIDWalletReportType.USER_TRANSACTION;
                case WalletReportType.walletadd:
                    return LinkIDWalletReportType.WALLET_ADD;
                case WalletReportType.walletremove:
                    return LinkIDWalletReportType.WALLET_REMOVE;
                case WalletReportType.applicationaddcreditinitial:
                    return LinkIDWalletReportType.APPLICATION_ADD_CREDIT_INITIAL;
                case WalletReportType.applicationaddcredit:
                    return LinkIDWalletReportType.APPLICATION_ADD_CREDIT;
                case WalletReportType.applicationremovecredit:
                    return LinkIDWalletReportType.APPLICATION_REMOVE_CREDIT;
                case WalletReportType.applicationrefund:
                    return LinkIDWalletReportType.APPLICATION_REFUND;
            }

            throw new RuntimeException("WalletReportType " + reportType + " not supported ?!");
        }

        private LinkIDParkingReport getParkingReport(LinkIDReportDateFilter dateFilter, LinkIDReportPageFilter pageFilter,
            List<String> barCodes, List<String> ticketNumbers, List<String> dtaKeys, List<String> parkings)
        {

            ParkingReportRequest request = new ParkingReportRequest();

            if (null != dateFilter)
            {
                ReportDateFilter wsDateFilter = new ReportDateFilter();
                wsDateFilter.startDate = dateFilter.startDate;
                if (null != dateFilter.endDate)
                {
                    wsDateFilter.endDate = dateFilter.endDate.Value;
                    wsDateFilter.endDateSpecified = true;
                }
                request.dateFilter = wsDateFilter;
            }

            if (null != pageFilter)
            {
                ReportPageFilter wsPageFilter = new ReportPageFilter();
                wsPageFilter.firstResult = pageFilter.firstResult;
                wsPageFilter.maxResults = pageFilter.maxResults;
                request.pageFilter = wsPageFilter;
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

            // operate
            ParkingReportResponse response = client.parkingReport(request);

            if (null != response.error)
            {
                throw new LinkIDReportException(response.error.errorCode);
            }

            List<LinkIDParkingSession> sessions = new List<LinkIDParkingSession>();
            if (null != response.sessions)
            {
                foreach (ParkingSession session in response.sessions)
                {
                    sessions.Add(new LinkIDParkingSession(session.date, session.barCode, session.parking, session.userId,
                        session.turnover, session.validated, session.paymentOrderReference, convert(session.paymentState)));
                }
            }
            return new LinkIDParkingReport(response.total, sessions);

        }

        private LinkIDPaymentReport getPaymentReport(LinkIDReportDateFilter dateFilter, List<String> orderReferences, 
            List<String> mandateReferences, LinkIDReportPageFilter pageFilter)
        {
            PaymentReportRequest request = new PaymentReportRequest();

            if (null != dateFilter)
            {
                request.dateFilter = new ReportDateFilter();
                request.dateFilter.startDate = dateFilter.startDate;
                if (null != dateFilter.endDate)
                {
                    request.dateFilter.endDate = dateFilter.endDate.Value;
                    request.dateFilter.endDateSpecified = true;
                }
            }
            if (null != pageFilter)
            {
                request.pageFilter = new ReportPageFilter();
                request.pageFilter.firstResult = pageFilter.firstResult;
                request.pageFilter.maxResults = pageFilter.maxResults;
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
                throw new LinkIDReportException(response.error.errorCode);
            }

            List<LinkIDPaymentOrder> orders = new List<LinkIDPaymentOrder>();
            if (null != response.orders)
            {
                foreach (PaymentOrder order in response.orders)
                {
                    orders.Add(convert(order));
                }
            }

            return new LinkIDPaymentReport(response.total, orders);
        }

        private LinkIDPaymentOrder convert(PaymentOrder wsOrder)
        {
            List<LinkIDPaymentTransaction> transactions = new List<LinkIDPaymentTransaction>();
            if (null != wsOrder.transactions)
            {
                foreach (PaymentTransaction paymentTransaction in wsOrder.transactions)
                {
                    transactions.Add(new LinkIDPaymentTransaction(convert(paymentTransaction.paymentMethodType),
                        paymentTransaction.paymentMethod, convert(paymentTransaction.paymentState),
                        paymentTransaction.creationDate, paymentTransaction.authorizationDate, paymentTransaction.capturedDate,
                        paymentTransaction.docdataReference, paymentTransaction.amount, convert(paymentTransaction.currency).Value, 
                        paymentTransaction.refundAmount));
                }
            }

            List<LinkIDWalletTransaction> walletTransactions = new List<LinkIDWalletTransaction>();
            if (null != wsOrder.walletTransactions)
            {
                foreach (WalletTransaction walletTransaction in wsOrder.walletTransactions)
                {
                    walletTransactions.Add(new LinkIDWalletTransaction(walletTransaction.walletId, walletTransaction.creationDate,
                        walletTransaction.transactionId, walletTransaction.amount, 
                        walletTransaction.currencySpecified ? convert(walletTransaction.currency) : null, 
                        walletTransaction.walletCoin, walletTransaction.refundAmount, 
                        wsOrder.description));
                }
            }

            return new LinkIDPaymentOrder(wsOrder.date, wsOrder.amount, 
                wsOrder.currencySpecified? convert(wsOrder.currency) : null, wsOrder.walletCoin,
                wsOrder.description, convert(wsOrder.paymentState), wsOrder.amountPayed, wsOrder.authorized, 
                wsOrder.captured, wsOrder.orderReference, wsOrder.userId, wsOrder.email, wsOrder.givenName, 
                wsOrder.familyName, transactions, walletTransactions);
        }


        private LinkIDLTQRLockType convert(LTQRLockType lockType)
        {
            switch (lockType)
            {
                case LTQRLockType.NEVER:
                    return LinkIDLTQRLockType.NEVER;
                case LTQRLockType.onscan:
                    return LinkIDLTQRLockType.ON_SCAN;
                case LTQRLockType.onfinish:
                    return LinkIDLTQRLockType.ON_FINISH;
            }

            throw new RuntimeException("LTQR LockType " + lockType + " not supported ?!");
        }

        private LTQRLockType convert(LinkIDLTQRLockType lockType)
        {
            switch (lockType)
            {
                case LinkIDLTQRLockType.NEVER:
                    return LTQRLockType.NEVER;
                case LinkIDLTQRLockType.ON_SCAN:
                    return LTQRLockType.onscan;
                case LinkIDLTQRLockType.ON_FINISH:
                    return LTQRLockType.onfinish;
            }

            throw new RuntimeException("LinkID LTQR LockType " + lockType + " not supported ?!");
        }

        private LinkIDLocalizedImages convert(ConfigLocalizedImage[] images)
        {
            if (null == images)
            {
                return null;
            }

            Dictionary<String, LinkIDLocalizedImage> imageMap = new Dictionary<string, LinkIDLocalizedImage>();
            foreach (ConfigLocalizedImage image in images)
            {
                imageMap.Add(image.language, new LinkIDLocalizedImage(image.url, image.language));
            }
            return new LinkIDLocalizedImages(imageMap);
        }

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

        private Nullable<LinkIDCurrency> convert(Currency currency)
        {
            switch (currency)
            {
                case Currency.EUR: return LinkIDCurrency.EUR;
            }

            return LinkIDCurrency.EUR;
        }

        private Currency convert(Nullable<LinkIDCurrency> currency)
        {
            switch (currency)
            {
                case LinkIDCurrency.EUR: return Currency.EUR;
            }

            return Currency.EUR;
        }

        private LinkIDPaymentMethodType convert(PaymentMethodType paymentMethodType)
        {
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

        private PaymentContext convert(LinkIDPaymentContext linkIDPaymentContext)
        {
            if (null == linkIDPaymentContext)
            {
                return null;
            }

            PaymentContext paymentContext = new PaymentContext();

            paymentContext.amount = linkIDPaymentContext.amount.amount;
            if (null != linkIDPaymentContext.amount.walletCoin)
            {
                paymentContext.walletCoin = linkIDPaymentContext.amount.walletCoin;
            }
            if (null != linkIDPaymentContext.amount.currency)
            {
                paymentContext.currencySpecified = true;
                paymentContext.currency = convert(linkIDPaymentContext.amount.currency);
            }
            paymentContext.description = linkIDPaymentContext.description;
            paymentContext.orderReference = linkIDPaymentContext.orderReference;
            paymentContext.paymentProfile = linkIDPaymentContext.paymentProfile;
            paymentContext.validationTime = linkIDPaymentContext.paymentValidationTime;
            paymentContext.validationTimeSpecified = true;
            paymentContext.allowPartial = linkIDPaymentContext.allowPartial;
            paymentContext.allowPartialSpecified = true;
            paymentContext.onlyWallets = linkIDPaymentContext.onlyWallets;
            paymentContext.onlyWalletsSpecified = true;
            paymentContext.mandate = null != linkIDPaymentContext.mandate;
            if (null != linkIDPaymentContext.mandate)
            {
                paymentContext.mandateDescription = linkIDPaymentContext.mandate.description;
                paymentContext.mandateReference = linkIDPaymentContext.mandate.reference;
            }
            paymentContext.paymentStatusLocation = linkIDPaymentContext.paymentStatusLocation;

            return paymentContext;
        }

        private LinkIDPaymentContext convert(PaymentContext paymentContext)
        {
            if (null == paymentContext)
            {
                return null;
            }

            LinkIDPaymentAmount amount = new LinkIDPaymentAmount(paymentContext.amount, 
                convert(paymentContext.currency), paymentContext.walletCoin);
            LinkIDPaymentContext linkIDPaymentContext = new LinkIDPaymentContext(amount);

            linkIDPaymentContext.description = paymentContext.description;
            linkIDPaymentContext.orderReference = paymentContext.orderReference;
            linkIDPaymentContext.paymentProfile = paymentContext.paymentProfile;
            linkIDPaymentContext.paymentValidationTime = paymentContext.validationTime;
            linkIDPaymentContext.allowPartial = paymentContext.allowPartial;
            linkIDPaymentContext.onlyWallets = paymentContext.onlyWallets;
            if (paymentContext.mandateSpecified && paymentContext.mandate)
            {
                linkIDPaymentContext.mandate = new LinkIDPaymentMandate(
                    paymentContext.mandateDescription, paymentContext.mandateReference); 
            }
            linkIDPaymentContext.paymentStatusLocation = paymentContext.paymentStatusLocation;

            return linkIDPaymentContext;

        }

        private Callback convert(LinkIDCallback linkIDCallback)
        {
            if (null == linkIDCallback)
            {
                return null;
            }

            Callback callback = new Callback();

            callback.location = linkIDCallback.location;
            callback.appSessionId = linkIDCallback.appSessionId;
            callback.inApp = linkIDCallback.inApp;

            return callback;
        }

        private LinkIDCallback convert(Callback callback)
        {
            if (null == callback)
            {
                return null;
            }

            return new LinkIDCallback(callback.location, callback.appSessionId, callback.inApp);
        }

        private LinkIDLTQRContent convert(LTQRContent ltqrContent)
        {
            LinkIDLTQRContent content = new LinkIDLTQRContent();
            
            // custom msgs
            content.authenticationMessage = ltqrContent.authenticationMessage;
            content.finishedMessage = ltqrContent.finishedMessage;

            // payment context
            if (null != ltqrContent.paymentContext)
            {
                content.paymentContext = convert(ltqrContent.paymentContext);
            }

            if (null != ltqrContent.callback)
            {
                content.callback = convert(ltqrContent.callback);
            }

            // identity profile
            content.identityProfile = ltqrContent.identityProfile;

            if (ltqrContent.sessionExpiryOverrideSpecified)
            {
                content.sessionExpiryOVerride = ltqrContent.sessionExpiryOverride;
            }
            content.theme = ltqrContent.theme;
            content.mobileLandingSuccess = ltqrContent.mobileLandingSuccess;
            content.mobileLandingError = ltqrContent.mobileLandingError;
            content.mobileLandingCancel = ltqrContent.mobileLandingCancel;

            // polling configuration
            content.pollingConfiguration = convert(ltqrContent.pollingConfiguration);

            // favorites configuration
            content.favoritesConfiguration = convert(ltqrContent.favoritesConfiguration);

            // configuration
            if (ltqrContent.expiryDateSpecified)
            {
                content.expiryDate = ltqrContent.expiryDate;
            }
            if (ltqrContent.expiryDurationSpecified)
            {
                content.expiryDuration = ltqrContent.expiryDuration;
            }
            content.waitForUnblock = ltqrContent.waitForUnblock;
            content.ltqrStatusLocation = ltqrContent.ltqrStatusLocation;
            
            return content;
        }

        private LTQRContent convert(LinkIDLTQRContent content)
        {
            LTQRContent ltqrContent = new LTQRContent();

            // custom msgs
            ltqrContent.authenticationMessage = content.authenticationMessage;
            ltqrContent.finishedMessage = content.finishedMessage;

            // payment context
            ltqrContent.paymentContext = convert(content.paymentContext);

            // callback
            ltqrContent.callback = convert(content.callback);

            // identity profile
            ltqrContent.identityProfile = content.identityProfile;

            if (content.sessionExpiryOVerride > 0)
            {
                ltqrContent.sessionExpiryOverride = content.sessionExpiryOVerride;
                ltqrContent.sessionExpiryOverrideSpecified = true;
            }
            ltqrContent.theme = content.theme;
            ltqrContent.mobileLandingSuccess = content.mobileLandingSuccess;
            ltqrContent.mobileLandingError = content.mobileLandingError;
            ltqrContent.mobileLandingCancel = content.mobileLandingCancel;

            // polling configuration
            ltqrContent.pollingConfiguration = convert(content.pollingConfiguration);

            // configuration
            if (null != content.expiryDate)
            {
                ltqrContent.expiryDate = content.expiryDate;
                ltqrContent.expiryDateSpecified = true;
            }
            if (null != content.expiryDuration && content.expiryDuration > 0)
            {
                ltqrContent.expiryDuration = content.expiryDuration.Value;
                ltqrContent.expiryDurationSpecified = true;
            }
            ltqrContent.waitForUnblock = content.waitForUnblock;
            if (null != content.ltqrStatusLocation)
            {
                ltqrContent.ltqrStatusLocation = content.ltqrStatusLocation;
            }

            // favorites configuration
            if (null != content.favoritesConfiguration)
            {
                ltqrContent.favoritesConfiguration = convert(content.favoritesConfiguration);
            }

            return ltqrContent;
        }

        private FavoritesConfiguration convert(LinkIDFavoritesConfiguration favoritesConfiguration)
        {
            if (null == favoritesConfiguration)
            {
                return null;
            }

            FavoritesConfiguration wsFavoritesConfiguration = new FavoritesConfiguration();
            wsFavoritesConfiguration.title = favoritesConfiguration.title;
            wsFavoritesConfiguration.info = favoritesConfiguration.info;
            wsFavoritesConfiguration.logoEncoded = favoritesConfiguration.logoEncoded;
            wsFavoritesConfiguration.backgroundColor = favoritesConfiguration.backgroundColor;
            wsFavoritesConfiguration.textColor = favoritesConfiguration.textColor;
            return wsFavoritesConfiguration;
        }

        private LinkIDFavoritesConfiguration convert(FavoritesConfiguration favoritesConfiguration)
        {
            if (null == favoritesConfiguration)
            {
                return null;
            }

            LinkIDFavoritesConfiguration config = new LinkIDFavoritesConfiguration(favoritesConfiguration.title,
                favoritesConfiguration.info, favoritesConfiguration.logoEncoded,
                favoritesConfiguration.backgroundColor, favoritesConfiguration.textColor);
            return config;
        }

        private LinkIDLTQRPollingConfiguration convert(LTQRPollingConfiguration pollingConfiguration)
        {
            if (null == pollingConfiguration)
            {
                return null;
            }

            return new LinkIDLTQRPollingConfiguration(pollingConfiguration.pollAttempts, pollingConfiguration.pollInterval,
                pollingConfiguration.paymentPollAttempts, pollingConfiguration.paymentPollInterval);
        }

        private LTQRPollingConfiguration convert(LinkIDLTQRPollingConfiguration pollingConfiguration)
        {
            if (null == pollingConfiguration)
            {
                return null;
            }

            LTQRPollingConfiguration ltqrPollingConfiguration = new LTQRPollingConfiguration();

            if (pollingConfiguration.pollAttempts > 1)
            {
                ltqrPollingConfiguration.pollAttemptsSpecified = true;
                ltqrPollingConfiguration.pollAttempts = pollingConfiguration.pollAttempts;
            }

            if (pollingConfiguration.pollInterval > 2)
            {
                ltqrPollingConfiguration.pollIntervalSpecified = true;
                ltqrPollingConfiguration.pollInterval = pollingConfiguration.pollInterval;
            }

            if (pollingConfiguration.paymentPollAttempts > 1)
            {
                ltqrPollingConfiguration.paymentPollAttemptsSpecified = true;
                ltqrPollingConfiguration.paymentPollAttempts = pollingConfiguration.paymentPollAttempts;
            }

            if (pollingConfiguration.paymentPollInterval > 2)
            {
                ltqrPollingConfiguration.paymentPollIntervalSpecified = true;
                ltqrPollingConfiguration.paymentPollInterval = pollingConfiguration.paymentPollInterval;
            }
            
            return ltqrPollingConfiguration;
        }

    }
}
