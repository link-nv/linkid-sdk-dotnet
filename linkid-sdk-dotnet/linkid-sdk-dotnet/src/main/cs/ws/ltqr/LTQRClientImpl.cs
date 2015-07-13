/*
 * SafeOnline project.
 * 
 * Copyright 2006-2008 	Lin.k N.V. All rights reserved.
 * Lin.k N.V. proprietary/confidential. Use is subject to license terms.
 */

using LTQRWSNameSpace;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Net.Security;
using System.ServiceModel.Security;

namespace safe_online_sdk_dotnet
{
    public class LTQRClientImpl : LTQRClient
    {
        private LTQRServicePortClient client;

        public LTQRClientImpl(string location, string username, string password)
		{			
			string address = "https://" + location + "/linkid-ws-username/ltqr30";
			EndpointAddress remoteAddress = new EndpointAddress(address);

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

            this.client = new LTQRServicePortClient(binding, remoteAddress);
            this.client.Endpoint.Behaviors.Add(new PasswordDigestBehavior(username, password));
		}

        public void enableLogging()
        {
            this.client.Endpoint.Behaviors.Add(new LoggingBehavior());
        }

        public LTQRSession push(String authenticationMessage, String finishedMessage, LinkIDPaymentContext linkIDPaymentContext, 
            bool oneTimeUse, Nullable<DateTime> expiryDate, Nullable<long> expiryDuration,
            LinkIDCallback callback, List<String> identityProfiles, Nullable<long> sessionExpiryOverride, String theme,
            String mobileLandingSuccess, String mobileLandingError, String mobileLandingCancel)
        {
            PushRequest request = new PushRequest();

            request.authenticationMessage = authenticationMessage;
            request.finishedMessage = finishedMessage;

            // payment context
            if (null != linkIDPaymentContext)
            {
                LTQRWSNameSpace.PaymentContextV20 paymentContext = new LTQRWSNameSpace.PaymentContextV20();
                paymentContext.amount = linkIDPaymentContext.amount.amount;
                paymentContext.currencySpecified = linkIDPaymentContext.amount.currency.HasValue;
                if (linkIDPaymentContext.amount.currency.HasValue)
                {
                    paymentContext.currency = convert(linkIDPaymentContext.amount.currency.Value);
                }
                paymentContext.walletCoin = linkIDPaymentContext.amount.walletCoin;
                paymentContext.description = linkIDPaymentContext.description;
                paymentContext.orderReference = linkIDPaymentContext.orderReference;
                paymentContext.paymentProfile = linkIDPaymentContext.paymentProfile;
                paymentContext.validationTime = linkIDPaymentContext.paymentValidationTime;
                paymentContext.validationTimeSpecified = true;
                paymentContext.mandate = null != linkIDPaymentContext.mandate;
                if (null != linkIDPaymentContext.mandate)
                {
                    paymentContext.mandateDescription = linkIDPaymentContext.mandate.description;
                    paymentContext.mandateReference = linkIDPaymentContext.mandate.reference;
                }

                request.paymentContext = paymentContext;
            }

            // callback
            if (null != callback)
            {
                LTQRWSNameSpace.Callback callbackType = new LTQRWSNameSpace.Callback();
                callbackType.appSessionId = callback.appSessionId;
                callbackType.location = callback.location;
                callbackType.inApp = callback.inApp;
                callbackType.inAppSpecified = true;
                request.callback = callbackType;                
            }

            // identity profiles
            if (null != identityProfiles && identityProfiles.Count > 0)
            {
                request.identityProfiles = identityProfiles.ToArray();
            }

            // configuration
            request.oneTimeUse = oneTimeUse;
            if (null != expiryDate)
            {
                request.expiryDate = expiryDate.Value;
                request.expiryDateSpecified = true;
            }
            if (null != expiryDuration)
            {
                request.expiryDuration = expiryDuration.Value;
                request.expiryDurationSpecified = true;
            }
            if (null != sessionExpiryOverride)
            {
                request.sessionExpiryOverride = sessionExpiryOverride.Value;
                request.sessionExpiryOverrideSpecified = true;
            }
            request.theme = theme;
            request.mobileLandingSuccess = mobileLandingSuccess;
            request.mobileLandingError = mobileLandingError;
            request.mobileLandingCancel = mobileLandingCancel;

            // operate
            PushResponse response = this.client.push(request);

            // convert response
            if (null != response.error)
            {
                throw new PushException(convert(response.error.errorCode));
            }

            if (null != response.success)
            {

                // convert base64 encoded QR image
                byte[] qrCodeImage = Convert.FromBase64String(response.success.encodedQR);

                return new LTQRSession(qrCodeImage, response.success.qrContent, response.success.ltqrReference, 
                    response.success.paymentOrderReference);
            }

            throw new RuntimeException("No success nor error element in the response ?!");
        }

