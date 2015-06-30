using System;

namespace safe_online_sdk_dotnet
{
    public class LinkIDPaymentMandate
    {
        public String description { get; set; }
        public String reference { get; set; }

        public LinkIDPaymentMandate(String description, String reference)
        {
            this.description = description;
            this.reference = reference;
        }
    }
}
