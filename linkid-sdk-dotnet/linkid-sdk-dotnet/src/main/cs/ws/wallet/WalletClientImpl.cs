﻿using System;
using System.ServiceModel;
using System.Net.Security;
using System.ServiceModel.Security;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using WalletWSNameSpace;

namespace safe_online_sdk_dotnet
{
    public class WalletClientImpl : WalletClient
    {
        private WalletServicePortClient client;

        public WalletClientImpl(string location, string username, string password)
		{			
			string address = "https://" + location + "/linkid-ws-username/wallet";
			EndpointAddress remoteAddress = new EndpointAddress(address);

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

            this.client = new WalletServicePortClient(binding, remoteAddress);
            this.client.Endpoint.Behaviors.Add(new PasswordDigestBehavior(username, password));
		}

        public void enableLogging()
        {
            this.client.Endpoint.Behaviors.Add(new LoggingBehavior());
        }

        public String enroll(String userId, String walletOrganizationId, double amount, LinkIDCurrency currency)
        {
            WalletEnrollRequest request = new WalletEnrollRequest();

            // input
            request.userId = userId;
            request.walletOrganizationId = walletOrganizationId;
            request.amount = amount;
            request.currency = convert(currency);

            WalletEnrollResponse response = this.client.enroll(request);

            // parse response
            if (null != response.error)
            {
                throw new WalletEnrollException(response.error.errorCode);
            }

            if (null != response.success)
            {
                return response.success.walletId;
            }

            throw new RuntimeException("No success nor error element in the response ?!");
        }

        public void addCredit(String userId, String walletId, double amount, LinkIDCurrency currency)
        {
            WalletAddCreditRequest request = new WalletAddCreditRequest();

            // input
            request.userId = userId;
            request.walletId = walletId;
            request.amount = amount;
            request.currency = convert(currency);

            WalletAddCreditResponse response = this.client.addCredit(request);

            // parse response
            if (null != response.error)
            {
                throw new WalletAddCreditException(response.error.errorCode);
            }

            if (null != response.success)
            {
                return;
            }
 
            throw new RuntimeException("No success nor error element in the response ?!");
        }

        public void removeCredit(String userId, String walletId, double amount, LinkIDCurrency currency)
        {
            WalletRemoveCreditRequest request = new WalletRemoveCreditRequest();

            // input
            request.userId = userId;
            request.walletId = walletId;
            request.amount = amount;
            request.currency = convert(currency);

            WalletRemoveCreditResponse response = this.client.removeCredit(request);

            // parse response
            if (null != response.error)
            {
                throw new WalletRemoveCreditException(response.error.errorCode);
            }

            if (null != response.success)
            {
                return;
            }

            throw new RuntimeException("No success nor error element in the response ?!");
        }

        public void remove(String userId, String walletId)
        {
            WalletRemoveRequest request = new WalletRemoveRequest();

            // input
            request.userId = userId;
            request.walletId = walletId;

            WalletRemoveResponse response = this.client.remove(request);

            // parse response
            if (null != response.error)
            {
                throw new WalletRemoveException(response.error.errorCode);
            }

            if (null != response.success)
            {
                return;
            }

            throw new RuntimeException("No success nor error element in the response ?!");
        }

        public void commit(String userId, String walletId, String walletTransactionId)
        {
            WalletCommitRequest request = new WalletCommitRequest();

            // input
            request.userId = userId;
            request.walletId = walletId;
            request.walletTransactionId = walletTransactionId;

            WalletCommitResponse response = this.client.commit(request);

            // parse response
            if (null != response.error)
            {
                throw new WalletCommitException(response.error.errorCode);
            }

            if (null != response.success)
            {
                return;
            }

            throw new RuntimeException("No success nor error element in the response ?!");
        }

        private Currency convert(LinkIDCurrency currency)
        {
            switch (currency)
            {
                case LinkIDCurrency.EUR: return Currency.EUR;
            }

            return Currency.EUR;
        }
    }
}
