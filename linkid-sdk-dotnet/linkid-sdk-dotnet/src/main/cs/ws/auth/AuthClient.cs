/*
 * SafeOnline project.
 * 
 * Copyright 2006-2008 	Lin.k N.V. All rights reserved.
 * Lin.k N.V. proprietary/confidential. Use is subject to license terms.
 */

using System;
using AttributeWSNamespace;

namespace safe_online_sdk_dotnet
{
    public interface AuthClient
    {
        /// <summary>
        /// Start a linkID authentication/payment
        /// </summary>
        /// <param name="authnRequest">SAML v2.0 authentication request</param>
        /// <param name="language">﻿ISO 639 alpha-2 or alpha-3 language code. Optional, default is en</param>
        /// <param name="userAgent">Optional user agent string, for adding e.g. callback params to the QR code URL, android chrome URL needs to be http://linkidmauthurl/MAUTH/xxx/eA==</param>
        /// <returns>the sessionId to be used in the redirect</returns>
        /// <exception cref="AuthnException">something went wrong, check the error code and extra info in the exception</exception>
        AuthnSession start(Saml2AuthUtil saml2AuthUtil, AuthnRequestType authnRequest, string language, string userAgent);

        /// <summary>
        /// Poll a linkID authentication/payment. Check the returned poll response to see where the linkID user is at
        /// </summary>
        /// <param name="sessionId">linkID session ID to poll</param>
        /// <returns>PollResponse containinbg the authenticationb state, payment state and if done the authentication context of the user</returns>
        PollResponse poll(Saml2AuthUtil saml2AuthUtil, string sessionId, string language);

        /// <summary>
        /// Cancel a linkID authentication/payment session.
        /// </summary>
        /// <param name="sessionId">linkID session ID to cancel</param>
        /// <exception cref="CancelException">Something went wrong, check the error code in the exception</exception>
        void cancel(String sessionId);

        // log incoming and outgoing soap messages to the console
        void enableLogging();

    }
}
