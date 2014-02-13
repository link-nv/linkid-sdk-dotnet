/*
 * SafeOnline project.
 * 
 * Copyright 2006-2008 	Lin.k N.V. All rights reserved.
 * Lin.k N.V. proprietary/confidential. Use is subject to license terms.
 */

using System;

namespace safe_online_sdk_dotnet
{
    public class LTQRSession
    {
        public byte[] qrCodeImage { get; set; }
        public String qrCodeURL { get; set; }
        public String orderReference{ get; set; }

        public LTQRSession(byte[] qrCodeImage, String qrCodeURL, String orderReference)
        {
            this.qrCodeImage = qrCodeImage;
            this.qrCodeURL = qrCodeURL;
            this.orderReference = orderReference;
        }
    }
}
