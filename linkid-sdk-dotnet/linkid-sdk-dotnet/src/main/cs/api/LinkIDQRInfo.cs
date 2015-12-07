using System;

namespace safe_online_sdk_dotnet
{
    public class LinkIDQRInfo
    {
        public byte[] qrImage { get; set; }
        public String qrEncoded { get; set; }
        public String qrCodeURL { get; set; }
        public String qrContent { get; set; }
        public bool mobile { get; set; }

        public LinkIDQRInfo(byte[] qrImage, String qrEncoded, String qrCodeURL, String qrContent, bool mobile)
        {
            this.qrImage = qrImage;
            this.qrEncoded = qrEncoded;
            this.qrCodeURL = qrCodeURL;
            this.qrContent = qrContent;
            this.mobile = mobile;
        }
    }
}
