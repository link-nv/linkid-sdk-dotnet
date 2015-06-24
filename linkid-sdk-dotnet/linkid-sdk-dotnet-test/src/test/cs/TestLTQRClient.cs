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
            LinkIDPaymentContext paymentContext = new LinkIDPaymentContext(20000, LinkIDCurrency.EUR, ".NET Test", paymentOrderReference, null);
            DateTime expiryDate = DateTime.Now.AddMonths(3);
            LinkIDCallback callback = new LinkIDCallback("google.be", null, true);

            LTQRSession session = client.push(null, finishedMessage, paymentContext, false, expiryDate, null, callback, null, null, null, null, null, null);

            Assert.NotNull(session);
        }

        [Test]
        public void testChange()
        {
            String finishedMessage = "Changed custom finished msg";
            String ltqrReference = "f077493e-badd-4f13-91f1-bd3fd58dbd82";
            String paymentOrderReference = "DOTNET-LTQR-" + Guid.NewGuid().ToString();

            LinkIDPaymentContext paymentContext = new LinkIDPaymentContext(9999, LinkIDCurrency.EUR, ".NET Test Changed", paymentOrderReference, null);
            DateTime expiryDate = DateTime.Now.AddMonths(12);

            client.change(ltqrReference, null, finishedMessage, paymentContext, expiryDate, null, null, null, null, null, null, null, null, false);
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

        [Test]
        public void testInfo()
        {
            string[] ltqrReferences = new string[] { "56360436-efe0-451c-b078-2954e52700da", "54fa7902-5b05-4586-ba24-749921ff9aa1" };

            // operate: info
            List<LinkIDLTQRInfo> infos = client.info(ltqrReferences);

            // Verify
            Assert.NotNull(infos);
            Assert.AreEqual(ltqrReferences.Length, infos.Count);

            foreach (LinkIDLTQRInfo info in infos)
            {
                Console.WriteLine("LTQR Info: " + info);
            }
        }
    }
}
