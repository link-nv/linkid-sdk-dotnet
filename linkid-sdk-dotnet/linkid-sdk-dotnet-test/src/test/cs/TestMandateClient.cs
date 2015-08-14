﻿using safe_online_sdk_dotnet;
using System;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

using safe_online_sdk_dotnet_test.test.cs;

namespace safe_online_sdk_dotnet.test.cs
{
    /// <summary>
	/// Test class for the Mandate WS client
	/// </summary>
    [TestFixture]
    public class TestMandateClient
    {
        MandateClient client = null;

        [SetUp]
        public void Init()
        {
            TestConstants.initForDevelopment();

            client = new MandateClientImpl(TestConstants.wsLocation, TestConstants.testWsUsername, TestConstants.testWsPassword);
        }

        [Test]
        public void testPay()
        {
            String mandateReference = "2876527e-850e-4615-975c-94a29ff48fb8";
            LinkIDPaymentContext paymentContext = new LinkIDPaymentContext(new LinkIDPaymentAmount(20000, LinkIDCurrency.EUR, null), 
                ".NET Mandate Test", null, null, 10, PaymentAddBrowser.NOT_ALLOWED);

            String orderReference = client.pay(mandateReference, paymentContext, "nl");
            Assert.NotNull(orderReference);
        }
    }
}