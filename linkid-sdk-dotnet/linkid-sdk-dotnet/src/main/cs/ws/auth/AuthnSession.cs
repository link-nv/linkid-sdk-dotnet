using System;

namespace safe_online_sdk_dotnet
{
    public class AuthnSession
    {
        public string sessionId { get; set; }
        public byte[] qrCodeImage { get; set; }
        public string qrCodeImageEncoded { get; set; }
        public string qrCodeURL { get; set; }

        public Saml2AuthUtil saml2AuthUtil { get; set; }

        public AuthnSession(string sessionId, byte[] qrCodeImage, string qrCodeImageEncoded, 
            string qrCodeURL, Saml2AuthUtil saml2AuthUtil)
        {
            this.sessionId = sessionId;
            this.qrCodeImage = qrCodeImage;
            this.qrCodeImageEncoded = qrCodeImageEncoded;
            this.qrCodeURL = qrCodeURL;

            this.saml2AuthUtil = saml2AuthUtil;
        }
    }
}
