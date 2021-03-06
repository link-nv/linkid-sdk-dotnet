﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace safe_online_sdk_dotnet
{
    public class LinkIDPaymentContext
    {
        public static readonly String AMOUNT_KEY           = "PaymentContext.amount";
        public static readonly String CURRENCY_KEY         = "PaymentContext.currency";
        public static readonly String WALLET_COIN_KEY      = "PaymentContext.walletCoin";
        public static readonly String DESCRIPTION_KEY      = "PaymentContext.description";
        public static readonly String ORDER_REFERENCE_KEY  = "PaymentContext.orderReference";
        public static readonly String PROFILE_KEY          = "PaymentContext.profile";
        public static readonly String VALIDATION_TIME_KEY  = "PaymentContext.validationTime";
        public static readonly String MANDATE_KEY          = "PaymentContext.mandate";
        public static readonly String MANDATE_DESCRIPTION_KEY = "PaymentContext.mandateDescription";
        public static readonly String MANDATE_REFERENCE_KEY = "PaymentContext.mandateReference";

        public static readonly String MENU_RESULT_SUCCESS_KEY  = "PaymentContext.menuResultSuccess";
        public static readonly String MENU_RESULT_CANCELED_KEY = "PaymentContext.menuResultCanceled";
        public static readonly String MENU_RESULT_PENDING_KEY  = "PaymentContext.menuResultPending";
        public static readonly String MENU_RESULT_ERROR_KEY    = "PaymentContext.menuResultError";

        public static readonly String STATUS_LOCATION_KEY = "PaymentContext.statusLocation";

        public static readonly String ALLOW_PARTIAL_KEY = "PaymentContext.allowPartial";
        public static readonly String ONLY_WALLETS_KEY  = "PaymentContext.onlyWallets";


        public LinkIDPaymentAmount amount;

        public String description { get; set; }

        // optional order reference, if not specified linkID will generate one in UUID format
        public String orderReference { get; set; }

        // optional payment profile
        public String paymentProfile { get; set; }

        // maximum time to wait for payment validation, if not specified defaults to 5s
        public int paymentValidationTime { get; set; }

        // mandate
        public LinkIDPaymentMandate mandate { get; set; }

        // optional payment menu return URLs
        public LinkIDPaymentMenu paymentMenu { get; set; }

        // optional payment status location, if not specified the default location(s) in the linkID application configuration will be used
        public String paymentStatusLocation { get; set; }

        public bool allowPartial {get; set;}    // allow partial payments via wallets
        public bool onlyWallets {get; set;}     // allow only wallets for this payments

        public LinkIDPaymentContext(LinkIDPaymentAmount amount, String description, String orderReference,
            String paymentProfile, int paymentValidationTime, LinkIDPaymentMandate mandate)
        {
            this.amount = amount;
            this.description = description;
            this.orderReference = orderReference;
            this.paymentProfile = paymentProfile;
            this.paymentValidationTime = paymentValidationTime;
            this.mandate = mandate;
        }

        public LinkIDPaymentContext(LinkIDPaymentAmount amount, String description, String orderReference, 
            String paymentProfile, int paymentValidationTime)
            : this(amount, description, orderReference, paymentProfile, paymentValidationTime, null) { }

        public LinkIDPaymentContext(LinkIDPaymentAmount amount, String description, String orderReference, 
            String paymentProfile)
            : this(amount, description, orderReference, paymentProfile, 5) {}

        public LinkIDPaymentContext(LinkIDPaymentAmount amount)
            : this(amount, null, null, null) {}

        public Dictionary<string, string> toDictionary()
        {
            if (null == amount.currency && null == amount.walletCoin)
            {
                throw new InvalidPaymentContextException("LinkIDPaymentContext.amount needs or currency or wallet specified, both are null");
            }
            if (null != amount.currency && null != amount.walletCoin)
            {
                throw new InvalidPaymentContextException("LinkIDPaymentContext.amount needs or currency or walletCoin specified, both are specified");
            }

            if (amount.amount <= 0)
            {
                throw new InvalidPaymentContextException("LinkIDPaymentContext.amount <= 0, this is not allowed");
            }

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add(AMOUNT_KEY, amount.amount.ToString());
            if (null != amount.currency)
                dictionary.Add(CURRENCY_KEY, amount.currency.Value.ToString());
            if (null != amount.walletCoin)
                dictionary.Add(WALLET_COIN_KEY, amount.walletCoin);
            if (null != description)
                dictionary.Add(DESCRIPTION_KEY, description);
            if (null != orderReference)
                dictionary.Add(ORDER_REFERENCE_KEY, orderReference);
            if (null != paymentProfile)
                dictionary.Add(PROFILE_KEY, paymentProfile);
            dictionary.Add(VALIDATION_TIME_KEY, paymentValidationTime.ToString());
            
            // mandates
            if (null != mandate)
            {
                dictionary.Add(MANDATE_KEY, "true");
                if (null != mandate.description)
                    dictionary.Add(MANDATE_DESCRIPTION_KEY, mandate.description);
                if (null != mandate.reference)
                    dictionary.Add(MANDATE_REFERENCE_KEY, mandate.reference);
            }
            else
            {
                dictionary.Add(MANDATE_KEY, "false");
            }

            // payment menu URLs
            if (null != paymentMenu)
            {
                dictionary.Add(MENU_RESULT_SUCCESS_KEY, paymentMenu.menuResultSuccess);
                dictionary.Add(MENU_RESULT_CANCELED_KEY, paymentMenu.menuResultCanceled);
                dictionary.Add(MENU_RESULT_PENDING_KEY, paymentMenu.menuResultPending);
                dictionary.Add(MENU_RESULT_ERROR_KEY, paymentMenu.menuResultError);
            }

            if (null != paymentStatusLocation)
            {
                dictionary.Add(STATUS_LOCATION_KEY, paymentStatusLocation);
            }

            // wallets
            dictionary.Add(ALLOW_PARTIAL_KEY, allowPartial.ToString());
            dictionary.Add(ONLY_WALLETS_KEY, onlyWallets.ToString());

            return dictionary;
        }
    }
}