        public LTQRSession change(String ltqrReference, String authenticationMessage, String finishedMessage,
            LinkIDPaymentContext linkIDPaymentContext, Nullable<DateTime> expiryDate, Nullable<long> expiryDuration,
            LinkIDCallback callback, List<String> identityProfiles, Nullable<long> sessionExpiryOverride, String theme,
            String mobileLandingSuccess, String mobileLandingError, String mobileLandingCancel, bool resetUsed)
        {
            ChangeRequest request = new ChangeRequest();
            request.ltqrReference = ltqrReference;

            request.authenticationMessage = authenticationMessage;
            request.finishedMessage = finishedMessage;

            // payment context
            if (null != linkIDPaymentContext)
            {
                LTQRWSNameSpace.PaymentContextV20 paymentContext = new LTQRWSNameSpace.PaymentContextV20();
                paymentContext.amount = linkIDPaymentContext.amount.amount;
                paymentContext.currencySpecified = linkIDPaymentContext.amount.currency.HasValue;
                if (linkIDPaymentContext.amount.currency.HasValue)
                {
                    paymentContext.currency = convert(linkIDPaymentContext.amount.currency.Value);
                }
                paymentContext.walletCoin = linkIDPaymentContext.amount.walletCoin;
                paymentContext.description = linkIDPaymentContext.description;
                paymentContext.orderReference = linkIDPaymentContext.orderReference;
                paymentContext.paymentProfile = linkIDPaymentContext.paymentProfile;
                paymentContext.validationTime = linkIDPaymentContext.paymentValidationTime;
                paymentContext.validationTimeSpecified = true;

                request.paymentContext = paymentContext;
            }

            // callback
            if (null != callback)
            {
                LTQRWSNameSpace.Callback callbackType = new LTQRWSNameSpace.Callback();
                callbackType.appSessionId = callback.appSessionId;
                callbackType.location = callback.location;
                callbackType.inApp = callback.inApp;
                callbackType.inAppSpecified = true;
                request.callback = callbackType;
            }

            // identity profiles
            if (null != identityProfiles && identityProfiles.Count > 0)
            {
                request.identityProfiles = identityProfiles.ToArray();
            }

            // configuration
            if (null != expiryDate)
            {
                request.expiryDate = expiryDate.Value;
                request.expiryDateSpecified = true;
            }
            if (null != expiryDuration)
            {
                request.expiryDuration = expiryDuration.Value;
                request.expiryDurationSpecified = true;
            }
            if (null != sessionExpiryOverride)
            {
                request.sessionExpiryOverride = sessionExpiryOverride.Value;
                request.sessionExpiryOverrideSpecified = true;
            }
            request.theme = theme;
            request.mobileLandingSuccess = mobileLandingSuccess;
            request.mobileLandingError = mobileLandingError;
            request.mobileLandingCancel = mobileLandingCancel;

            request.resetUsedSpecified = true;
            request.resetUsed = resetUsed;

            // operate
            ChangeResponse response = this.client.change(request);

            // convert response
            if (null != response.error)
            {
                throw new ChangeException(convert(response.error.errorCode));
            }

            if (null != response.success)
            {
                // convert base64 encoded QR image
                byte[] qrCodeImage = Convert.FromBase64String(response.success.encodedQR);

                return new LTQRSession(qrCodeImage, response.success.qrContent, 
                    response.success.ltqrReference, response.success.paymentOrderReference);
            }

            throw new RuntimeException("No success nor error element in the response ?!");
        }


        public LTQRClientSession[] pull(String[] ltqrReferences, String[] paymentOrderReferences, String[] clientSessionIds)
        {
            PullRequest request = new PullRequest();

            if (null != ltqrReferences)
            {
                request.ltqrReferences = ltqrReferences;
            }

            if (null != paymentOrderReferences)
            {
                request.paymentOrderReferences = paymentOrderReferences;
            }

            if (null != clientSessionIds)
            {
                request.clientSessionIds = clientSessionIds;
            }

            // operate
            PullResponse response = this.client.pull(request);

            // convert response
            if (null != response.error)
            {
                throw new PullException(convert(response.error.errorCode));
            }

            if (null != response.success)
            {

                LTQRClientSession[] clientSessions = new LTQRClientSession[response.success.Length];
                int i = 0;
                foreach(ClientSession clientSession in response.success)
                {
                    clientSessions[i++] = new LTQRClientSession(clientSession.ltqrReference, clientSession.clientSessionId, clientSession.userId, 
                        clientSession.created, clientSession.paymentStatus, clientSession.paymentOrderReference);
                }
                return clientSessions;

            }

            throw new RuntimeException("No success nor error element in the response ?!");
        }

        public void remove(String[] ltqrReferences, String[] paymentOrderReferences, String[] clientSessionIds)
        {
            RemoveRequest request = new RemoveRequest();

            if (null == ltqrReferences || 0 == ltqrReferences.Length)
            {
                throw new RuntimeException("ltqrReferences list cannot be empty");
            }

            request.ltqrReferences = ltqrReferences;
            
            if (null != paymentOrderReferences)
            {
                request.paymentOrderReferences = paymentOrderReferences;
            }

            if (null != clientSessionIds)
            {
                request.clientSessionIds = clientSessionIds;
            }

            // operate
            RemoveResponse response = this.client.remove(request);

            // convert response
            if (null != response.error)
            {
                throw new PullException(convert(response.error.errorCode));
            }

            if (null != response.success)
            {
                return;
            }

            throw new RuntimeException("No success nor error element in the response ?!");
        }

