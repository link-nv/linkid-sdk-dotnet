using AuthWSNameSpace;
using System;

namespace safe_online_sdk_dotnet
{
    public class PollResponse
    {
        public AuthenticationState authenticationState { get; set; }
        public LinkIDPaymentState paymentState { get; set; }
        public String paymentMenuURL { get; set; }
        public AuthenticationProtocolContext authenticationContext { get; set; }

        public PollResponse(AuthenticationState authenticationState, AuthWSNameSpace.PaymentState paymentState,
            String paymentMenuURL, AuthenticationProtocolContext authenticationContext)
        {
            this.authenticationState = authenticationState;
            this.paymentState = convert(paymentState);
            this.paymentMenuURL = paymentMenuURL;
            this.authenticationContext = authenticationContext;
        }

        public static LinkIDPaymentState convert(AuthWSNameSpace.PaymentState wsPaymentState)
        {
            switch (wsPaymentState)
            {
                case AuthWSNameSpace.PaymentState.linkidpaymentstatestarted:
                    return LinkIDPaymentState.STARTED;
                case AuthWSNameSpace.PaymentState.linkidpaymentstatefailed:
                    return LinkIDPaymentState.FAILED;
                case AuthWSNameSpace.PaymentState.linkidpaymentstatepayed:
                    return LinkIDPaymentState.PAYED;
                case AuthWSNameSpace.PaymentState.linkidpaymentstaterefund_started:
                    return LinkIDPaymentState.REFUND_STARTED;
                case AuthWSNameSpace.PaymentState.linkidpaymentstaterefunded:
                    return LinkIDPaymentState.REFUNDED;
                case AuthWSNameSpace.PaymentState.linkidpaymentstatewaiting:
                    return LinkIDPaymentState.WAITING_FOR_UPDATE;
            }

            throw new RuntimeException("Unexpected payment state:" + wsPaymentState);
        }
    }
}
