/*
 * linkID project.
 * 
 * Copyright 2006-2015 linkID N.V. All rights reserved.
 * linkID N.V. proprietary/confidential. Use is subject to license terms.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public interface LinkIDServiceClient
    {
        // log incoming and outgoing soap messages to the console
        void enableLogging();

        /// <summary>
        /// Start a linkID authentication
        /// </summary>
        /// <param name="authenticationContext">the linkID authentication context</param>
        /// <param name="userAgent">Optional user agent string, for adding e.g. callback params to the QR code URL, android chrome URL needs to be http://linkidmauthurl/MAUTH/xxx/eA==</param>
        /// <returns>the sessionId to be used in the redirect</returns>
        /// <exception cref="LinkIDAuthException">something went wrong, check the error code and extra info in the exception</exception>
        LinkIDAuthSession authStart(LinkIDAuthenticationContext authenticationContext, string userAgent);

        /// <summary>
        /// Poll the linkID authentication
        /// </summary>
        /// <param name="sessionId">the sessionId of the authentication</param>
        /// <param name="language">optional language</param>
        /// <returns>poll response containing the state of the authentication</returns>
        /// <exception cref="LinkIDAuthPollException">something went wrong, check the error code</exception>
        LinkIDAuthPollResponse authPoll(String sessionId, String language);

        /// <summary>
        /// Cancel the linkID authentication
        /// </summary>
        /// <param name="sessionId">the sessionId of the authentication</param>
        /// <exception cref="LinkIDAuthCancelException">something went wrong, check the error code</exception>
        void authCancel(String sessionId);

        /// <summary>
        /// Fetch a linkID callback authentication response
        /// </summary>
        /// <param name="sessionId">the session ID of the authentication response</param>
        /// <returns>the authentication response</returns>
        /// <exception cref="LinkIDCallbackPullException">something went wrong, check the error code</exception>
        LinkIDAuthnResponse callbackPull(String sessionId);

        /// <summary>
        /// Fetch the list of linkID applications allowed to use specified wallet organization
        /// </summary>
        /// <param name="walletOrganizationId">the wallet organization ID</param>
        /// <param name="language">locale to return the application friendly name in</param>
        /// <returns>the list of applications</returns>
        /// <exception cref="LinkIDConfigWalletApplicationsException">something went wrong, check the error code</exception>
        List<LinkIDApplication> configWalletApplications(String walletOrganizationId, String language);

        /// <summary>
        /// Fetch the application's themes
        /// </summary>
        /// <param name="applicationName"></param>
        /// <returns>the themes</returns>
        /// <exception cref="LinkIDThemesException">something went wrong, check the error code</exception>
        LinkIDThemes getThemes(String applicationName);

        /// <summary>
        /// Fetch the specified keys's localization in linkID
        /// </summary>
        /// <param name="keys">the keys to fetch localization for</param>
        /// <returns>the localizations</returns>
        /// <exception cref="LinkIDLocalizationException">something went wrong, check the error code</exception>
        List<LinkIDLocalization> getLocalization(List<String> keys);

        /// <summary>
        /// Fetch the payment status of specified order
        /// </summary>
        /// <param name="orderReference">the order reference of the payment order</param>
        /// <returns>the payment status details</returns>
        /// <exception cref="LinkIDPaymentStatusException">something went wrong, check the error code</exception>
        LinkIDPaymentStatus getPaymentStatus(String orderReference);

        /// <summary>
        /// Capture a payment
        /// </summary>
        /// <param name="orderReference">the order reference of the payment order</param>
        /// <exception cref="LinkIDPaymentCaptureException">something went wrong, check the error code</exception>
        void paymentCapture(String orderReference);
       
        /// <summary>
        /// Refund a payment
        /// </summary>
        /// <param name="orderReference">the order reference of the payment order</param>
        /// <exception cref="LinkIDPaymentRefundException">something went wrong, check the error code</exception>
        void paymentRefund(String orderReference);

        /// <summary>
        /// Make a payment for specified mandate
        /// </summary>
        /// <param name="mandateReference">reference of the mandate</param>
        /// <param name="paymentContext">payment info</param>
        /// <param name="notificationLocation">optional notification location override</param>
        /// <param name="language">language</param>
        /// <returns>the reference of the payment order</returns>
        /// <exception cref="LinkIDMandatePaymentException">something went wrong, check the error code</exception>
        String mandatePayment(String mandateReference, LinkIDPaymentContext paymentContext, 
            String notificationLocation, String language);

        /// <summary>
        /// Push a long term QR session to linkID
        /// </summary>
        /// <param name="content">configuration of the LTQR</param>
        /// <param name="userAgent">optional user agent for the QR code URL formatting</param>
        /// <param name="lockType">lock type of the LTQR</param>
        /// <returns>LTQR session info</returns>
        /// <exception cref="LinkIDLTQRPushException">something went wrong, check the error code</exception>
        LinkIDLTQRSession ltqrPush(LinkIDLTQRContent content, String userAgent, LinkIDLTQRLockType lockType);

        /// <summary>
        /// Bulk push long term QR sessions to linkID
        /// </summary>
        /// <param name="contents">the LTQR request contents</param>
        /// <returns>list of responses for the LTQR requests</returns>
        /// <exception cref="LinkIDLTQRBulkPushException">something went wrong, check the error code</exception>
        List<LinkIDLTQRPushResponse> ltqrBulkPush(List<LinkIDLTQRPushContent> contents);

        /// <summary>
        /// Change an existing long term QR code
        /// </summary>
        /// <param name="ltqrReference">LTQR reference</param>
        /// <param name="content">Configuration of this LTQR</param>
        /// <param name="userAgent">Optional user agent case you want to get the QR code URL formatted</param>
        /// <param name="unlock">unlocks the lTQR code that has been locked depending on the lockType</param>
        /// <param name="unblock">unblocks the lTQR code if waitForUnblock was set true. This will allow users that were waiting to continue the QR session</param>
        /// <returns>LTQR session info</returns>
        /// <exception cref="LinkIDLTQRChangeException">something went wrong, check the error code</exception>
        LinkIDLTQRSession ltqrChange(String ltqrReference, LinkIDLTQRContent content, String userAgent, Boolean unlock, Boolean unblock);

        /// <summary>
        /// Fetch a set of client sessions
        /// </summary>
        /// <param name="ltqrReferences">LTQR references</param>
        /// <param name="paymentOrderReferences">Payment order references</param>
        /// <param name="clientSessionIds">LTQR Client session IDs</param>
        /// <returns>list of all client sessions</returns>
        /// <exception cref="LinkIDLTQRPullException">something went wrong, check the error code</exception>
        List<LinkIDLTQRClientSession> ltqrPull(List<String> ltqrReferences, List<String> paymentOrderReferences, List<String> clientSessionIds);

        /// <summary>
        /// Remove a set of LTQR client sessions
        /// </summary>
        /// <param name="ltqrReferences">LTQR references</param>
        /// <param name="paymentOrderReferences">Payment order references</param>
        /// <param name="clientSessionIds">LTQR Client session IDs</param>
        /// <exception cref="LinkIDLTQRRemoveException">something went wrong, check the error code</exception>
        void ltqrRemove(List<String> ltqrReferences, List<String> paymentOrderReferences, List<String> clientSessionIds);

        /// <summary>
        /// Fetch info for the specified LTQR references
        /// </summary>
        /// <param name="ltqrReferences">the lTQR references</param>
        /// <param name="userAgent">optional user agent for formatting the QR code URL</param>
        /// <returns>the LTQR info objects</returns>
        /// <exception cref="LinkIDLTQRInfoException">something went wrong, check the error code</exception>
        List<LinkIDLTQRInfo> ltqrInfo(List<String> ltqrReferences, String userAgent);

        /// <summary>
        /// Fetch payment report
        /// </summary>
        /// <param name="dateFilter">optional date filter</param>
        /// <param name="pageFilter">optional page filter in case of large result sets</param>
        /// <returns>the payment report</returns>
        /// <exception cref="LinkIDReportException">something went wrong, check the error code</exception>
        LinkIDPaymentReport getPaymentReport(LinkIDReportDateFilter dateFilter, LinkIDReportPageFilter pageFilter);

        /// <summary>
        /// Fetch payment report
        /// </summary>
        /// <param name="orderReferences">order references</param>
        /// <param name="pageFilter">optional page filter in case of large result sets</param>
        /// <returns>the payment report</returns>
        /// <exception cref="LinkIDReportException">something went wrong, check the error code</exception>
        LinkIDPaymentReport getPaymentReportForOrderReferences(List<String> orderReferences, LinkIDReportPageFilter pageFilter);

        /// <summary>
        /// Fetch payment report
        /// </summary>
        /// <param name="mandateReferences">mandate references</param>
        /// <param name="pageFilter">optional page filter in case of large result sets</param>
        /// <returns>the payment report</returns>
        /// <exception cref="LinkIDReportException">something went wrong, check the error code</exception>
        LinkIDPaymentReport getPaymentReportForMandates(List<String> mandateReferences, LinkIDReportPageFilter pageFilter);

        /// <summary>
        /// Fetch parking report
        /// </summary>
        /// <param name="dateFilter">optional date filter</param>
        /// <param name="pageFilter">optional page filter in case of large result sets</param>
        /// <returns>the parking report</returns>
        /// <exception cref="LinkIDReportException">something went wrong, check the error code</exception>
        LinkIDParkingReport getParkingReport(LinkIDReportDateFilter dateFilter, LinkIDReportPageFilter pageFilter);

        /// <summary>
        /// Fetch parking report
        /// </summary>
        /// <param name="dateFilter">optional date filter</param>
        /// <param name="pageFilter">optional page filter in case of large result sets</param>
        /// <param name="parkings">optional list of parkings</param>
        /// <returns>the parking report</returns>
        /// <exception cref="LinkIDReportException">something went wrong, check the error code</exception>
        LinkIDParkingReport getParkingReport(LinkIDReportDateFilter dateFilter, LinkIDReportPageFilter pageFilter, List<String> parkings);

        /// <summary>
        /// Fetch parking report
        /// </summary>
        /// <param name="barCodes">optional list of barCodes</param>
        /// <param name="pageFilter">optional page filter in case of large result sets</param>
        /// <returns>the parking report</returns>
        /// <exception cref="LinkIDReportException">something went wrong, check the error code</exception>
        LinkIDParkingReport getParkingReportForBarCodes(List<String> barCodes, LinkIDReportPageFilter pageFilter);

        /// <summary>
        /// Fetch parking report
        /// </summary>
        /// <param name="ticketNumbers">optional list of ticketNumbers</param>
        /// <param name="pageFilter">optional page filter in case of large result sets</param>
        /// <returns>the parking report</returns>
        /// <exception cref="LinkIDReportException">something went wrong, check the error code</exception>
        LinkIDParkingReport getParkingReportForTicketNumbers(List<String> ticketNumbers, LinkIDReportPageFilter pageFilter);

        /// <summary>
        /// Fetch parking report
        /// </summary>
        /// <param name="dtaKeys">optional list of dtaKeys</param>
        /// <param name="pageFilter">optional page filter in case of large result sets</param>
        /// <returns>the parking report</returns>
        /// <exception cref="LinkIDReportException">something went wrong, check the error code</exception>
        LinkIDParkingReport getParkingReportForDTAKeys(List<String> dtaKeys, LinkIDReportPageFilter pageFilter);

        /// <summary>
        /// Fetch wallet report
        /// </summary>
        /// <param name="language">optional language, default is en</param>
        /// <param name="walletOrganizationId">the wallet organization ID</param>
        /// <param name="applicationFilter">optional application filter</param>
        /// <param name="walletFilter">optional wallet filter</param>
        /// <param name="dateFilter">optional date filter</param>
        /// <param name="pageFilter">optional page filter</param>
        /// <returns>the wallet report</returns>
        /// <exception cref="LinkIDReportException">something went wrong, check the error code</exception>
        LinkIDWalletReport getWalletReport(String language, String walletOrganizationId,
            LinkIDReportApplicationFilter applicationFilter, LinkIDReportWalletFilter walletFilter, 
            LinkIDReportDateFilter dateFilter, LinkIDReportPageFilter pageFilter);

        /// <summary>
        /// Fetch wallet info report for specified wallet IDs
        /// </summary>
        /// <param name="language">optional language, default is en</param>
        /// <param name="walletIds">list of wallet IDs to fetch info for</param>
        /// <returns>list of wallet report info objects for the specified walletIds. If a walletId was not found it will be skipped</returns>
        /// <exception cref="LinkIDWalletInfoReportException">something went wrong, check the error code</exception>
        List<LinkIDWalletInfoReport> getWalletInfoReport(String language, List<String> walletIds);

        /// <summary>
        /// Enroll users for a wallet. Optionally specify initial credit to add to wallet if applicable
        /// </summary>
        /// <param name="userId">the userId</param>
        /// <param name="walletOrganizationId">the wallet organization ID</param>
        /// <returns>the wallet ID</returns>
        /// <exception cref="LinkIDWalletEnrollException">something went wrong, check the error code</exception>
        String walletEnroll(String userId, String walletOrganizationId, Nullable<double> amount, Nullable<LinkIDCurrency> currency, String walletCoin, LinkIDWalletReportInfo reportInfo);

        /// <summary>
        /// Get info about a wallet for specified user and wallet organization
        /// </summary>
        /// <param name="userId">the userId</param>
        /// <param name="walletOrganizationId">the walletOrganizationId</param>
        /// <returns>wallet info or null if no such wallet for that user</returns>
        /// <exception cref="LinkIDWalletGetInfoException">something went wrong, check the error code</exception>
        LinkIDWalletInfo walletGetInfo(String userId, String walletOrganizationId);

        /// <summary>
        /// Add credit for a user of a wallet
        /// </summary>
        /// <param name="userId">the userId</param>
        /// <param name="walletId">the walletId</param>
        /// <param name="amount">the amount to add</param>
        /// <param name="currency">optional currency</param>
        /// <param name="walletCoin">optional walletCoint</param>
        /// <exception cref="LinkIDWalletAddCreditException">something went wrong, check the error code</exception>
        void walletAddCredit(String userId, String walletId, double amount, Nullable<LinkIDCurrency> currency, String walletCoin, LinkIDWalletReportInfo reportInfo);

        /// <summary>
        /// Remove credit for a user of a wallet
        /// </summary>
        /// <param name="userId">the userId</param>
        /// <param name="walletId">the walletId</param>
        /// <param name="amount">the amount to remove</param>
        /// <param name="currency">optional currency</param>
        /// <param name="walletCoin">optional walletCoint</param>
        /// <exception cref="LinkIDWalletRemoveCreditException">something went wrong, check the error code</exception>
        void walletRemoveCredit(String userId, String walletId, double amount, Nullable<LinkIDCurrency> currency, String walletCoin, LinkIDWalletReportInfo reportInfo);

        /// <summary>
        /// Remove the specified wallet from that user
        /// </summary>
        /// <param name="userId">the userId</param>
        /// <param name="walletId">the walletId</param>
        /// <exception cref="LinkIDWalletRemoveException">something went wrong, check the error code</exception>
        void walletRemove(String userId, String walletId);

        /// <summary>
        /// Commit a wallet transaction. The amount payed for the specified wallet transaction ID will be free'd.
        /// If not committed, linKID will after a period of time release it
        /// </summary>
        /// <param name="userId">the userId</param>
        /// <param name="walletId">the walletId</param>
        /// <param name="walletTransactionId">the wallet transaction ID</param>
        /// <exception cref="LinkIDWalletCommitException">something went wrong, check the error code</exception>
        void walletCommit(String userId, String walletId, String walletTransactionId);

        /// <summary>
        /// Release a wallet transaction immediately instead of waiting for the wallet's expiration
        /// </summary>
        /// <param name="userId">the userId</param>
        /// <param name="walletId">the walletId</param>
        /// <param name="walletTransactionId">the wallet transaction ID</param>
        /// <exception cref="LinkIDWalletReleaseException">something went wrong, check the error code</exception>
        void walletRelease(String userId, String walletId, String walletTransactionId);
    }
}
