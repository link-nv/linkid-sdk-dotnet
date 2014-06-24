/*
 * SafeOnline project.
 * 
 * Copyright 2006-2008 	Lin.k N.V. All rights reserved.
 * Lin.k N.V. proprietary/confidential. Use is subject to license terms.
 */

using System;
using MandateWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class MandatePayException : System.Exception
    {
        public MandateWSNameSpace.ErrorCode errorCode { get; set; }

        public MandatePayException(MandateWSNameSpace.ErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
