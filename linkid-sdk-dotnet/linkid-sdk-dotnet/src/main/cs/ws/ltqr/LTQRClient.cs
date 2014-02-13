/*
 * SafeOnline project.
 * 
 * Copyright 2006-2008 	Lin.k N.V. All rights reserved.
 * Lin.k N.V. proprietary/confidential. Use is subject to license terms.
 */

using System;

namespace safe_online_sdk_dotnet
{
    public interface LTQRClient
    {
        /// <summary>
        /// Push a long term QR session to linkID
        /// </summary>
        /// <param name="paymentContext">Optional payment context</param>
        /// <param name="oneTimeUse">Long term QR session can only be used once</param>
        /// <param name="expiryDate">Optional expiry date of the long term session</param>
        /// <param name="expiryDuration">Optional expiry duration of the long term session. 
        /// Expressed in number of seconds starting from the creation.
        /// Do not mix this attribute with expiryDate. If so, expiryDate will be preferred</param>
        /// <returns>Success object containing the QR in PNG format, the content of the QR code and the order reference of the created long term session
        /// This order reference will be used in the notifications to the service provider.</returns>
        /// <exception cref="PushException">Something went wrong, check the error code what</exception>
        LTQRSession push(PaymentContext paymentContext, bool oneTimeUse, Nullable<DateTime> expiryDate, Nullable<long> expiryDuration);

        /// <summary>
        /// Fetch a set of client sessions
        /// </summary>
        /// <param name="orderReferences">optional list of long term order references, if null all client sessions for all ltqr session will be returned</param>
        /// <param name="clientSessionIds">optional list of client session IDs</param>
        /// <returns>list of client sessions</returns>
        /// <exception cref="PullException">Something went wrong, check the error code what</exception>
        LTQRClientSession[] pull(String[] orderReferences, String[] clientSessionIds);

        /// <summary>
        /// Remove a set of client sessions
        /// </summary>
        /// <param name="sessionIds">﻿list of long term order references to remove, if list of clientSessionIds is empty, all associated ones will be removed.</param>
        /// <param name="clientSessionIds">﻿optional list of client session IDs to remove</param>
        /// <exception cref="RemoveException">Something went wrong, check the error code what</exception>
        void remove(String[] orderReferences, String[] clientSessionIds);

        // log incoming and outgoing soap messages to the console
        void enableLogging();

    }
}
