/*
 * SafeOnline project.
 * 
 * Copyright 2006-2008 	Lin.k N.V. All rights reserved.
 * Lin.k N.V. proprietary/confidential. Use is subject to license terms.
 */

using System;
using HawsWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class HawsPushException : System.Exception
    {
        public PushErrorCode errorCode { get; set; }
        public String info { get; set; }

        public HawsPushException(PushErrorCode errorCode, String info)
        {
            this.errorCode = errorCode;
            this.info = info;
        }
    }
}
