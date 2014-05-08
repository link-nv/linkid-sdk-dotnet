using AuthWSNameSpace;
using System;

namespace safe_online_sdk_dotnet
{
    public class PollResponse
    {
        public AuthenticationState authenticationState { get; set; }
        public PaymentState paymentState { get; set; }
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

        public static PaymentState convert(AuthWSNameSpace.PaymentState wsPaymentState)
        {
            switch (wsPaymentState)
            {
                case AuthWSNameSpace.PaymentState.linkidpaymentstatestarted:
                    return PaymentState.STARTED;
                case AuthWSNameSpace.PaymentState.linkidpaymentstatedeferred:
                    return PaymentState.DEFERRED;
                case AuthWSNameSpace.PaymentState.linkidpaymentstatefailed:
                    return PaymentState.FAILED;
                case AuthWSNameSpace.PaymentState.linkidpaymentstatepayed:
                    return PaymentState.PAYED;
                case AuthWSNameSpace.PaymentState.linkidpaymentstaterefund_started:
                    return PaymentState.REFUND_STARTED;
                case AuthWSNameSpace.PaymentState.linkidpaymentstaterefunded:
                    return PaymentState.REFUNDED;
                case AuthWSNameSpace.PaymentState.linkidpaymentstatewaiting:
                    return PaymentState.WAITING_FOR_UPDATE;
            }

            throw new RuntimeException("Unexpected payment state:" + wsPaymentState);
        }
    }
}
