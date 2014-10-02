/*
 * SafeOnline project.
 * 
 * Copyright 2006-2008 	Lin.k N.V. All rights reserved.
 * Lin.k N.V. proprietary/confidential. Use is subject to license terms.
 */

using System;

namespace safe_online_sdk_dotnet
{
    public class PaymentTransaction
    {
        public DateTime date { get; set; }
        public double amount { get; set; }
        public Currency currency { get; set; }
        public String paymentMethod { get; set; }
        public String description { get; set; }
        public PaymentState paymentState { get; set; }
        public bool paid { get; set; }
        public String orderReference { get; set; }
        public String docdataReference { get; set; }
        public String userId { get; set; }
        public String email { get; set; }
        public String givenName { get; set; }
        public String familyName { get; set; }

        public PaymentTransaction(DateTime date, double amount, Currency currency, String paymentMethod,
            String description, PaymentState paymentState, bool paid, String orderReference, 
            String docdataReference, String userId, String email, String givenName, String familyName)
        {
            this.date = date;
            this.amount = amount;
            this.currency = currency;
            this.paymentMethod = paymentMethod;
            this.description = description;
            this.paymentState = paymentState;
            this.paid = paid;
            this.orderReference = orderReference;
            this.docdataReference = docdataReference;
            this.userId = userId;
            this.email = email;
            this.givenName = givenName;
            this.familyName = familyName;
        }
    }
}
