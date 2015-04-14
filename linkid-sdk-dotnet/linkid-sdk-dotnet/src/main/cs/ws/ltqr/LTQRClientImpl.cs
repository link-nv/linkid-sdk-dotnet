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
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;

namespace safe_online_sdk_dotnet
{
    public class LTQRClientImpl : LTQRClient
    {
        private LTQRServicePortClient client;

        public LTQRClientImpl(string location, string username, string password)
		{			
			string address = "https://" + location + "/linkid-ws-username/ltqr20";
			EndpointAddress remoteAddress = new EndpointAddress(address);

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

            this.client = new LTQRServicePortClient(binding, remoteAddress);
            this.client.Endpoint.Behaviors.Add(new PasswordDigestBehavior(username, password));
		}

        public LTQRClientImpl(string location, X509Certificate2 appCertificate, X509Certificate2 linkidCertificate)
        {
            string address = "https://" + location + "/linkid-ws/ltqr20";
            EndpointAddress remoteAddress = new EndpointAddress(address);

            this.client = new LTQRServicePortClient(new LinkIDBinding(linkidCertificate), remoteAddress);


            this.client.ClientCredentials.ClientCertificate.Certificate = appCertificate;
            this.client.ClientCredentials.ServiceCertificate.DefaultCertificate = linkidCertificate;
            // To override the validation for our self-signed test certificates
            this.client.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;

            this.client.Endpoint.Contract.ProtectionLevel = ProtectionLevel.Sign;

        }

        public void enableLogging()
        {
            this.client.Endpoint.Behaviors.Add(new LoggingBehavior());
        }

        public LTQRSession push(String authenticationMessage, String finishedMessage, LinkIDPaymentContext paymentContextDO, 
            bool oneTimeUse, Nullable<DateTime> expiryDate, Nullable<long> expiryDuration,
            LinkIDCallback callback, List<String> identityProfiles)
        {
            PushRequest request = new PushRequest();

            request.authenticationMessage = authenticationMessage;
            request.finishedMessage = finishedMessage;

            // payment context
            if (null != paymentContextDO)
            {
                LTQRWSNameSpace.PaymentContext paymentContext = new LTQRWSNameSpace.PaymentContext();
                paymentContext.amount = paymentContextDO.amount;
                paymentContext.currency = convert(paymentContextDO.currency);
                paymentContext.description = paymentContextDO.description;
                paymentContext.orderReference = paymentContextDO.orderReference;
                paymentContext.paymentProfile = paymentContextDO.paymentProfile;
                paymentContext.validationTime = paymentContextDO.paymentValidationTime;
                paymentContext.validationTimeSpecified = true;
                paymentContext.allowDeferredPay = paymentContextDO.allowDeferredPay;
                paymentContext.allowDeferredPaySpecified = true;

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
            LinkIDPaymentContext paymentContextDO, Nullable<DateTime> expiryDate, Nullable<long> expiryDuration,
            LinkIDCallback callback, List<String> identityProfiles)
        {
            ChangeRequest request = new ChangeRequest();
            request.ltqrReference = ltqrReference;

            request.authenticationMessage = authenticationMessage;
            request.finishedMessage = finishedMessage;

            // payment context
            if (null != paymentContextDO)
            {
                LTQRWSNameSpace.PaymentContext paymentContext = new LTQRWSNameSpace.PaymentContext();
                paymentContext.amount = paymentContextDO.amount;
                paymentContext.currency = convert(paymentContextDO.currency);
                paymentContext.description = paymentContextDO.description;
                paymentContext.orderReference = paymentContextDO.orderReference;
                paymentContext.paymentProfile = paymentContextDO.paymentProfile;
                paymentContext.validationTime = paymentContextDO.paymentValidationTime;
                paymentContext.validationTimeSpecified = true;
                paymentContext.allowDeferredPay = paymentContextDO.allowDeferredPay;
                paymentContext.allowDeferredPaySpecified = true;

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


        // helper methods

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
    }
}
