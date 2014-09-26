using safe_online_sdk_dotnet;
using System;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

using safe_online_sdk_dotnet_test.test.cs;

namespace safe_online_sdk_dotnet.test.cs
{
    /// <summary>
	/// Test class for the LTQR WS client.
	/// </summary>
    [TestFixture]
    public class TestLTQRClient
    {
        LTQRClient client = null;

        [SetUp]
        public void Init()
        {
            TestConstants.initForDevelopment();

            client = new LTQRClientImpl(TestConstants.wsLocation, TestConstants.testWsUsername, TestConstants.testWsPassword);
        }

        [Test]
        public void testPush()
        {
            String finishedMessage = "Custom finished msg";
            String paymentOrderReference = "DOTNET-LTQR-" + Guid.NewGuid().ToString();
            PaymentContext paymentContext = new PaymentContext(20000, Currency.EUR, ".NET Test", paymentOrderReference, null);
            DateTime expiryDate = DateTime.Now.AddMonths(3);

            LTQRSession session = client.push(null, finishedMessage, paymentContext, false, expiryDate, null);

            Assert.NotNull(session);
        }

        [Test]
        public void testChange()
        {
            String finishedMessage = "Changed custom finished msg";
            String ltqrReference = "f077493e-badd-4f13-91f1-bd3fd58dbd82";
            String paymentOrderReference = "DOTNET-LTQR-" + Guid.NewGuid().ToString();

            PaymentContext paymentContext = new PaymentContext(9999, Currency.EUR, ".NET Test Changed", paymentOrderReference, null);
            DateTime expiryDate = DateTime.Now.AddMonths(12);

            client.change(ltqrReference, null, finishedMessage, paymentContext, expiryDate, null);
        }

        [Test]
        public void testPull()
        {
            string[] ltqrReferences = new string[] { "f077493e-badd-4f13-91f1-bd3fd58dbd82" };
            string[] paymentOrderReferences = new string[] {  };
            string[] clientSessionIds = new string[] { };

            LTQRClientSession[] clientSessions = client.pull(ltqrReferences, paymentOrderReferences, clientSessionIds);

            Assert.NotNull(clientSessions);
            //Assert.AreEqual(1, clientSessions.Length);

            foreach (LTQRClientSession clientSession in clientSessions)
            {
                Console.WriteLine("Client session: " + clientSession.ToString());
            }
        }

        [Test]
        public void testRemove()
        {
            string[] ltqrReferences = new string[] { "5eddcdd6-eaa6-4552-b6f1-8fdeb14d44c0" };
            string[] paymentOrderReferences = new string[] { "DOTNET-LTQR-" };
            string[] clientSessionIds = new string[] { };

            // operate: fetch
            LTQRClientSession[] clientSessions = client.pull(ltqrReferences, paymentOrderReferences, clientSessionIds);
            Assert.NotNull(clientSessions);
            //Assert.AreEqual(2, clientSessions.Length);
            foreach (LTQRClientSession clientSession in clientSessions)
            {
                Console.WriteLine("Client session: " + clientSession.ToString());
            }

            // operate: remove
            client.remove(ltqrReferences, paymentOrderReferences, clientSessionIds);

            // operate: fetch again
            clientSessions = client.pull(ltqrReferences, paymentOrderReferences, clientSessionIds);
            Assert.AreEqual(0, clientSessions.Length);
        }
    }
}
