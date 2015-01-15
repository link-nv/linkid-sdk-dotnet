using System;
using System.Collections;
using System.Collections.Generic;

namespace safe_online_sdk_dotnet
{
    public enum Currency {EUR};

    public enum PaymentAddBrowser { NOT_ALLOWED, REDIRECT };

    public class PaymentContext
    {
        public static readonly String AMOUNT_KEY           = "PaymentContext.amount";
        public static readonly String CURRENCY_KEY         = "PaymentContext.currency";
        public static readonly String DESCRIPTION_KEY      = "PaymentContext.description";
        public static readonly String ORDER_REFERENCE_KEY  = "PaymentContext.orderReference";
        public static readonly String PROFILE_KEY          = "PaymentContext.profile";
        public static readonly String VALIDATION_TIME_KEY  = "PaymentContext.validationTime";
        public static readonly String ﻿ADD_BROWSER_KEY      = "PaymentContext.addBrowser";
        public static readonly String DEFERRED_PAY_KEY     = "PaymentContext.deferredPay";
        public static readonly String MANDATE_KEY          = "PaymentContext.mandate";
        public static readonly String MANDATE_DESCRIPTION_KEY = "PaymentContext.mandateDescription";
        public static readonly String MANDATE_REFERENCE_KEY = "PaymentContext.mandateReference";

        public static readonly String MENU_RESULT_SUCCESS_KEY  = "PaymentContext.menuResultSuccess";
        public static readonly String MENU_RESULT_CANCELED_KEY = "PaymentContext.menuResultCanceled";
        public static readonly String MENU_RESULT_PENDING_KEY  = "PaymentContext.menuResultPending";
        public static readonly String MENU_RESULT_ERROR_KEY    = "PaymentContext.menuResultError";


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

        // ﻿whether or not to allow to display the option in the client to add a payment method in the browser. default is not allowed
        public PaymentAddBrowser paymentAddBrowser { get; set; }

        // whether or not deferred payments are allowed, if a user has no payment token attached to the linkID account
        // linkID can allow for the user to make a deferred payment which he can complete later on from this browser.
        public Boolean allowDeferredPay {get; set;}

        // mandates
        public Boolean mandate { get; set; }
        public String mandateDescription { get; set; }
        public String mandateReference { get; set; }

        // optional payment menu return URLs (returnPaymentMenuURL)
        public String paymentMenuResultSuccess { get; set; }
        public String paymentMenuResultCanceled { get; set; }
        public String paymentMenuResultPending { get; set; }
        public String paymentMenuResultError { get; set; }

        public PaymentContext(Double amount, Currency currency, String description, String orderReference,
            String paymentProfile, int paymentValidationTime, 
            PaymentAddBrowser paymentAddBrowser, Boolean allowDeferredPay,
            Boolean mandate, String mandateDescription, String mandateReference)
        {
            if (amount <= 0)
            {
                throw new InvalidPaymentContextException("Invalid payment context amount: " + amount);
            }

            this.amount = amount;
            this.currency = currency;
            this.description = description;
            this.orderReference = orderReference;
            this.paymentProfile = paymentProfile;
            this.paymentValidationTime = paymentValidationTime;
            this.paymentAddBrowser = paymentAddBrowser;
            this.allowDeferredPay = allowDeferredPay;
            this.mandate = mandate;
            this.mandateDescription = mandateDescription;
            this.mandateReference = mandateReference;
        }

        public PaymentContext(Double amount, Currency currency, String description, String orderReference, 
            String paymentProfile, int paymentValidationTime, PaymentAddBrowser paymentAddBrowser, Boolean allowDeferredPay)
            : this(amount, currency, description, orderReference, paymentProfile, paymentValidationTime,
                paymentAddBrowser, allowDeferredPay, false, null, null) { }

        public PaymentContext(double amount, Currency currency, String description, String orderReference, String paymentProfile)
            : this(amount, currency, description, orderReference, paymentProfile, 5, PaymentAddBrowser.REDIRECT, false) {}

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
            if (null != paymentAddBrowser)
            {
                dictionary.Add(ADD_BROWSER_KEY, paymentAddBrowser.ToString());
            }
            dictionary.Add(DEFERRED_PAY_KEY, allowDeferredPay.ToString());

            // mandates
            dictionary.Add(MANDATE_KEY, mandate.ToString());
            if (null != mandateDescription)
                dictionary.Add(MANDATE_DESCRIPTION_KEY, mandateDescription);
            if (null != mandateReference)
                dictionary.Add(MANDATE_REFERENCE_KEY, mandateReference);

            // payment menu URLs
            if (null != paymentMenuResultSuccess)
                dictionary.Add(MENU_RESULT_SUCCESS_KEY, paymentMenuResultSuccess);
            if (null != paymentMenuResultCanceled)
                dictionary.Add(MENU_RESULT_CANCELED_KEY, paymentMenuResultCanceled);
            if (null != paymentMenuResultPending)
                dictionary.Add(MENU_RESULT_PENDING_KEY, paymentMenuResultPending);
            if (null != paymentMenuResultError)
                dictionary.Add(MENU_RESULT_ERROR_KEY, paymentMenuResultError);

            return dictionary;
        }
    }
}