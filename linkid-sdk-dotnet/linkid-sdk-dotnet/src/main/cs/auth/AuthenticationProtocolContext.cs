/*
 * Created by SharpDevelop.
 * User: devel
 * Date: 22/12/2008
 * Time: 12:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Collections.Generic;

namespace safe_online_sdk_dotnet
{
	/// <summary>
	/// Description of AuthenticationProtocolContext.
	/// </summary>
	public class AuthenticationProtocolContext
	{
        public String userId { get; set; }
        public List<String> authenticatedDevices { get; set; }
        public Dictionary<String, List<LinkIDAttribute>> attributes { get; set; }
        public LinkIDPaymentResponse paymentResponse { get; set; }

        public AuthenticationProtocolContext()
        {
            this.userId = null;
            this.authenticatedDevices = null;
            this.attributes = null;
            this.paymentResponse = null;
        }

        public AuthenticationProtocolContext(string userId, List<String> authenticatedDevices,
            Dictionary<String, List<LinkIDAttribute>> attributes, LinkIDPaymentResponse paymentResponse)
		{
			this.userId = userId;
			this.authenticatedDevices = authenticatedDevices;
			this.attributes = attributes;
            this.paymentResponse = paymentResponse;
		}

        public override string ToString()
        {
            String output = "";

            output += "AuthenticationContext:\n";
            output += "  * UserID: " + userId + "\n";
            foreach (String authenticatedDevice in authenticatedDevices)
            {
                output += "  * Authenticated device: " + authenticatedDevice + "\n";
            }

            if (null != paymentResponse)
            {
                output += "  * Payment response\n";
                output += "      - orderReference: " + paymentResponse.orderReference + "\n";
                output += "      - paymentState: " + paymentResponse.paymentState + "\n";
                output += "      - paymentMenuURL: " + paymentResponse.paymentMenuURL + "\n";
                output += "      - docdataReference: " + paymentResponse.docdataReference + "\n";
                output += "      - mandateReference: " + paymentResponse.mandateReference + "\n";
            }

            if (null != attributes)
            {
                output += "  * Attributes\n";
                foreach (String key in attributes.Keys)
                {
                    foreach(LinkIDAttribute attribute in attributes[key])
                    {
                        output += "    " + attribute.ToString();
                    }
                }
            }

            return output;
        }
	}
}
