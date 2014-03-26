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
    public interface HawsClient
    {
        /// <summary>
        /// Push the SAML v2.0 authentication request to linkID. Returns the sessionId to be used in the redirect.
        /// </summary>
        /// <param name="authnRequest">SAML v2.0 authentication request</param>
        /// <param name="language">﻿ISO 639 alpha-2 or alpha-3 language code. Optional, default is en</param>
        /// <returns>the sessionId to be used in the redirect</returns>
        /// <exception cref="HawsPushException">something went wrong, check the error code and extra info in the exception</exception>
        String push(AuthnRequestType authnRequest, String language);

        /// <summary>
        /// Fetch the SAML v2.0 authentication response for specified sessionId.
        /// </summary>
        /// <param name="sessionId">the sessionId of the SAML v2.0 authentication response</param>
        /// <returns>The SAML v2.0 authentication response</returns>
        /// <exception cref="HawsPullException">something went wrong, check the error code and extra info in the exception</exception>
        ResponseType pull(String sessionId);

        // log incoming and outgoing soap messages to the console
        void enableLogging();

    }
}
