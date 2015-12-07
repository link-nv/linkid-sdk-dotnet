using System;
using LinkIDWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDAuthPollResponse
    {
        public AuthAuthenticationState linkIDAuthenticationState { get; set; }
        public LinkIDPaymentState paymentState { get; set; }
        public String paymentMenuURL { get; set; }

        public LinkIDAuthnResponse linkIDAuthnResponse { get; set; }

        public LinkIDAuthPollResponse(AuthAuthenticationState linkIDAuthenticationState,
            LinkIDPaymentState paymnentState, String paymentMenuURL, LinkIDAuthnResponse linkIDAuthnResponse)
        {
            this.linkIDAuthenticationState = linkIDAuthenticationState;
            this.paymentState = paymentState;
            this.paymentMenuURL = paymentMenuURL;
            this.linkIDAuthnResponse = linkIDAuthnResponse;
        }
    }
}
