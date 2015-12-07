using System;
using System.Collections;
using System.Collections.Generic;

namespace safe_online_sdk_dotnet
{
    public class LinkIDAuthnResponse
    {
        public String userId { get; set; }
        public Dictionary<String, List<LinkIDAttribute>> attributes { get; set; }
        public LinkIDPaymentResponse paymentResponse { get; set; }
        public LinkIDExternalCodeResponse externalCodeResponse { get; set; }

        public LinkIDAuthnResponse(String userId, Dictionary<String, List<LinkIDAttribute>> attributes,
            LinkIDPaymentResponse paymentResponse, LinkIDExternalCodeResponse externalCodeResponse)
        {
            this.userId = userId;
            this.attributes = attributes;
            this.paymentResponse = paymentResponse;
            this.externalCodeResponse = externalCodeResponse;
        }

        public override string ToString()
        {
            String output = "";

            output += "UserID: " + userId + "\n";

            if (null != paymentResponse)
            {
                output += "Payment response\n";
                output += paymentResponse;
            }

            if (null != externalCodeResponse)
            {
                output += "External code response\n";
                output += externalCodeResponse;
            }

            if (null != attributes)
            {
                output += "Attributes\n";
                foreach (String key in attributes.Keys)
                {
                    foreach (LinkIDAttribute attribute in attributes[key])
                    {
                        output += "  " + attribute.ToString();
                    }
                }
            }

            return output;
        }
    }
}
