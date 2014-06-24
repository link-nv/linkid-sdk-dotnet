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
    public interface MandateClient
    {
        /// <summary>
        /// Make a payment for specified mandate
        /// </summary>
        /// <param name="mandateReference">The mandate reference</param>
        /// <param name="paymentContext">Payment context</param>
        /// <param name="language">Language</param>
        /// <returns>the order reference for this payment</returns>
        /// <exception cref="MandatePayException">something went wrong, check the error code and extra info in the exception</exception>
        String pay(String mandateReference, PaymentContext paymentContext, String language);
    }
}
