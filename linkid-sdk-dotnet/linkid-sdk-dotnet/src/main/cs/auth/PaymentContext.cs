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
        public static readonly String MANDATE_KEY = "PaymentContext.mandate";
        public static readonly String MANDATE_DESCRIPTION_KEY = "PaymentContext.mandateDescription";
        public static readonly String MANDATE_REFERENCE_KEY = "PaymentContext.mandateReference";

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
        public Boolean allowDeferredPay {get; set;}

        // mandates
        public Boolean mandate { get; set; }
        public String mandateDescription { get; set; }
        public String mandateReference { get; set; }

        public PaymentContext(Double amount, Currency currency, String description, String orderReference,
            String paymentProfile, int paymentValidationTime, Boolean showAddPaymentMethodLink, Boolean allowDeferredPay,
            Boolean mandate, String mandateDescription, String mandateReference)
        {
            this.amount = amount;
            this.currency = currency;
            this.description = description;
            this.orderReference = orderReference;
            this.paymentProfile = paymentProfile;
            this.paymentValidationTime = paymentValidationTime;
            this.showAddPaymentMethodLink = showAddPaymentMethodLink;
            this.allowDeferredPay = allowDeferredPay;
            this.mandate = mandate;
            this.mandateDescription = mandateDescription;
            this.mandateReference = mandateReference;
        }

        public PaymentContext(Double amount, Currency currency, String description, String orderReference, 
            String paymentProfile, int paymentValidationTime, Boolean showAddPaymentMethodLink, Boolean allowDeferredPay)
            : this(amount, currency, description, orderReference, paymentProfile, paymentValidationTime,
                showAddPaymentMethodLink, allowDeferredPay, false, null, null) {}

        public PaymentContext(double amount, Currency currency, String description, String orderReference, String paymentProfile)
            : this(amount, currency, description, orderReference, paymentProfile, 5, true, false) {}

        public PaymentContext(double amount, Currency currency)
            : this(amount, currency, null, null, null) {}

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

            // mandates
            dictionary.Add(MANDATE_KEY, mandate.ToString());
            if (null != mandateDescription)
                dictionary.Add(MANDATE_DESCRIPTION_KEY, mandateDescription);
            if (null != mandateReference)
                dictionary.Add(MANDATE_REFERENCE_KEY, mandateReference);

            return dictionary;
        }
    }
}