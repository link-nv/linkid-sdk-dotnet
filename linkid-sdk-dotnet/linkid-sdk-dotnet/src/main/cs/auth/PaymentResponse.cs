using System;
using System.Collections;
using System.Collections.Generic;
using AttributeWSNamespace;

namespace safe_online_sdk_dotnet
{
    public enum PaymentState { STARTED, DEFERRED, WAITING_FOR_UPDATE, FAILED, REFUNDED, REFUND_STARTED, PAYED };

    public class PaymentResponse
    {
        public static readonly String LOCAL_NAME = "PaymentResponse";

        public static readonly String ORDER_REF_KEY = "PaymentResponse.txnId";
        public static readonly String STATE_KEY     = "PaymentResponse.state";
        public static readonly String MANDATE_REF_KEY = "PaymentResponse.mandateRef";

        public static readonly String DOCDATA_REF_KEY = "PaymentResponse.docdataRef";

        public String orderReference { get; set; }
        public PaymentState paymentState { get; set; }
        public String mandateReference { get; set; }
        public String docdataReference { get; set; }

        public PaymentResponse(String orderReference, PaymentState paymentState, String mandateReference, String docdataReference)
        {
            this.orderReference = orderReference;
            this.paymentState = paymentState;
            this.mandateReference = mandateReference;
            this.docdataReference = docdataReference;
        }

        public static PaymentResponse fromDictionary(Dictionary<string, string> dictionary)
        {
            if (null == dictionary[ORDER_REF_KEY])
                throw new RuntimeException("Payment response's transaction ID field is not present!");
            if (null == dictionary[STATE_KEY])
                throw new RuntimeException("Payment response's state field is not present!");

            String mandateReference = null;
            if (null != dictionary[MANDATE_REF_KEY]) mandateReference = dictionary[MANDATE_REF_KEY];

            String docdataReference = null;
            if (null != dictionary[DOCDATA_REF_KEY]) docdataReference = dictionary[DOCDATA_REF_KEY];

            return new PaymentResponse(dictionary[ORDER_REF_KEY], parse(dictionary[STATE_KEY]), mandateReference, docdataReference); 
        }

        public static PaymentResponse fromSaml(PaymentResponseType paymentResponseType)
        {
            String orderReference = null;
            String paymentStateString = null;
            String mandateReference = null;
            String docdataReference = null;
            foreach (object item in paymentResponseType.Items)
            {
                AttributeType attributeType = (AttributeType)item;
                if (attributeType.Name.Equals(ORDER_REF_KEY))
                    orderReference = (String)attributeType.AttributeValue[0];
                if (attributeType.Name.Equals(STATE_KEY))
                    paymentStateString = (String)attributeType.AttributeValue[0];
                if (attributeType.Name.Equals(MANDATE_REF_KEY))
                    mandateReference = (String)attributeType.AttributeValue[0];
                if (attributeType.Name.Equals(DOCDATA_REF_KEY))
                    docdataReference = (String)attributeType.AttributeValue[0];
            }

            return new PaymentResponse(orderReference, PaymentResponse.parse(paymentStateString), mandateReference, docdataReference);

        }

        public static PaymentState parse(String paymentStateString)
        {
            if (paymentStateString.Equals(PaymentState.STARTED.ToString()))
                return PaymentState.STARTED;
            if (paymentStateString.Equals(PaymentState.DEFERRED.ToString()))
                return PaymentState.DEFERRED;
            if (paymentStateString.Equals(PaymentState.WAITING_FOR_UPDATE.ToString()))
                return PaymentState.WAITING_FOR_UPDATE;
            if (paymentStateString.Equals(PaymentState.FAILED.ToString()))
                return PaymentState.FAILED;
            if (paymentStateString.Equals(PaymentState.REFUNDED.ToString()))
                return PaymentState.REFUNDED;
            if (paymentStateString.Equals(PaymentState.REFUND_STARTED.ToString()))
                return PaymentState.REFUND_STARTED;
            if (paymentStateString.Equals(PaymentState.PAYED.ToString()))
                return PaymentState.PAYED;

            throw new RuntimeException("Unsupported payment state! " + paymentStateString);
        }
    }
}