        public List<LinkIDLTQRInfo> info(String[] ltqrReferences)
        {
            if (null == ltqrReferences || 0 == ltqrReferences.Length)
            {
                throw new RuntimeException("ltqrReferences list cannot be empty");
            }

            // operate
            InfoResponse response = this.client.info(ltqrReferences);

            // convert response
            if (null != response.error)
            {
                throw new LinkIDLTQRInfoException(convert(response.error.errorCode));
            }

            if (null != response.success)
            {
                List<LinkIDLTQRInfo> infos = new List<LinkIDLTQRInfo>();

                foreach (LTQRInfo ltqrInfo in response.success)
                {
                    // convert base64 encoded QR image
                    byte[] qrCodeImage = Convert.FromBase64String(ltqrInfo.encodedQR);

                    infos.Add(new LinkIDLTQRInfo(ltqrInfo.ltqrReference, ltqrInfo.sessionId, ltqrInfo.created,
                        qrCodeImage, ltqrInfo.qrContent, ltqrInfo.authenticationMessage, ltqrInfo.finishedMessage,
                        ltqrInfo.oneTimeUse, ltqrInfo.expiryDate, 
                        ltqrInfo.expiryDurationSpecified? ltqrInfo.expiryDuration : 0,
                        getPaymentContext(ltqrInfo), getCallback(ltqrInfo), ltqrInfo.identityProfiles, 
                        ltqrInfo.sessionExpiryOverrideSpecified? ltqrInfo.sessionExpiryOverride : 0,
                        ltqrInfo.theme, ltqrInfo.mobileLandingSuccess, ltqrInfo.mobileLandingError, 
                        ltqrInfo.mobileLandingCancel));
                }

                return infos;
            }

            throw new RuntimeException("No success nor error element in the response ?!");
        }



        // helper methods

        private LinkIDPaymentContext getPaymentContext(LTQRInfo ltqrInfo)
        {
            if (null == ltqrInfo.paymentContext) return null;

            LinkIDPaymentAmount amount;
            if (ltqrInfo.paymentContext.currencySpecified)
            {
                amount = new LinkIDPaymentAmount(ltqrInfo.paymentContext.amount, convert(ltqrInfo.paymentContext.currency), null);
            }
            else
            {
                amount = new LinkIDPaymentAmount(ltqrInfo.paymentContext.amount, null, ltqrInfo.paymentContext.walletCoin);
            }

            LinkIDPaymentMandate mandate = null;
            if (ltqrInfo.paymentContext.mandate)
            {
                mandate = new LinkIDPaymentMandate(ltqrInfo.paymentContext.mandateDescription, ltqrInfo.paymentContext.mandateReference);
            }

            return new LinkIDPaymentContext(amount, ltqrInfo.paymentContext.description, ltqrInfo.paymentContext.orderReference, ltqrInfo.paymentContext.paymentProfile,
                ltqrInfo.paymentContext.validationTimeSpecified ? ltqrInfo.paymentContext.validationTime : 5,
                PaymentAddBrowser.NOT_ALLOWED, mandate);
        }

        private LinkIDCallback getCallback(LTQRInfo ltqrInfo)
        {
            if (null == ltqrInfo.callback) return null;
            
            return new LinkIDCallback(ltqrInfo.callback.location, ltqrInfo.callback.appSessionId, 
                ltqrInfo.callback.inAppSpecified ? ltqrInfo.callback.inApp : false);
        }

        private ErrorCode convert(LTQRWSNameSpace.ErrorCode errorCode)
        {
            switch (errorCode)
            {
                case LTQRWSNameSpace.ErrorCode.errorcredentialsinvalid: return ErrorCode.ERROR_CREDENTIALS_INVALID;
            }

            throw new RuntimeException("Unexpected error code " + errorCode.ToString() + "!");
        }

        private ChangeErrorCode convert(LTQRWSNameSpace.ChangeErrorCode errorCode)
        {
            switch (errorCode)
            {
                case LTQRWSNameSpace.ChangeErrorCode.errorcredentialsinvalid: return ChangeErrorCode.ERROR_CREDENTIALS_INVALID;
                case LTQRWSNameSpace.ChangeErrorCode.errornotfound: return ChangeErrorCode.ERROR_NOT_FOUND;
            }

            throw new RuntimeException("Unexpected error code " + errorCode.ToString() + "!");
        }

        private LTQRWSNameSpace.Currency convert(LinkIDCurrency currency)
        {
            switch (currency)
            {
                case LinkIDCurrency.EUR:
                    return LTQRWSNameSpace.Currency.EUR;
            }

            throw new RuntimeException("Currency " + currency.ToString() + " is not supported!");
        }

        private LinkIDCurrency convert(LTQRWSNameSpace.Currency currency)
        {
            switch (currency)
            {
                case LTQRWSNameSpace.Currency.EUR:
                    return LinkIDCurrency.EUR;
            }

            throw new RuntimeException("Currency " + currency.ToString() + " is not supported!");
        }
    }
}
