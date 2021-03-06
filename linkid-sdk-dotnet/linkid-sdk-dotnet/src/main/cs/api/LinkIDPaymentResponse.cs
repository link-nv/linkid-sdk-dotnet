﻿using System;
using System.Collections;
using System.Collections.Generic;
using AttributeWSNamespace;

namespace safe_online_sdk_dotnet
{
    public class LinkIDPaymentResponse
    {
        public static readonly String LOCAL_NAME = "PaymentResponse";

        public static readonly String ORDER_REF_KEY = "PaymentResponse.txnId";
        public static readonly String STATE_KEY     = "PaymentResponse.state";
        public static readonly String MANDATE_REF_KEY = "PaymentResponse.mandateRef";

        ﻿public static readonly String MENU_URL_KEY     = "PaymentResponse.menuURL";

        public static readonly String DOCDATA_REF_KEY = "PaymentResponse.docdataRef";

        public String orderReference { get; set; }
        public LinkIDPaymentState paymentState { get; set; }
        public String mandateReference { get; set; }
        public String docdataReference { get; set; }

        ﻿public String paymentMenuURL { get; set; }

         public LinkIDPaymentResponse(String orderReference, LinkIDPaymentState paymentState, 
            String mandateReference, String docdataReference, String paymentMenuURL)
        {
            this.orderReference = orderReference;
            this.paymentState = paymentState;
            this.mandateReference = mandateReference;
            this.docdataReference = docdataReference;
            this.paymentMenuURL = paymentMenuURL;
        }

         public override string ToString()
         {
             String output = "";

             output += "orderReference=" + orderReference + "\n";
             output += "paymentState=" + paymentState + "\n";
             output += "mandateReference=" + mandateReference + "\n";
             output += "docdataReference=" + docdataReference + "\n";
             output += "paymentMenuURL=" + paymentMenuURL + "\n";

             return output;
         }

         public static LinkIDPaymentResponse fromDictionary(Dictionary<string, string> dictionary)
        {
            if (null == dictionary[ORDER_REF_KEY])
                throw new RuntimeException("Payment response's transaction ID field is not present!");
            if (null == dictionary[STATE_KEY])
                throw new RuntimeException("Payment response's state field is not present!");

            String mandateReference = null;
            if (null != dictionary[MANDATE_REF_KEY]) mandateReference = dictionary[MANDATE_REF_KEY];

            String docdataReference = null;
            if (null != dictionary[DOCDATA_REF_KEY]) docdataReference = dictionary[DOCDATA_REF_KEY];

            String paymentMenuURL = null;
            if (null != dictionary[MENU_URL_KEY]) paymentMenuURL = dictionary[MENU_URL_KEY];

            return new LinkIDPaymentResponse(dictionary[ORDER_REF_KEY], parse(dictionary[STATE_KEY]),
                mandateReference, docdataReference, paymentMenuURL); 
        }

        public static LinkIDPaymentResponse fromSaml(PaymentResponseType paymentResponseType)
        {
            String orderReference = null;
            String paymentStateString = null;
            String mandateReference = null;
            String docdataReference = null;
            String paymentMenuURL = null;
            foreach (object item in paymentResponseType.Items)
            {
                AttributeType attributeType = (AttributeType)item;
                if (attributeType.Name.Equals(ORDER_REF_KEY) && null != attributeType.AttributeValue)
                    orderReference = (String)attributeType.AttributeValue[0];
                if (attributeType.Name.Equals(STATE_KEY) && null != attributeType.AttributeValue)
                    paymentStateString = (String)attributeType.AttributeValue[0];
                if (attributeType.Name.Equals(MANDATE_REF_KEY) && null != attributeType.AttributeValue)
                    mandateReference = (String)attributeType.AttributeValue[0];
                if (attributeType.Name.Equals(DOCDATA_REF_KEY) && null != attributeType.AttributeValue)
                    docdataReference = (String)attributeType.AttributeValue[0];
                if (attributeType.Name.Equals(MENU_URL_KEY) && null != attributeType.AttributeValue)
                    paymentMenuURL = (String)attributeType.AttributeValue[0];
            }

            return new LinkIDPaymentResponse(orderReference, LinkIDPaymentResponse.parse(paymentStateString),
                mandateReference, docdataReference, paymentMenuURL);

        }

        public static LinkIDPaymentState parse(String paymentStateString)
        {
            if (paymentStateString.Equals(LinkIDPaymentState.STARTED.ToString()))
                return LinkIDPaymentState.STARTED;
            if (paymentStateString.Equals(LinkIDPaymentState.WAITING_FOR_UPDATE.ToString()))
                return LinkIDPaymentState.WAITING_FOR_UPDATE;
            if (paymentStateString.Equals(LinkIDPaymentState.FAILED.ToString()))
                return LinkIDPaymentState.FAILED;
            if (paymentStateString.Equals(LinkIDPaymentState.REFUNDED.ToString()))
                return LinkIDPaymentState.REFUNDED;
            if (paymentStateString.Equals(LinkIDPaymentState.REFUND_STARTED.ToString()))
                return LinkIDPaymentState.REFUND_STARTED;
            if (paymentStateString.Equals(LinkIDPaymentState.PAYED.ToString()))
                return LinkIDPaymentState.PAYED;

            throw new RuntimeException("Unsupported payment state! " + paymentStateString);
        }
    }
}