using System;
using System.Collections;
using System.Collections.Generic;

namespace safe_online_sdk_dotnet
{
    public enum Currency {EUR};

    public class PaymentContext
    {
        public static readonly String AMOUNT_KEY           = "PaymentContext.amount";
        public static readonly String CURRENCY_KEY         = "PaymentContext.currency";
        public static readonly String DESCRIPTION_KEY      = "PaymentContext.description";
        public static readonly String ORDER_REFERENCE_KEY  = "PaymentContext.orderReference";
        public static readonly String PROFILE_KEY          = "PaymentContext.profile";
        public static readonly String VALIDATION_TIME_KEY  = "PaymentContext.validationTime";
        public static readonly String ADD_LINK_KEY         = "PaymentContext.addLinkKey";
        public static readonly String DEFERRED_PAY_KEY     = "PaymentContext.deferredPay";

        // amount to pay, carefull amount is in cents!!
        public Double amount { get; set; }
        public Currency currency { get; set; }
        public String description { get; set; }

        // optional order reference, if not specified linkID will generate one in UUID format
        public String orderReference { get; set; }

        // optional payment profile
        public String paymentProfile { get; set; }

        // maximum time to wait for payment validation, if not specified defaults to 5s
        public int paymentValidationTime { get; set; }

        // whether or not to display a link to linkID's payment method page if the linkID user has no payment methods added, default is true
        public Boolean showAddPaymentMethodLink { get; set; }

        // whether or not deferred payments are allowed, if a user has no payment token attached to the linkID account
        // linkID can allow for the user to make a deferred payment which he can complete later on from this browser.
        public Boolean allowDeferredPay;

        public PaymentContext(Double amount, Currency currency, String description, String orderReference, 
            String paymentProfile, int paymentValidationTime, Boolean showAddPaymentMethodLink, Boolean allowDeferredPay)
        {
            this.amount = amount;
            this.currency = currency;
            this.description = description;
            this.orderReference = orderReference;
            this.paymentProfile = paymentProfile;
            this.paymentValidationTime = paymentValidationTime;
            this.showAddPaymentMethodLink = showAddPaymentMethodLink;
            this.allowDeferredPay = allowDeferredPay;
        }

        public PaymentContext(double amount, Currency currency, String description, String orderReference, String paymentProfile)
        {
            this.amount = amount;
            this.currency = currency;
            this.description = description;
            this.orderReference = orderReference;
            this.paymentProfile = paymentProfile;
            this.paymentValidationTime = 5;
            this.showAddPaymentMethodLink = true;
            this.allowDeferredPay = false;
        }

        public PaymentContext(double amount, Currency currency)
        {
            this.amount = amount;
            this.currency = currency;
            this.paymentValidationTime = 5;
            this.showAddPaymentMethodLink = true;
            this.allowDeferredPay = false;
        }

        public Dictionary<string, string> toDictionary()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add(AMOUNT_KEY, amount.ToString());
            dictionary.Add(CURRENCY_KEY, currency.ToString());
            if (null != description)
                dictionary.Add(DESCRIPTION_KEY, description);
            if (null != orderReference)
                dictionary.Add(ORDER_REFERENCE_KEY, orderReference);
            if (null != paymentProfile)
                dictionary.Add(PROFILE_KEY, paymentProfile);
            dictionary.Add(VALIDATION_TIME_KEY, paymentValidationTime.ToString());
            dictionary.Add(ADD_LINK_KEY, showAddPaymentMethodLink.ToString());
            dictionary.Add(DEFERRED_PAY_KEY, allowDeferredPay.ToString());
            return dictionary;
        }
    }
}