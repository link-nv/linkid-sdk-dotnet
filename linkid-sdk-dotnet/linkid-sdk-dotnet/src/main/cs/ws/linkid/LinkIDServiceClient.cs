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
        /// <param name="language">language</param>
        /// <returns>the reference of the payment order</returns>
        /// <exception cref="LinkIDMandatePaymentException">something went wrong, check the error code</exception>
        String mandatePayment(String mandateReference, LinkIDPaymentContext paymentContext, String language);

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
    }
}
