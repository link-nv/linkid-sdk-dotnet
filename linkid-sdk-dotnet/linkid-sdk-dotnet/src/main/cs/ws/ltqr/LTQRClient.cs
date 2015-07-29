/*
 * SafeOnline project.
 * 
 * Copyright 2006-2008 	Lin.k N.V. All rights reserved.
 * Lin.k N.V. proprietary/confidential. Use is subject to license terms.
 */

using System;
using System.Collections.Generic;

namespace safe_online_sdk_dotnet
{
    public interface LTQRClient
    {
        /// <summary>
        /// Push a long term QR session to linkID
        /// </summary>
        /// <param name="authenticationMessage">Optional authentication message to be shown in the pin view in the mobile app. If there is a payment, this will be ignored.</param>
        /// <param name="finishedMessage">Optional finished message on the final view in the mobile app.</param>
        /// <param name="paymentContext">Optional payment context</param>
        /// <param name="oneTimeUse">Long term QR session can only be used once</param>
        /// <param name="expiryDate">Optional expiry date of the long term session</param>
        /// <param name="expiryDuration">Optional expiry duration of the long term session. 
        /// Expressed in number of seconds starting from the creation.
        /// Do not mix this attribute with expiryDate. If so, expiryDate will be preferred</param>
        /// <param name="callback">Optional callback config</param>
        /// <param name="identityProfiles">Optional identity profiles</param>
        /// <param name="sessionExpiryOverride">optional session expiry (seconds)</param>
        /// <param name="theme">optional theme, if not specified default application theme will be chosen</param>
        /// <param name="mobileLandingSuccess">optional landing page for an authn/payment started on iOS browser</param>
        /// <param name="mobileLandingError">optional landing page for an authn/payment started on iOS browser</param>
        /// <param name="mobileLandingCancel">optional landing page for an authn/payment started on iOS browser</param>
        /// <param name="pollingConfiguration">Optional polling configuration</param>
        /// <param name="waitForUnlock">Marks the LTQR to wait for an explicit unlock call. This only makes sense for single-use LTQR codes. Unlock the LTQR with the change operation with unlock=true</param>
        /// <returns>Success object containing the QR in PNG format, the content of the QR code and the LTQR reference of the created long term session
        /// This LTQR reference will be used in the notifications to the service provider.</returns>
        /// <exception cref="PushException">Something went wrong, check the error code what</exception>
        LTQRSession push(String authenticationMessage, String finishedMessage, LinkIDPaymentContext paymentContext, 
            bool oneTimeUse, Nullable<DateTime> expiryDate, Nullable<long> expiryDuration,
            LinkIDCallback callback, List<String> identityProfiles, Nullable<long> sessionExpiryOverride, String theme,
            String mobileLandingSuccess, String mobileLandingError, String mobileLandingCancel, 
            LinkIDLTQRPollingConfiguration pollingConfiguration, bool waitForUnlock);

        /// <summary>
        /// Change ﻿Change an existing long term QR code
        /// </summary>
        /// <param name="orderReference">Required LTQR reference</param>
        /// <param name="authenticationMessage">Optional authentication message to be shown in the pin view in the mobile app. If there is a payment, this will be ignored.</param>
        /// <param name="finishedMessage">Optional finished message on the final view in the mobile app.</param>
        /// <param name="paymentContext">Optional payment context</param>
        /// <param name="oneTimeUse">Long term QR session can only be used once</param>
        /// <param name="expiryDate">Optional expiry date of the long term session</param>
        /// <param name="expiryDuration">Optional expiry duration of the long term session. 
        /// <param name="sessionExpiryOverride">optional session expiry (seconds)</param>
        /// <param name="theme">optional theme, if not specified default application theme will be chosen</param>
        /// <param name="mobileLandingSuccess">optional landing page for an authn/payment started on iOS browser</param>
        /// <param name="mobileLandingError">optional landing page for an authn/payment started on iOS browser</param>
        /// <param name="mobileLandingCancel">optional landing page for an authn/payment started on iOS browser</param>
        /// <param name="resetUsed">Optional flag for single use LTQR codes to let them be used again one time. If multi use this flag does nothing.</param>
        /// Expressed in number of seconds starting from the creation.
        /// Do not mix this attribute with expiryDate. If so, expiryDate will be preferred</param>
        /// <param name="pollingConfiguration">Optional polling configuration</param>
        /// <param name="waitForUnlock">Marks the LTQR to wait for an explicit unlock call. This only makes sense for single-use LTQR codes. Unlock the LTQR with the change operation with unlock=true</param>
        /// <param name="unlock">Unlocks the LTQR. When the first linkID user has finished for this LTQR, it will go back to locked if waitForUnlock=true</param>
        /// <exception cref="ChangeException">Something went wrong, check the error code what</exception>
        LTQRSession change(String ltqrReference, String authenticationMessage, String finishedMessage,
            LinkIDPaymentContext paymentContext, Nullable<DateTime> expiryDate, Nullable<long> expiryDuration,
            LinkIDCallback callback, List<String> identityProfiles, Nullable<long> sessionExpiryOverride, String theme,
            String mobileLandingSuccess, String mobileLandingError, String mobileLandingCancel, bool resetUsed,
            LinkIDLTQRPollingConfiguration pollingConfiguration, bool waitForUnlock, bool unlock);

        /// <summary>
        /// Fetch a set of client sessions
        /// </summary>
        /// <param name="ltqrReferences">optional list of long term QR references, if null all client sessions for all ltqr session will be returned</param>
        /// <param name="paymentOrderReferences">optional list of payment order references</param>
        /// <param name="clientSessionIds">optional list of client session IDs</param>
        /// <returns>list of client sessions</returns>
        /// <exception cref="PullException">Something went wrong, check the error code what</exception>
        LTQRClientSession[] pull(String[] ltqrReferences, String[] paymentOrderReferences, String[] clientSessionIds);

        /// <summary>
        /// Remove a set of client sessions
        /// </summary>
        /// <param name="ltqrReferences">﻿list of long term QR references to remove, if list of clientSessionIds is empty, all associated ones will be removed.</param>
        /// <param name="paymentOrderReferences">optional list of payment order references</param>
        /// <param name="clientSessionIds">﻿optional list of client session IDs to remove</param>
        /// <exception cref="RemoveException">Something went wrong, check the error code what</exception>
        void remove(String[] ltqrReferences, String[] paymentOrderReferences, String[] clientSessionIds);

        /// <summary>
        /// Fetch info for the specified LTQR references
        /// </summary>
        /// <param name="ltqrReferences">the list of LTQR references to fetch info for</param>
        /// <returns>the LTQR info objects</returns>
        /// <exception cref="LinkIDLTQRInfoException">Something went wrong, check the error code what</exception>
        List<LinkIDLTQRInfo> info(String[] ltqrReferences);

        // log incoming and outgoing soap messages to the console
        void enableLogging();

    }
}
