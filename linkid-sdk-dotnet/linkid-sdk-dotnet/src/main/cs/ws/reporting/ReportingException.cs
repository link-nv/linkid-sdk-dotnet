/*
 * SafeOnline project.
 * 
 * Copyright 2006-2008 	Lin.k N.V. All rights reserved.
 * Lin.k N.V. proprietary/confidential. Use is subject to license terms.
 */

using System;
using ReportingWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class ReportingException : System.Exception
    {
        public ReportingWSNameSpace.ErrorCode errorCode { get; set; }

        public ReportingException(ReportingWSNameSpace.ErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
