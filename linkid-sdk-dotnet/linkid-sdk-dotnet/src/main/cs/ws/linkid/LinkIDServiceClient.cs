/*
 * linkID project.
 * 
 * Copyright 2006-2015 linkID N.V. All rights reserved.
 * linkID N.V. proprietary/confidential. Use is subject to license terms.
 */

using System;
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
    }
}
