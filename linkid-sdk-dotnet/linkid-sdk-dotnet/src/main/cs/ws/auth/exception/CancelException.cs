/*
 * SafeOnline project.
 * 
 * Copyright 2006-2008 	Lin.k N.V. All rights reserved.
 * Lin.k N.V. proprietary/confidential. Use is subject to license terms.
 */

using System;
using AuthWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class CancelException : System.Exception
    {
        public CancelErrorCode errorCode { get; set; }
        public String info { get; set; }

        public CancelException(CancelErrorCode errorCode, String info)
        {
            this.errorCode = errorCode;
            this.info = info;
        }
    }
}
