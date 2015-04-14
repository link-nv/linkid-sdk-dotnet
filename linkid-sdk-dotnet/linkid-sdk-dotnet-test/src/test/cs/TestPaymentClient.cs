using safe_online_sdk_dotnet;
using System;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

using safe_online_sdk_dotnet_test.test.cs;

namespace safe_online_sdk_dotnet.test.cs
{
    /// <summary>
	/// Test class for the Payment WS client.
	/// </summary>
    [TestFixture]
    public class TestPaymentClient
    {
        PaymentClient client = null;

        [SetUp]
        public void Init()
        {
            TestConstants.initForDevelopment();

            client = new PaymentClientImpl(TestConstants.wsLocation, TestConstants.testWsUsername, TestConstants.testWsPassword);
            client.enableLogging();
        }

        [Test]
        public void testGetStatus()
        {
            String orderReference = "afc766fe-f091-4f8e-9dec-eed83422ca43";

            LinkIDPaymentStatus paymentStatus = client.getStatus(orderReference);

            Assert.NotNull(paymentStatus);
            Console.WriteLine(paymentStatus.ToString());
        }
    }
}
