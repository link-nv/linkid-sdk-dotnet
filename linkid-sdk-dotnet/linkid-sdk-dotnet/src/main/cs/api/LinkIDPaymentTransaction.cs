﻿using System;
using System.Collections.Generic;
using System.Text;

namespace safe_online_sdk_dotnet
{
    public class LinkIDPaymentTransaction
    {
        public LinkIDPaymentMethodType paymentMethodType { get; set; }
        public String paymentMethod { get; set; }
        public LinkIDPaymentState paymentState { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime authorizationDate { get; set; }
        public DateTime capturedDate { get; set; }
        public DateTime refundedDate { get; set; }
        public String docdataReference { get; set; }
        public double amount { get; set; }
        public LinkIDCurrency currency { get; set; }
        public double refundAmount { get; set; }

        public LinkIDPaymentTransaction(LinkIDPaymentMethodType paymentMethodType, String paymentMethod,
            LinkIDPaymentState paymentState, DateTime creationDate, DateTime authorizationDate,
            DateTime capturedDate, DateTime refundedDate, String docdataReference, 
            double amount, LinkIDCurrency currency, double refundAmount)
        {
            this.paymentMethodType = paymentMethodType;
            this.paymentMethod = paymentMethod;
            this.paymentState = paymentState;
            this.creationDate = creationDate;
            this.authorizationDate = authorizationDate;
            this.capturedDate = capturedDate;
            this.refundedDate = refundedDate;
            this.docdataReference = docdataReference;
            this.amount = amount;
            this.currency = currency;
            this.refundAmount = refundAmount;
        }

        public override String ToString()
        {
            String output = "";

            output += "    Payment transaction: \n";
            output += "      * amount: " + amount + currency + "\n";
            output += "      * paymentMethodType: " + paymentMethodType + "\n";
            output += "      * paymentMethod: " + paymentMethod + "\n";
            output += "      * paymentState: " + paymentState + "\n";
            output += "      * creationDate: " + creationDate + "\n";
            output += "      * authorizationDate: " + authorizationDate + "\n";
            output += "      * capturedDate: " + capturedDate + "\n";
            output += "      * refundedDate: " + refundedDate + "\n";
            output += "      * docdataReference: " + docdataReference + "\n";
            output += "      * refundAmount: " + refundAmount + "\n";

            return output;
        }
    }
}
