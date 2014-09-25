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
        public String ltqrReference{ get; set; }

        public String paymentOrderReference { get; set; }   // optional payment order ref, if applicable

        public LTQRSession(byte[] qrCodeImage, String qrCodeURL, String ltqrReference, 
            String paymentOrderReference)
        {
            this.qrCodeImage = qrCodeImage;
            this.qrCodeURL = qrCodeURL;
            this.ltqrReference = ltqrReference;
            this.paymentOrderReference = paymentOrderReference;
        }
    }
}
