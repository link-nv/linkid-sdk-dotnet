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
    public class AuthnException : System.Exception
    {
        public StartErrorCode errorCode { get; set; }
        public String info { get; set; }

        public AuthnException(StartErrorCode errorCode, String info)
        {
            this.errorCode = errorCode;
            this.info = info;
        }
    }
}
